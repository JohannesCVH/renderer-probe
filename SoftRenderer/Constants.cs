namespace SoftRenderer;

public static class Constants
{
    public static string FONT_PATH = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Fonts/open-sans/OpenSans-Regular.ttf"
    );

    public const int UPDATE_INTERVAL = 10;
}