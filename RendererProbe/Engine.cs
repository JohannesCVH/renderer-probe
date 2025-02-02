using System.Numerics;
using static RendererProbe.Globals;

namespace RendererProbe;

internal static class Engine
{
    //public const int WINDOW_WIDTH = 1280;
    //public const int WINDOW_HEIGHT = 480;
    //public const int WINDOW_WIDTH_HALF = WINDOW_WIDTH / 2;
    //public const int WINDOW_HEIGHT_HALF = WINDOW_HEIGHT / 2;
    //public static int WINDOW_FOV = 90;
    //public const float WINDOW_ASPECT = (float)WINDOW_WIDTH / WINDOW_HEIGHT;
    //public const float Z_NEAR = 1.0f;
    //public const float Z_FAR = 10.0f;
    //public static float FOV_RAD = 1.0f / (float)Math.Tan(WINDOW_FOV * 0.5f / 180.0f * 3.14159f);

    public static int NormalizeCoordX(float coord)
    {
        return (int)Math.Floor(WINDOW_WIDTH_HALF + (coord * WINDOW_WIDTH_HALF));
    }

    public static int NormalizeCoordY(float coord)
    {
        return (int)Math.Floor(WINDOW_HEIGHT_HALF - (coord * WINDOW_HEIGHT_HALF));
    }

    public static Vector2 NormalizeVector2(Vector2 pos)
    {
        return new Vector2()
        {
            X = WINDOW_WIDTH_HALF + (pos.X * WINDOW_WIDTH_HALF),
            Y = WINDOW_HEIGHT_HALF - (pos.Y * WINDOW_HEIGHT_HALF)
        };
    }

    //public static Vector3 NormalizeVector3(Vector3 pos)
    //{
    //    return new Vector3()
    //    {
    //        X = WINDOW_WIDTH_HALF + ((pos.X / 100) * WINDOW_WIDTH_HALF),
    //        Y = WINDOW_HEIGHT_HALF - ((pos.Y / 100) * WINDOW_HEIGHT_HALF)
    //    };
    //}

    public static Vector3 Vector3ToScreenSpace(Vector3 pos)
    {
        return new Vector3()
        {
            X = WINDOW_WIDTH_HALF + ((pos.X / 100) * WINDOW_WIDTH_HALF),
            Y = WINDOW_HEIGHT_HALF - ((pos.Y / 100) * WINDOW_HEIGHT_HALF)
        };
    }
}