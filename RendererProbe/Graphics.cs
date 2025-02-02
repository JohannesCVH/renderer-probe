﻿using Raylib_cs;
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

	public static Vector2 ToScreenSpaceVec2(Vector2 pos)
	{
		float x = ToScreenSpaceX(pos.X);
		float y = ToScreenSpaceY(pos.Y);

		return new Vector2() { X = x, Y = y };
	}

	public static Vector3 ScaleVector3(Vector3 vec, float scaleF)
	{
		vec.X = vec.X * scaleF;
		vec.Y = vec.Y * scaleF;
		vec.Z = vec.Z * scaleF;

		return vec;
	}

	public static Vector3 Vector3ToPerspective(Vector3 vec)
	{
		float fovRad = AngleToRad(WINDOW_FOV / 2);
		//Distance from camera to the projection plane.
		// 1 / tan(angle / 2)
		//float zNearDistance = 1 / ((float)Math.Tan(fovRad / 2));

		float z = Z_NEAR + vec.Z;

		//float z = vec.Z;
		float x = ((vec.X * WINDOW_ASPECT) * (1 / (float)Math.Tan(fovRad))) / z;
		float y = (vec.Y * (1 / (float)Math.Tan(fovRad))) / z;

		return new Vector3(x, y, z);
	}

	public static float AngleToRad(float angle)
	{
		return angle * ((float)Math.PI / 180.0f);
	}

	public static Vector3 Vector3ToWorldSpace(Vector3 pos, Vector3 coords)
	{
		pos.X = pos.X + coords.X;
		pos.Y = pos.Y + coords.Y;
		pos.Z = pos.Z + coords.Z;

		return pos;
	}
	
	public static Vector3 Vector3Normalize(Vector3 pos)
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
	public Vector3[] Vertices;

	public Triangle()
	{
		Vertices = new Vector3[3];
	}

	public Triangle(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
	{
		Vertices = [
			new Vector3(x1, y1, z1),
			new Vector3(x2, y2, z2),
			new Vector3(x3, y3, z3)
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

	public void DrawMesh(Vector3 meshPos)
	{
		meshPos = Graphics.Vector3Normalize(meshPos);
		Vector3 cameraPos = Graphics.Vector3Normalize(new Vector3(Camera.CAMERA_X, Camera.CAMERA_Y, 0.0f));
		
		//Calculate Middle
		float xMid = Triangles.Sum(x => x.Vertices[0].X + x.Vertices[1].X + x.Vertices[2].X) / (3.0f * Triangles.Length);
		float yMid = Triangles.Sum(x => x.Vertices[0].Y + x.Vertices[1].Y + x.Vertices[2].Y) / (3.0f * Triangles.Length);
		
		for (int i = 0; i < Triangles.Length; i++)
		{
			Triangle triangle = new Triangle(Triangles[i]);

			for (int j = 0; j < 3; j++)
			{
				float x = triangle.Vertices[j].X;
				float y = triangle.Vertices[j].Y;
				
				//Camera Position
				x = x - cameraPos.X;
				y = y - cameraPos.Y;

				//Translate to coordinates
				x += meshPos.X - xMid;
				y += meshPos.Y - yMid;

				//Camera Zoom
				x /= Camera.CAMERA_ZOOM;
				y /= Camera.CAMERA_ZOOM;
				
				//Set Final
				triangle.Vertices[j].X = x;
				triangle.Vertices[j].Y = y;

				//Triangles[i].Vertices[j] = Graphics.Vector3ToWorldSpace(Triangles[i].Vertices[j], coordinates);

				//triangle.Vertices[j] = Graphics.Vector3ToPerspective(triangle.Vertices[j]);
			}

			//To Screen Space
			triangle.Vertices[0] = Graphics.ToScreenSpaceVec3(triangle.Vertices[0]);
			triangle.Vertices[1] = Graphics.ToScreenSpaceVec3(triangle.Vertices[1]);
			triangle.Vertices[2] = Graphics.ToScreenSpaceVec3(triangle.Vertices[2]);

			//Only draw polygon 
			if (Graphics.CalculateNormal(triangle).Z < 0)
			{
				// Raylib.DrawTriangleLines(
				// 	new Vector2(triangle.Vertices[0].X, triangle.Vertices[0].Y),
				// 	new Vector2(triangle.Vertices[1].X, triangle.Vertices[1].Y),
				// 	new Vector2(triangle.Vertices[2].X, triangle.Vertices[2].Y),
				// 	Color.White
				// );

				Raylib.DrawTriangle(
					new Vector2(triangle.Vertices[0].X, triangle.Vertices[0].Y),
					new Vector2(triangle.Vertices[1].X, triangle.Vertices[1].Y),
					new Vector2(triangle.Vertices[2].X, triangle.Vertices[2].Y),
					Color.White
				);
			}

		}
	}
}