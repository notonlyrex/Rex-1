using System.Collections.Generic;

namespace ConsoleGameEngine
{
    public class SpriteRenderer
    {
        private readonly ConsoleEngine engine;

        public List<(Sprite, Point)> Batch;

        public SpriteRenderer(ConsoleEngine engine)
        {
            this.engine = engine;
            Batch = new List<(Sprite, Point)>();
        }

        public void RenderBatch()
        {
            foreach (var (sprite, origin) in Batch)
            {
                RenderSingle(origin, sprite);
            }

            Batch.Clear();
        }

        public void RenderSingle(Point origin, Sprite s)
        {
            for (int i = 0; i < s.Glyphs.GetLength(0); i++)
            {
                for (int j = 0; j < s.Glyphs.GetLength(1); j++)
                {
                    if (s.Glyphs[i, j] != null)
                        engine.SetPixel(new Point(origin.X + i, origin.Y + j), s.Glyphs[i, j].fg, s.Glyphs[i, j].c);
                }
            }
        }
    }
}