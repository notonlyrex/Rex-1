using ConsoleGameEngine;
using System;

namespace RexMinus1
{
    internal class RexMinus1 : ConsoleGame
    {
        private ModelRenderer modelRenderer;
        private SpriteRenderer spriteRenderer;

        private LevelManager levelManager;
        private AnimationRenderer animationRenderer;

        private bool drawDebug = false;

        private static void Main(string[] args)
        {
            // cachowanie efektów dźwiękowych
            AudioPlaybackEngine.Instance.AddCachedSound("zap", "Assets/coin.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("boom", "Assets/collect.wav");
            AudioPlaybackEngine.Instance.Volume = 0.01f;

            // parametry wyświetlania
            new RexMinus1().Construct(128, 64, 8, 8, FramerateMode.MaxFps);

            // zwalnianie podsystemu audio
            AudioPlaybackEngine.Instance.Dispose();
        }

        // na początku działania gry
        public override void Create()
        {
            // paleta domyślna
            Engine.SetPalette(Palettes.Default);

            // docelowy framerate
            TargetFramerate = 30;

            // ładowanie rendererów
            modelRenderer = new ModelRenderer(Engine);
            spriteRenderer = new SpriteRenderer(Engine);
            levelManager = new LevelManager();
            animationRenderer = new AnimationRenderer(Engine);

            // ładowanie poziomów i ekranów
            levelManager.Add(new Levels.Intro());
            levelManager.Add(new Levels.Test());

            // inicjalizacja podstawowych rzeczy w poziomach
            // i ladowanie do pamięci wszystkiego
            levelManager.Initialize(Engine, modelRenderer, spriteRenderer, animationRenderer);
        }

        // co każdą klatkę - tutaj obliczenia
        public override void Update()
        {
            levelManager.CurrentLevel.Update();

            if (Engine.GetKeyDown(ConsoleKey.F1))
                drawDebug = !drawDebug;
        }

        // co każdą klatkę - tutaj rendering
        public override void Render()
        {
            Engine.ClearBuffer();

            // SCENE RENDER
            levelManager.CurrentLevel.Render();

            // DEBUG RENDER
            if (drawDebug)
                levelManager.CurrentLevel.DrawDebug();

            Engine.DisplayBuffer();
        }
    }
}