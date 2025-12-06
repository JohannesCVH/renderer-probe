using SFML.Graphics;
using SFML.System;
using System.Numerics;
using static SoftRenderer.Globals;
using static SoftRenderer.MathLib;

namespace SoftRenderer;

public static class Graphics
{
	public static int ToScreenSpaceX(float coord)
	{
		return (int)Math.Floor(WINDOW_WIDTH_HALF + (coord * WINDOW_WIDTH_HALF));
	}

	public static int ToScreenSpaceY(float coord)
	{
		return (int)Math.Floor(WINDOW_HEIGHT_HALF - (coord * WINDOW_HEIGHT_HALF));
	}

	public static Vector4 ToScreenSpaceVec4(Vector4 pos)
	{
		float x = ToScreenSpaceX(pos.X);
		float y = ToScreenSpaceY(pos.Y);

		return new Vector4(){ X = x, Y = y, Z = pos.Z };
	}

	public static Vector2 ToScreenSpaceVec2(Vector2 pos)
	{
		float x = ToScreenSpaceX(pos.X);
		float y = ToScreenSpaceY(pos.Y);

		return new Vector2() { X = x, Y = y };
	}

	public static float AngleToRad(float angle)
	{
		return angle * ((float)Math.PI / 180.0f);
	}
	
	public static Vector4 Vector4Normalize(Vector4 pos)
	{
		pos.X = pos.X / WORLD_SIZE;
		pos.Y = pos.Y / WORLD_SIZE;
		pos.Z = pos.Z / WORLD_SIZE;
		
		return pos;
	}

	public static Vector4 CalculateNormal(Triangle triangle)
	{
		Vector4 normal, line1, line2;

		line1 = VecSub(triangle.Vertices[1], triangle.Vertices[0]);
		line2 = VecSub(triangle.Vertices[2], triangle.Vertices[0]);

		normal = VecCrossProd(line1, line2);
		normal = VecNorm(normal);

		return normal;
	}

	public static float CalculateDotProduct(Vector3 vecA, Vector3 vecB)
	{
		float dotProduct = (vecA.X * vecB.X) + (vecA.Y * vecB.Y) + (vecA.Z * vecB.Z);
		return dotProduct;
	}
}

public struct Triangle
{
	public Vector4[] Vertices;
	public Color Color = Color.White;

	public Triangle()
	{
		Vertices = new Vector4[3];
	}

	public Triangle(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3, Color? color = default)
	{
		Vertices = [
			new Vector4(x1, y1, z1, 1),
			new Vector4(x2, y2, z2, 1),
			new Vector4(x3, y3, z3, 1)
		];

		Color = color ?? Color.White;
	}

	public Triangle(Triangle triangle)
	{
		Vertices = [
			triangle.Vertices[0],
			triangle.Vertices[1],
			triangle.Vertices[2]
		];

		Color = triangle.Color;
	}
}

public struct Mesh
{
	private Triangle[] Triangles;
	private Triangle?[] TrianglesOrdered;

	public Mesh(Triangle[] triangles)
	{
		Triangles = triangles;
		TrianglesOrdered = new Triangle?[Triangles.Length];
	}

	public void DrawMesh(RenderWindow window, Mat4x4 worldMatrix, float scale, float angle)
	{

		float angleRad = Graphics.AngleToRad(angle);
		
		for (int i = 0; i < Triangles.Length; i++)
		{
			Triangle triangle = new Triangle(Triangles[i]);

			for (int j = 0; j < 3; j++)
			{
				triangle.Vertices[j] = MatMulVec(worldMatrix, triangle.Vertices[j]);
				
				if (PERSPECTIVE)
				{
					triangle.Vertices[j] = MatMulVec(CreatePerspectiveMatrix(), triangle.Vertices[j]);
				}
				else
				{
					triangle.Vertices[j].X = WINDOW_ASPECT * triangle.Vertices[j].X;
				}
			}

			//If the tri normal is not  in the camera direction then skip
			if (VecDot(Graphics.CalculateNormal(triangle), Camera.CAMERA_DIRECTION) > 0.0f)
				continue;

			//Illumination
			Vector4 triNormal = Graphics.CalculateNormal(triangle);
			Vector4 lightDir = new Vector4(0.0f, 0.0f, -1.0f, 0.0f);
			lightDir = VecNorm(lightDir);

			float lightDot = VecDot(triNormal, lightDir);
			byte lightDotAdj = (byte)((lightDot + 1) / 2 * 255);

			triangle.Color = new Color(lightDotAdj, lightDotAdj, lightDotAdj, 255);

			//Normalize
			triangle.Vertices[0] = VecDiv(triangle.Vertices[0], triangle.Vertices[0].W);
			triangle.Vertices[1] = VecDiv(triangle.Vertices[1], triangle.Vertices[1].W);
			triangle.Vertices[2] = VecDiv(triangle.Vertices[2], triangle.Vertices[2].W);
			
			//To Screen Space
			triangle.Vertices[0] = Graphics.ToScreenSpaceVec4(triangle.Vertices[0]);
			triangle.Vertices[1] = Graphics.ToScreenSpaceVec4(triangle.Vertices[1]);
			triangle.Vertices[2] = Graphics.ToScreenSpaceVec4(triangle.Vertices[2]);

			TrianglesOrdered[i] = triangle;
		}

		//Sort Vertex Array by Z depth and then draw
		Array.Sort(TrianglesOrdered, CompareTriDepth);
		// Console.WriteLine("\n\n");
		
		Array.ForEach(TrianglesOrdered.Where(x => x != null).ToArray(), x => {
			float zDepth = (x.Value.Vertices[0].Z + x.Value.Vertices[1].Z + x.Value.Vertices[2].Z) / 3;
			// Console.WriteLine($"Z Depth: {zDepth}");

			VertexArray vaTri = new VertexArray(PrimitiveType.Triangles, 3);
			vaTri.Append(new Vertex(new Vector2f(x.Value.Vertices[0].X, x.Value.Vertices[0].Y), x.Value.Color));
			vaTri.Append(new Vertex(new Vector2f(x.Value.Vertices[1].X, x.Value.Vertices[1].Y), x.Value.Color));
			vaTri.Append(new Vertex(new Vector2f(x.Value.Vertices[2].X, x.Value.Vertices[2].Y), x.Value.Color));

			window.Draw(vaTri);

			if (DRAW_LINES)
            {
                VertexArray vaTriLines1 = new VertexArray(PrimitiveType.Lines, 2);
				vaTriLines1.Append(new Vertex(new Vector2f(x.Value.Vertices[0].X, x.Value.Vertices[0].Y), Color.Black));
				vaTriLines1.Append(new Vertex(new Vector2f(x.Value.Vertices[1].X, x.Value.Vertices[1].Y), Color.Black));
				VertexArray vaTriLines2 = new VertexArray(PrimitiveType.Lines, 2);
				vaTriLines2.Append(new Vertex(new Vector2f(x.Value.Vertices[1].X, x.Value.Vertices[1].Y), Color.Black));
				vaTriLines2.Append(new Vertex(new Vector2f(x.Value.Vertices[2].X, x.Value.Vertices[2].Y), Color.Black));
				VertexArray vaTriLines3 = new VertexArray(PrimitiveType.Lines, 2);
				vaTriLines3.Append(new Vertex(new Vector2f(x.Value.Vertices[2].X, x.Value.Vertices[2].Y), Color.Black));
				vaTriLines3.Append(new Vertex(new Vector2f(x.Value.Vertices[0].X, x.Value.Vertices[0].Y), Color.Black));
				
				window.Draw(vaTriLines1);
				window.Draw(vaTriLines2);
				window.Draw(vaTriLines3);
            }
		});
		Array.Fill(TrianglesOrdered, null);
	}

	public int CompareTriDepth(Triangle? tri1, Triangle? tri2)
	{
		if (tri1 == null) return 1;
		if (tri2 == null) return -1;
		
		float tri1Depth = (tri1.Value.Vertices[0].Z + tri1.Value.Vertices[1].Z + tri1.Value.Vertices[2].Z) / 3;
		float tri2Depth = (tri2.Value.Vertices[0].Z + tri2.Value.Vertices[1].Z + tri2.Value.Vertices[2].Z) / 3;

		return tri1Depth < tri2Depth ? -1 : 1;
	}
}