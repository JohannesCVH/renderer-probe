using System.Numerics;
using SFML.Graphics;

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
	public Entity(Vector4 pos, float scaleF, float angle, Triangle[] triangles)
	{
		PositionX = pos.X;
		PositionY = pos.Y;
		PositionZ = pos.Z;
		ScaleFactor = scaleF;
		Angle = angle;
		Mesh = new Mesh { Triangles = triangles };
	}

	public void Draw(RenderWindow window)
	{
		Mesh.DrawMesh(window, new Vector4(PositionX, PositionY, PositionZ, 0), ScaleFactor, Angle);
	}

	public void Rotate(float? angle = null)
	{   
		Angle += angle ?? Rotation;
	}

	public void RotateRoll(float? angle = null)
	{   
		Angle += angle ?? Rotation;
	}

	public void RotateYaw(float? angle = null)
	{   
		Angle += angle ?? Rotation;
	}

	public void RotatePitch(float? angle = null)
	{   
		Angle += angle ?? Rotation;
	}
}