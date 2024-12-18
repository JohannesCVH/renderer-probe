namespace RendererProbe;

internal static class Constants
{
    public const int WINDOW_WIDTH = 720;
    public const int WINDOW_HEIGHT = 720;
    public const int WINDOW_WIDTH_HALF = WINDOW_WIDTH / 2;
    public const int WINDOW_HEIGHT_HALF = WINDOW_HEIGHT / 2;
    public static int WINDOW_FOV = 60;
    public const float WINDOW_ASPECT = (float)WINDOW_HEIGHT / WINDOW_WIDTH;
    public static float Z_NEAR = 1.0f;
    public static float Z_FAR = 10.0f;
    public static bool PERSPECTIVE = false;
    public const float WORLD_SIZE = 100.0f;
}