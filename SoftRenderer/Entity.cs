using System.Numerics;
using SFML.Graphics;
using static SoftRenderer.MathLib;
using static SoftRenderer.Graphics;
using static SoftRenderer.Globals;
using static SoftRenderer.Constants;

namespace SoftRenderer;

public class Entity
{
	public Vector4 Position { get; set; }

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

	//Rotation Matrices
	// Mat4x4 

	Mat4x4 ScaleMatrix { get; set; }
	Mat4x4 RotationMatrix { get; set; }
	Mat4x4 TransMatrix { get; set; }
	Mat4x4 WorldMatrix { get; set; }
	

	public Entity(Vector4 pos, float scaleF, float angle, Triangle[] triangles)
	{
		Position = pos;
		ScaleFactor = scaleF;
		Angle = angle;
		Mesh = new Mesh(triangles);
		Update();
	}

	public void Update()
	{
		float angleRad = AngleToRad(Angle);
		
		ScaleMatrix = CreateScaleMatrix(ScaleFactor);
		RotationMatrix = MatMulMat(ScaleMatrix, CreateRotationMatrix_Pitch(angleRad));
		RotationMatrix = MatMulMat(RotationMatrix, CreateRotationMatrix_Yaw(angleRad));
		RotationMatrix = MatMulMat(RotationMatrix, CreateRotationMatrix_Roll(angleRad));
		TransMatrix = MatMulMat(RotationMatrix, CreateTranslationMatrix(Position));
		WorldMatrix = MatMulMat(TransMatrix, CreateIdentityMatrix());
	}

	public void Draw(RenderWindow window)
	{
		Mesh.DrawMesh(window, WorldMatrix, ScaleFactor, Angle);
	}

	public void Rotate(float? angle = null)
	{   
		if (UPDATE_TIMER.ElapsedMilliseconds < UPDATE_INTERVAL)
			return;

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