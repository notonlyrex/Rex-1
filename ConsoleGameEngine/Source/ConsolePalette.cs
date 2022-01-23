using System.Collections.Generic;

namespace ConsoleGameEngine
{
    internal class ConsolePalette
    {
        public static Dictionary<int, SdlSharp.Graphics.Color> Palette = new Dictionary<int, SdlSharp.Graphics.Color>();

        public static int SetColor(int consoleColor, Color targetColor)
        {
            return SetColor(consoleColor, targetColor.R, targetColor.G, targetColor.B);
        }

        private static int SetColor(int color, uint r, uint g, uint b)
        {
            Palette[color] = new SdlSharp.Graphics.Color((byte)r, (byte)g, (byte)b);

            return 0;
        }
    }
}