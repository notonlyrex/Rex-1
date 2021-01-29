using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace ConsoleGameEngine
{
    public class Sprite
    {
        public Glyph[,] Glyphs { get; set; }

        public static Sprite FromFile(string fileName)
        {
            using (var img = Image.Load<Rgba32>(fileName))
            {
                var result = new Sprite();
                result.Glyphs = new Glyph[img.Width, img.Height];

                for (int y = 0; y < img.Height; y++)
                {
                    Span<Rgba32> pixelRowSpan = img.GetPixelRowSpan(y);
                    for (int x = 0; x < img.Width; x++)
                    {
                        if (pixelRowSpan[x].A == 255)
                            result.Glyphs[x, y] = new Glyph() { c = 'x', bg = 0, fg = 1 };
                    }
                }

                return result;
            }
        }
    }
}