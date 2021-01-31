using ConsoleGameEngine;
using System.Collections.Generic;

namespace RexMinus1
{
    public class LevelManager
    {
        private List<Level> levels = new List<Level>();
        private int currentLevel = 0;

        public int GameOverWin { get; set; }
        public int GameOverLose { get; set; }
        public int Welcome { get; set; }
        public int Intro { get; set; }

        public Level CurrentLevel => levels[currentLevel];

        public void Add(Level level)
        {
            levels.Add(level);
        }

        public void GoTo(int level)
        {
            currentLevel = level;
            CurrentLevel.Start();
        }

        public void GoToNext()
        {
            if (levels.Count > currentLevel + 1)
            {
                GoTo(currentLevel + 1);
            }
            else
            {
                GoTo(GameOverWin);
            }
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