using ConsoleGameEngine;
using System.Collections.Generic;

namespace RexMinus1
{
    public class LevelManager
    {
        private List<Level> levels = new List<Level>();
        private int currentLevel = 0;

        public Level CurrentLevel => levels[currentLevel];

        public void Add(Level level)
        {
            levels.Add(level);
        }

        public void GoToNext()
        {
            currentLevel += 1;
        }

        public void Initialize(ConsoleEngine engine, ModelRenderer modelRenderer, SpriteRenderer spriteRenderer)
        {
            foreach (var item in levels)
            {
                item.Engine = engine;
                item.ModelRenderer = modelRenderer;
                item.SpriteRenderer = spriteRenderer;
                item.LevelManager = this;

                item.Create();
            }
        }
    }
}