using System.Collections.Generic;

namespace ConsoleGameEngine
{
    internal class ConsolePalette
    {
        public static Dictionary<int, Raylib_cs.Color> Palette = new Dictionary<int, Raylib_cs.Color>();

        public static int SetColor(int consoleColor, Color targetColor)
        {
            return SetColor(consoleColor, targetColor.R, targetColor.G, targetColor.B);
        }

        private static int SetColor(int color, uint r, uint g, uint b)
        {
            Palette[color] = new Raylib_cs.Color((int)r, (int)g, (int)b, 0);

            return 0;
        }
    }
}