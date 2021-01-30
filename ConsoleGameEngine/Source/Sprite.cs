using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace ConsoleGameEngine
{
    public class Sprite
    {
        public Glyph[,] Glyphs { get; set; }

        private static int ColorToBasicColor(Rgba32 color)
        {
            if (color.R == 20 && color.G == 28 && color.B == 25)
                return 0;

            if (color.R == 34 && color.G == 52 && color.B == 44)
                return 2; // 3

            if (color.R == 86 && color.G == 149 && color.B == 124)
                return 11; // 11

            if (color.R == 39 && color.G == 68 && color.B == 56)
                return 2;

            return 13;
        }

        private static char AlphaToCharacter(Rgba32 color)
        {
            ConsoleCharacter result = ConsoleCharacter.Null;

            if (color.A > 32)
                result = ConsoleCharacter.Light;

            if (color.A > 64)
                result = ConsoleCharacter.Medium;

            if (color.A > 128)
                result = ConsoleCharacter.Dark;

            if (color.A > 192)
                result = ConsoleCharacter.Full;

            return (char)result;
        }

        public static Sprite FromFile(string fileName)
        {
            using (var img = Image.Load<Rgba32>(fileName))
            {
                var result = new Sprite();
                result.Glyphs = new Glyph[img.Width, img.Height];

                for (int y = 0; y < img.Height; y++)
                {
                    // Full, Dark, Medium, Light

                    // 20,28,25 - 1
                    // 34,52,44 - 2/3
                    // 86.149.124 - 10/11
                    // 39,68,56 - 2/3

                    Span<Rgba32> pixelRowSpan = img.GetPixelRowSpan(y);
                    for (int x = 0; x < img.Width; x++)
                    {
                        if (pixelRowSpan[x].A > 0)
                            result.Glyphs[x, y] = new Glyph() { c = AlphaToCharacter(pixelRowSpan[x]), bg = 0, fg = ColorToBasicColor(pixelRowSpan[x]) };
                    }
                }

                return result;
            }
        }
    }
}