using SFML.Graphics;
using SFML.System;

namespace SoftRenderer;

public class WindowText
{
    public Text Text { get; set; }
    public string DisplayedString { get { return Text.DisplayedString; } set { Text.DisplayedString = value; } }

    public WindowText(Font font, Vector2f pos)
    {
        Text = new Text()
        {
            Font = font,
            CharacterSize = 18,
            Position = new Vector2f(pos.X, pos.Y)
        };
    }
}