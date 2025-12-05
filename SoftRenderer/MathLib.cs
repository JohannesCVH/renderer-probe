using System.Numerics;
using static SoftRenderer.Globals;

namespace SoftRenderer;

public struct Mat4x4
{
	public float[,] M;

	public Mat4x4()
	{
		M = new float[4,4];
	}
}

public static class MathLib
{
	public static Mat4x4 MatMulMat(Mat4x4 m1, Mat4x4 m2)
	{
		Mat4x4 matrix = new Mat4x4();
		for (int row = 0; row < 4; row++)
		{
			for (int col = 0; col < 4; col++)
			{
				matrix.M[row,col] = 
					m1.M[row,0] * m2.M[0,col] +
					m1.M[row,1] * m2.M[1,col] +
					m1.M[row,2] * m2.M[2,col] +
					m1.M[row,3] * m2.M[3,col];
			}
		}
		return matrix;
	}
	
	public static Vector4 MatMulVec(Mat4x4 matrix, Vector4 vec)
	{
		Vector4 retVec = new Vector4();
		retVec.X = (vec.X * matrix.M[0,0]) + (vec.Y * matrix.M[1,0]) + (vec.Z * matrix.M[2,0]) + (vec.W * matrix.M[3,0]);
		retVec.Y = (vec.X * matrix.M[0,1]) + (vec.Y * matrix.M[1,1]) + (vec.Z * matrix.M[2,1]) + (vec.W * matrix.M[3,1]);
		retVec.Z = (vec.X * matrix.M[0,2]) + (vec.Y * matrix.M[1,2]) + (vec.Z * matrix.M[2,2]) + (vec.W * matrix.M[3,2]);
		retVec.W = (vec.X * matrix.M[0,3]) + (vec.Y * matrix.M[1,3]) + (vec.Z * matrix.M[2,3]) + (vec.W * matrix.M[3,3]);
		
		return retVec;
	}

	public static Mat4x4 CreateIdentityMatrix()
	{
		Mat4x4 matrix = new Mat4x4();
		matrix.M[0,0] = 1.0f;
		matrix.M[1,1] = 1.0f;
		matrix.M[2,2] = 1.0f;
		matrix.M[3,3] = 1.0f;
		return matrix;
	}
	
	public static Mat4x4 CreateRotationMatrix_Pitch(float angleRad)
	{
		Mat4x4 matrix = new Mat4x4();
		
		matrix.M[0,0] = 1.0f;
		matrix.M[1,0] = 0.0f;
		matrix.M[2,0] = 0.0f;
		matrix.M[3,0] = 0.0f;
		
		matrix.M[0,1] = 0.0f;
		matrix.M[1,1] = (float)Math.Cos(angleRad);
		matrix.M[2,1] = -(float)Math.Sin(angleRad);
		matrix.M[3,1] = 0.0f;
		
		matrix.M[0,2] = 0.0f;
		matrix.M[1,2] = (float)Math.Sin(angleRad);
		matrix.M[2,2] = (float)Math.Cos(angleRad);
		matrix.M[3,2] = 0.0f;
		
		matrix.M[0,3] = 0.0f;
		matrix.M[1,3] = 0.0f;
		matrix.M[2,3] = 0.0f;
		matrix.M[3,3] = 1.0f;
		
		return matrix;
	}
	
	public static Mat4x4 CreateRotationMatrix_Yaw(float angleRad)
	{
		Mat4x4 matrix = new Mat4x4();
		
		matrix.M[0,0] = (float)Math.Cos(angleRad);
		matrix.M[1,0] = 0.0f;
		matrix.M[2,0] = (float)Math.Sin(angleRad);
		matrix.M[3,0] = 0.0f;
		
		matrix.M[0,1] = 0.0f;
		matrix.M[1,1] = 1.0f;
		matrix.M[2,1] = 0.0f;
		matrix.M[3,1] = 0.0f;
		
		matrix.M[0,2] = -(float)Math.Sin(angleRad);
		matrix.M[1,2] = 0.0f;
		matrix.M[2,2] = (float)Math.Cos(angleRad);
		matrix.M[3,2] = 0.0f;
		
		matrix.M[0,3] = 0.0f;
		matrix.M[1,3] = 0.0f;
		matrix.M[2,3] = 0.0f;
		matrix.M[3,3] = 1.0f;
		
		return matrix;
	}
	
	public static Mat4x4 CreateRotationMatrix_Roll(float angleRad)
	{
		Mat4x4 matrix = new Mat4x4();
		
		matrix.M[0,0] = (float)Math.Cos(angleRad);
		matrix.M[1,0] = -(float)Math.Sin(angleRad);
		matrix.M[2,0] = 0.0f;
		matrix.M[3,0] = 0.0f;
		
		matrix.M[0,1] = (float)Math.Sin(angleRad);
		matrix.M[1,1] = (float)Math.Cos(angleRad);
		matrix.M[2,1] = 0.0f;
		matrix.M[3,1] = 0.0f;
		
		matrix.M[0,2] = 0.0f;
		matrix.M[1,2] = 0.0f;
		matrix.M[2,2] = 1.0f;
		matrix.M[3,2] = 0.0f;
		
		matrix.M[0,3] = 0.0f;
		matrix.M[1,3] = 0.0f;
		matrix.M[2,3] = 0.0f;
		matrix.M[3,3] = 1.0f;
		
		return matrix;
	}

	public static Mat4x4 CreateScaleMatrix(float scale)
	{
		Mat4x4 matrix = new Mat4x4();

		matrix.M[0,0] = scale;
		matrix.M[1,0] = 0.0f;
		matrix.M[2,0] = 0.0f;
		matrix.M[3,0] = 0.0f;
				
		matrix.M[0,1] = 0.0f;
		matrix.M[1,1] = scale;
		matrix.M[2,1] = 0.0f;
		matrix.M[3,1] = 0.0f;
				
		matrix.M[0,2] = 0.0f;
		matrix.M[1,2] = 0.0f;
		matrix.M[2,2] = scale;
		matrix.M[3,2] = 0.0f;
				
		matrix.M[0,3] = 0.0f;
		matrix.M[1,3] = 0.0f;
		matrix.M[2,3] = 0.0f;
		matrix.M[3,3] = 1.0f;
				
		return matrix;
	}
	
	public static Mat4x4 CreateTranslationMatrix(Vector4 pos)
	{
		Mat4x4 matrix = new Mat4x4();

		matrix.M[0,0] = 1.0f;
		matrix.M[1,0] = 0.0f;
		matrix.M[2,0] = 0.0f;
		matrix.M[3,0] = pos.X;
				
		matrix.M[0,1] = 0.0f;
		matrix.M[1,1] = 1.0f;
		matrix.M[2,1] = 0.0f;
		matrix.M[3,1] = pos.Y;
				
		matrix.M[0,2] = 0.0f;
		matrix.M[1,2] = 0.0f;
		matrix.M[2,2] = 1.0f;
		matrix.M[3,2] = pos.Z;
				
		matrix.M[0,3] = 0.0f;
		matrix.M[1,3] = 0.0f;
		matrix.M[2,3] = 0.0f;
		matrix.M[3,3] = 1.0f;
				
		return matrix;
	}
	
	public static Mat4x4 CreatePerspectiveMatrix()
	{
		Mat4x4 matrix = new Mat4x4();

		var fov_half = WINDOW_FOV / 2;
		var fovHalfTan = 1.0f / (float)Math.Tan(Graphics.AngleToRad(fov_half));

		matrix.M[0,0] = WINDOW_ASPECT * fovHalfTan;
		matrix.M[1,0] = 0.0f;
		matrix.M[2,0] = 0.0f;
		matrix.M[3,0] = 0.0f;
				
		matrix.M[0,1] = 0.0f;
		matrix.M[1,1] = fovHalfTan;
		matrix.M[2,1] = 0.0f;
		matrix.M[3,1] = 0.0f;
				
		matrix.M[0,2] = 0.0f;
		matrix.M[1,2] = 0.0f;
		matrix.M[2,2] = 1.0f;
		// matrix.M[3,2] = 0.0f;
		matrix.M[2,2] = Z_FAR / (Z_FAR - Z_NEAR);
		matrix.M[3,2] = (Z_FAR * Z_NEAR / (Z_FAR - Z_NEAR));
				
		matrix.M[0,3] = 0.0f;
		matrix.M[1,3] = 0.0f;
		matrix.M[2,3] = 1.0f;
		matrix.M[3,3] = 0.0f;
				
		return matrix;
	}

	public static Vector3 VecAdd(Vector3 vec1, Vector3 vec2) =>
		new Vector3(vec1.X + vec2.X, vec1.Y + vec2.Y, vec1.Z + vec2.Z);

	public static Vector4 VecSub(Vector4 vec1, Vector4 vec2) =>
		new Vector4(vec1.X - vec2.X, vec1.Y - vec2.Y, vec1.Z - vec2.Z, 1.0f);

	public static Vector3 VecMul(Vector3 vec1, float val) =>
		new Vector3(vec1.X * val, vec1.Y * val, vec1.Z * val);

	public static Vector4 VecDiv(Vector4 vec1, float val) =>
		new Vector4(vec1.X / val, vec1.Y / val, vec1.Z / val, 1.0f);

	public static float VecDot(Vector4 vec1, Vector4 vec2) =>
		(vec1.X * vec2.X) + (vec1.Y * vec2.Y) + (vec1.Z * vec2.Z);


	public static float VecLen(Vector4 vec) =>
		(float)Math.Sqrt(VecDot(vec, vec));

	public static Vector4 VecNorm(Vector4 vec)
	{
		float length = VecLen(vec);
		return new Vector4(vec.X / length, vec.Y / length, vec.Z / length, 1.0f);
	}

	public static Vector4 VecCrossProd(Vector4 vec1, Vector4 vec2)
	{
		Vector4 retVec = new Vector4();
		retVec.X = (vec1.Y * vec2.Z) - (vec1.Z * vec2.Y);
		retVec.Y = (vec1.Z * vec2.X) - (vec1.X * vec2.Z);
		retVec.Z = (vec1.X * vec2.Y) - (vec1.Y * vec2.X);
		return retVec;
	}
}
