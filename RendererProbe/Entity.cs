using System.Numerics;

namespace RendererProbe;

public class Entity
{
	public float PositionX { get; set; }
	public float PositionY { get; set; }
	public float PositionZ { get; set; }

	public float ScaleFactor { get; set; }
	public float Rotation { get; set; }
	private float _angle;
	public float Angle { 
		get { return _angle; } 
		set {
			if (_angle + value >= 360) 
				_angle = 0 + value;
			else if (_angle + value < 0) 
				_angle = 360 + value;
			else _angle = value;
		}
	}
	public Mesh Mesh { get; set; }
	public Entity(Vector3 pos, float scaleF, float angle, Triangle[] triangles)
	{
		PositionX = pos.X;
		PositionY = pos.Y;
		PositionZ = pos.Z;
		ScaleFactor = scaleF;
		Angle = angle;
		Mesh = new Mesh { Triangles = triangles };
		Scale();
	}

	public void Draw()
	{
		Mesh.DrawMesh(new Vector3(PositionX, PositionY, PositionZ));
	}

	public void Rotate(float? angle = null)
	{   
		Angle += angle ?? Rotation;

		float angleRad = Graphics.AngleToRad(angle ?? Rotation);

		for (int i = 0; i < Mesh.Triangles.Length; i++)
		{
			for (int j = 0; j < Mesh.Triangles[i].Vertices.Length; j++)
			{
				float x = Mesh.Triangles[i].Vertices[j].X;
				float y = Mesh.Triangles[i].Vertices[j].Y;
				float z = Mesh.Triangles[i].Vertices[j].Z;

				float x2 = x * (float)Math.Cos(angleRad) - (y * (float)Math.Sin(angleRad));
				float y2 = x * (float)Math.Sin(angleRad) + (y * (float)Math.Cos(angleRad));

				Mesh.Triangles[i].Vertices[j].X = x2;
				Mesh.Triangles[i].Vertices[j].Y = y2;
				Mesh.Triangles[i].Vertices[j].Z = z;
			}
		}
	}

	public void Scale()
	{
		for (int i = 0; i < Mesh.Triangles.Length; i++)
		{
			for (int j = 0; j < Mesh.Triangles[i].Vertices.Length; j++)
			{
				Mesh.Triangles[i].Vertices[j].X *= ScaleFactor / Constants.WORLD_SIZE;
				Mesh.Triangles[i].Vertices[j].Y *= ScaleFactor / Constants.WORLD_SIZE;
				Mesh.Triangles[i].Vertices[j].Z *= ScaleFactor / Constants.WORLD_SIZE;
			}
		}
	}
}