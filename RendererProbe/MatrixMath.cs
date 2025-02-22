using System.Numerics;
using static RendererProbe.Globals;

namespace RendererProbe;

public struct Mat4x4
{
	public float[,] M;

	public Mat4x4()
	{
		M = new float[4,4];
	}
}

public static class MatrixMath
{
	public static Vector4 MultiplyVector(this Vector4 vec, Mat4x4 matrix)
	{
		Vector4 retVec;
		retVec.X = (vec.X * matrix.M[0,0]) + (vec.Y * matrix.M[0,1]) + (vec.Z * matrix.M[0,2] + (vec.W * matrix.M[0,3]));
		retVec.Y = (vec.X * matrix.M[1,0]) + (vec.Y * matrix.M[1,1]) + (vec.Z * matrix.M[1,2] + (vec.W * matrix.M[1,3]));
		retVec.Z = (vec.X * matrix.M[2,0]) + (vec.Y * matrix.M[2,1]) + (vec.Z * matrix.M[2,2] + (vec.W * matrix.M[2,3]));
		retVec.W = (vec.X * matrix.M[3,0]) + (vec.Y * matrix.M[3,1]) + (vec.Z * matrix.M[3,2] + (vec.W * matrix.M[3,3]));
		
		return retVec;
	}
	
	public static Mat4x4 CreateRotationMatrix_Pitch(float angleRad)
	{
		Mat4x4 matrix = new Mat4x4();
		
		matrix.M[0,0] = 1.0f;
		matrix.M[0,1] = 0.0f;
		matrix.M[0,2] = 0.0f;
		matrix.M[0,3] = 0.0f;
		
		matrix.M[1,0] = 0.0f;
		matrix.M[1,1] = (float)Math.Cos(angleRad);
		matrix.M[1,2] = -(float)Math.Sin(angleRad);
		matrix.M[1,3] = 0.0f;
		
		matrix.M[2,0] = 0.0f;
		matrix.M[2,1] = (float)Math.Sin(angleRad);
		matrix.M[2,2] = (float)Math.Cos(angleRad);
		matrix.M[2,3] = 0.0f;
		
		matrix.M[3,0] = 0.0f;
		matrix.M[3,1] = 0.0f;
		matrix.M[3,2] = 0.0f;
		matrix.M[3,3] = 1.0f;
		
		return matrix;
	}
	
	public static Mat4x4 CreateRotationMatrix_Yaw(float angleRad)
	{
		Mat4x4 matrix = new Mat4x4();
		
		matrix.M[0,0] = (float)Math.Cos(angleRad);
		matrix.M[0,1] = 0.0f;
		matrix.M[0,2] = (float)Math.Sin(angleRad);
		matrix.M[0,3] = 0.0f;
		
		matrix.M[1,0] = 0.0f;
		matrix.M[1,1] = 1.0f;
		matrix.M[1,2] = 0.0f;
		matrix.M[1,3] = 0.0f;
		
		matrix.M[2,0] = -(float)Math.Sin(angleRad);
		matrix.M[2,1] = 0.0f;
		matrix.M[2,2] = (float)Math.Cos(angleRad);
		matrix.M[2,3] = 0.0f;
		
		matrix.M[3,0] = 0.0f;
		matrix.M[3,1] = 0.0f;
		matrix.M[3,2] = 0.0f;
		matrix.M[3,3] = 1.0f;
		
		return matrix;
	}
	
	public static Mat4x4 CreateRotationMatrix_Roll(float angleRad)
	{
		Mat4x4 matrix = new Mat4x4();
		
		matrix.M[0,0] = (float)Math.Cos(angleRad);
		matrix.M[0,1] = -(float)Math.Sin(angleRad);
		matrix.M[0,2] = 0.0f;
		matrix.M[0,3] = 0.0f;
		
		matrix.M[1,0] = (float)Math.Sin(angleRad);
		matrix.M[1,1] = (float)Math.Cos(angleRad);
		matrix.M[1,2] = 0.0f;
		matrix.M[1,3] = 0.0f;
		
		matrix.M[2,0] = 0.0f;
		matrix.M[2,1] = 0.0f;
		matrix.M[2,2] = 1.0f;
		matrix.M[2,3] = 0.0f;
		
		matrix.M[3,0] = 0.0f;
		matrix.M[3,1] = 0.0f;
		matrix.M[3,2] = 0.0f;
		matrix.M[3,3] = 1.0f;
		
		return matrix;
	}

	public static Mat4x4 CreateScaleMatrix(float scale)
	{
		Mat4x4 matrix = new Mat4x4();

		matrix.M[0,0] = scale;
		matrix.M[0,1] = 0.0f;
		matrix.M[0,2] = 0.0f;
		matrix.M[0,3] = 0.0f;
				
		matrix.M[1,0] = 0.0f;
		matrix.M[1,1] = scale;
		matrix.M[1,2] = 0.0f;
		matrix.M[1,3] = 0.0f;
				
		matrix.M[2,0] = 0.0f;
		matrix.M[2,1] = 0.0f;
		matrix.M[2,2] = scale;
		matrix.M[2,3] = 0.0f;
				
		matrix.M[3,0] = 0.0f;
		matrix.M[3,1] = 0.0f;
		matrix.M[3,2] = 0.0f;
		matrix.M[3,3] = 1.0f;
				
		return matrix;
	}
	
	public static Mat4x4 CreateTranslationMatrix(Vector4 pos)
	{
		Mat4x4 matrix = new Mat4x4();

		matrix.M[0,0] = 1.0f;
		matrix.M[0,1] = 0.0f;
		matrix.M[0,2] = 0.0f;
		matrix.M[0,3] = pos.X;
				
		matrix.M[1,0] = 0.0f;
		matrix.M[1,1] = 1.0f;
		matrix.M[1,2] = 0.0f;
		matrix.M[1,3] = pos.Y;
				
		matrix.M[2,0] = 0.0f;
		matrix.M[2,1] = 0.0f;
		matrix.M[2,2] = 1.0f;
		matrix.M[2,3] = pos.Z;
				
		matrix.M[3,0] = 0.0f;
		matrix.M[3,1] = 0.0f;
		matrix.M[3,2] = 0.0f;
		matrix.M[3,3] = 1.0f;
				
		return matrix;
	}

	public static Mat4x4 CreateAspectMatrix()
	{
		Mat4x4 matrix = new Mat4x4();

		matrix.M[0,0] = WINDOW_ASPECT;
		matrix.M[0,1] = 0.0f;
		matrix.M[0,2] = 0.0f;
		matrix.M[0,3] = 0.0f;
				
		matrix.M[1,0] = 0.0f;
		matrix.M[1,1] = 1.0f;
		matrix.M[1,2] = 0.0f;
		matrix.M[1,3] = 0.0f;
				
		matrix.M[2,0] = 0.0f;
		matrix.M[2,1] = 0.0f;
		matrix.M[2,2] = 1.0f;
		matrix.M[2,3] = 0.0f;
				
		matrix.M[3,0] = 0.0f;
		matrix.M[3,1] = 0.0f;
		matrix.M[3,2] = 0.0f;
		matrix.M[3,3] = 1.0f;
				
		return matrix;
	}
	
	public static Mat4x4 CreatePerspectiveMatrix(Vector4 vec)
	{
		Mat4x4 matrix = new Mat4x4();

		var fov_half = WINDOW_FOV / 2;

		matrix.M[0,0] = WINDOW_ASPECT * (1 / ((float)Math.Tan(Graphics.AngleToRad(fov_half)) * vec.Z));
		matrix.M[0,1] = 0.0f;
		matrix.M[0,2] = 0.0f;
		matrix.M[0,3] = 0.0f;
				
		matrix.M[1,0] = 0.0f;
		matrix.M[1,1] = 1 / ((float)Math.Tan(Graphics.AngleToRad(fov_half)) * vec.Z);
		matrix.M[1,2] = 0.0f;
		matrix.M[1,3] = 0.0f;
				
		matrix.M[2,0] = 0.0f;
		matrix.M[2,1] = 0.0f;
		// matrix.M[2,2] = 1.0f;
		// matrix.M[2,3] = 0.0f;
		matrix.M[2,2] = Z_FAR / (Z_FAR - Z_NEAR);
		matrix.M[2,3] = -(Z_FAR * Z_NEAR / (Z_FAR - Z_NEAR));
				
		matrix.M[3,0] = 0.0f;
		matrix.M[3,1] = 0.0f;
		matrix.M[3,2] = 0.0f;
		matrix.M[3,3] = 1.0f;
				
		return matrix;
	}
}
