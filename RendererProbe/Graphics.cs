using SFML.Graphics;
using SFML.System;
using System.Numerics;
using static RendererProbe.Globals;

namespace RendererProbe;

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

	public static Vector3 ToScreenSpaceVec3(Vector3 pos)
	{
		float x = ToScreenSpaceX(pos.X);
		float y = ToScreenSpaceY(pos.Y);

		return new Vector3(){ X = x, Y = y, Z = pos.Z };
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

	public static Vector3 CalculateNormal(Triangle triangle)
	{
		Vector3 normal, line1, line2;

		line1.X = triangle.Vertices[1].X - triangle.Vertices[0].X;
		line1.Y = triangle.Vertices[1].Y - triangle.Vertices[0].Y;
		line1.Z = triangle.Vertices[1].Z - triangle.Vertices[0].Z;

		line2.X = triangle.Vertices[2].X - triangle.Vertices[0].X;
		line2.Y = triangle.Vertices[2].Y - triangle.Vertices[0].Y;
		line2.Z = triangle.Vertices[2].Z - triangle.Vertices[0].Z;

		normal.X = (line1.Y * line2.Z) - (line1.Z * line2.Y);
		normal.Y = (line1.Z * line2.X) - (line1.X * line2.Z);
		normal.Z = (line1.X * line2.Y) - (line1.Y * line2.X);

		float normalL = (float)Math.Sqrt((normal.X * normal.X) + (normal.Y * normal.Y) + (normal.Z * normal.Z));
		normal.X /= normalL;
		normal.Y /= normalL;
		normal.Z /= normalL;

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
	public Triangle[] Triangles;

	public void DrawMesh(RenderWindow window, Vector4 meshPos, float scale, float angle)
	{
		float angleRad = Graphics.AngleToRad(angle);
		
		for (int i = 0; i < Triangles.Length; i++)
		{
			Triangle triangle = new Triangle(Triangles[i]);

			for (int j = 0; j < 3; j++)
			{	
				//Scale
				triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateScaleMatrix(scale));

				//Rotation
				triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateRotationMatrix_Pitch(Graphics.AngleToRad(angleRad)));
				triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateRotationMatrix_Yaw(Graphics.AngleToRad(angle)));
				triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateRotationMatrix_Roll(Graphics.AngleToRad(angle)));
				
				//Translation
				triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateTranslationMatrix(meshPos));
				
				//Perspective Projection
				if (PERSPECTIVE)
				{
					triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreatePerspectiveMatrix());
				}
				else
				{
					triangle.Vertices[j].X = WINDOW_ASPECT * triangle.Vertices[j].X;
				}
			}

			//Normalize
			// triangle.Vertices[0] = Graphics.Vector4Normalize(triangle.Vertices[0]);
			// triangle.Vertices[1] = Graphics.Vector4Normalize(triangle.Vertices[1]);
			// triangle.Vertices[2] = Graphics.Vector4Normalize(triangle.Vertices[2]);

			//Should Draw
			bool shouldDraw = ShouldDraw(triangle);

			if (shouldDraw)
			{
				//Illumination
				Vector3 triNormal = Graphics.CalculateNormal(triangle);
				Vector4 lightDir = new Vector4(0.0f, 0.0f, -1.0f, 0.0f);
				float lightDirL = (float)Math.Sqrt((lightDir.X * lightDir.X) + (lightDir.Y * lightDir.Y) + (lightDir.Z * lightDir.Z));
				lightDir.X /= lightDirL;
				lightDir.Y /= lightDirL;
				lightDir.Z /= lightDirL;

				float lightDot = (triNormal.X * lightDir.X) + (triNormal.Y * lightDir.Y) + (triNormal.Z * lightDir.Z);
				byte lightDotAdj = (byte)((lightDot + 1) / 2 * 255);

				Color colorShade = new Color(lightDotAdj, lightDotAdj, lightDotAdj, 255);
				
				//To Screen Space
				triangle.Vertices[0] = Graphics.ToScreenSpaceVec4(triangle.Vertices[0]);
				triangle.Vertices[1] = Graphics.ToScreenSpaceVec4(triangle.Vertices[1]);
				triangle.Vertices[2] = Graphics.ToScreenSpaceVec4(triangle.Vertices[2]);

				VertexArray vaTri = new VertexArray(PrimitiveType.Triangles, 3);
				vaTri.Append(new Vertex(new Vector2f(triangle.Vertices[0].X, triangle.Vertices[0].Y), colorShade));
				vaTri.Append(new Vertex(new Vector2f(triangle.Vertices[1].X, triangle.Vertices[1].Y), colorShade));
				vaTri.Append(new Vertex(new Vector2f(triangle.Vertices[2].X, triangle.Vertices[2].Y), colorShade));

				window.Draw(vaTri);
			}
		}
	}
	private bool ShouldDraw(Triangle tri)
	{
		Vector3 triNormal = Graphics.CalculateNormal(tri);

		float sum = 
			(triNormal.X * (tri.Vertices[0].X - Camera.CAMERA_X)) +
			(triNormal.Y * (tri.Vertices[0].Y - Camera.CAMERA_Y)) +
			(triNormal.Z * (tri.Vertices[0].Z - Camera.CAMERA_Z));
		
		if (sum < 0.0f) return true;
		else return false;
	}
}