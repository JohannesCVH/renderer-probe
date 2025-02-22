using Raylib_cs;
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
}

public struct Triangle
{
	public Vector4[] Vertices;

	public Triangle()
	{
		Vertices = new Vector4[3];
	}

	public Triangle(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
	{
		Vertices = [
			new Vector4(x1, y1, z1, 1),
			new Vector4(x2, y2, z2, 1),
			new Vector4(x3, y3, z3, 1)
		];
	}

	public Triangle(Triangle triangle)
	{
		Vertices = [
			triangle.Vertices[0],
			triangle.Vertices[1],
			triangle.Vertices[2]
		];
	}
}

public struct Mesh
{
	public Triangle[] Triangles;

	public void DrawMesh(Vector4 meshPos, float scale, float angle)
	{
		// meshPos = Graphics.Vector4Normalize(meshPos);
		Vector4 cameraPos = Graphics.Vector4Normalize(new Vector4(Camera.CAMERA_X, Camera.CAMERA_Y, Camera.CAMERA_Z, 0.0f));
		
		//Calculate Middle
		float xMid = Triangles.Sum(t => t.Vertices[0].X + t.Vertices[1].X + t.Vertices[2].X) / (3.0f * Triangles.Length);
		float yMid = Triangles.Sum(t => t.Vertices[0].Y + t.Vertices[1].Y + t.Vertices[2].Y) / (3.0f * Triangles.Length);
		float zMid = Triangles.Sum(t => t.Vertices[0].Z + t.Vertices[1].Z + t.Vertices[2].Z) / (3.0f * Triangles.Length);

		float angleRad = Graphics.AngleToRad(angle);
		
		for (int i = 0; i < Triangles.Length; i++)
		{
			Triangle triangle = new Triangle(Triangles[i]);

			for (int j = 0; j < 3; j++)
			{	
				// float x = triangle.Vertices[j].X;
				// float y = triangle.Vertices[j].Y;
				// float z = triangle.Vertices[j].Z;
				
				//Camera Position
				// x = x - cameraPos.X;
				// y = y - cameraPos.Y;
				
				//Set Final
				// triangle.Vertices[j].X = x;
				// triangle.Vertices[j].Y = y;

				// Triangles[i].Vertices[j] = Graphics.Vector3ToWorldSpace(Triangles[i].Vertices[j], coordinates);

				//triangle.Vertices[j] = Graphics.Vector3ToPerspective(triangle.Vertices[j]);

				//Translate to coordinates
				triangle.Vertices[j].X -= xMid;
				triangle.Vertices[j].Y -= yMid;
				triangle.Vertices[j].Z -= zMid;
				
				//Rotation
				triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateRotationMatrix_Pitch(angleRad));
				triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateRotationMatrix_Yaw(Graphics.AngleToRad(angle)));
				// triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateRotationMatrix_Roll(Graphics.AngleToRad(angle)));
				
				//Scale
				triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateScaleMatrix(scale));
				
				//Translation
				triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreateTranslationMatrix(meshPos));
				
				//Perspective Projection
				if (PERSPECTIVE)
				{
					triangle.Vertices[j] = triangle.Vertices[j].MultiplyVector(MatrixMath.CreatePerspectiveMatrix(triangle.Vertices[j]));
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
			bool shouldDraw = false;
			Vector3 triNormal = Graphics.CalculateNormal(triangle);
			if (
				(triNormal.X * (triangle.Vertices[0].X - Camera.CAMERA_X)) +
				(triNormal.Y * (triangle.Vertices[0].Y - Camera.CAMERA_Y)) +
				(triNormal.Z * (triangle.Vertices[0].Z - Camera.CAMERA_Z)) < 0.1f
			)
				shouldDraw = true;

			//To Screen Space
			triangle.Vertices[0] = Graphics.ToScreenSpaceVec4(triangle.Vertices[0]);
			triangle.Vertices[1] = Graphics.ToScreenSpaceVec4(triangle.Vertices[1]);
			triangle.Vertices[2] = Graphics.ToScreenSpaceVec4(triangle.Vertices[2]);

			//Only draw polygon
			// if (Graphics.CalculateNormal(triangle).Z > 0)
			if (shouldDraw)
			{
				Raylib.DrawTriangle(
					new Vector2(triangle.Vertices[0].X, triangle.Vertices[0].Y),
					new Vector2(triangle.Vertices[1].X, triangle.Vertices[1].Y),
					new Vector2(triangle.Vertices[2].X, triangle.Vertices[2].Y),
					Color.White
				);
				
				// Raylib.DrawTriangleLines(
				// 	new Vector2(triangle.Vertices[0].X, triangle.Vertices[0].Y),
				// 	new Vector2(triangle.Vertices[1].X, triangle.Vertices[1].Y),
				// 	new Vector2(triangle.Vertices[2].X, triangle.Vertices[2].Y),
				// 	Color.Black
				// );
			}
		}
	}
}