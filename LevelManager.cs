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

        public void GoTo(int level)
        {
            currentLevel += level;
            CurrentLevel.AnimationRenderer.Clear();
        }

        public void Initialize(ConsoleEngine engine, ModelRenderer modelRenderer, SpriteRenderer spriteRenderer, AnimationRenderer animationRenderer)
        {
            foreach (var item in levels)
            {
                item.Engine = engine;
                item.ModelRenderer = modelRenderer;
                item.SpriteRenderer = spriteRenderer;
                item.AnimationRenderer = animationRenderer;

                item.LevelManager = this;

                item.Create();
            }
        }
    }
}