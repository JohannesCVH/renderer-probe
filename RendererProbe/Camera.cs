using System.Numerics;

namespace RendererProbe;

internal static class Camera
{
	public static float CAMERA_X = 0.0f;
	public static float CAMERA_Y = 0.0f;
	public static float CAMERA_Z = 0.0f;
	public static Vector3 CAMERA_DIRECTION = new Vector3(0.0f, 0.0f, 0.0f);
	public static float CAMERA_YAW = 0.0f;
	public static DateTime LAST_UPDATED = DateTime.Now;
	
	public static void HandleInput()
	{
		float movement = 4.0f;
		float rotationSpeed = 2.0f;
	}
}