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
            AudioPlaybackEngine.Instance.AddCachedSound("beep_1", "Assets/sound_beep_1.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("beep_2", "Assets/sound_beep_2.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("beep_3", "Assets/sound_beep_3.wav");

            AudioPlaybackEngine.Instance.AddCachedSound("beep_danger", "Assets/sound_beep_danger.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("beep_error", "Assets/sound_beep_error.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("beep_error_2", "Assets/sound_beep_error_2.wav");

            AudioPlaybackEngine.Instance.AddCachedSound("boom", "Assets/sound_boom.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("boom_2", "Assets/sound_boom_2.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("boom_3", "Assets/sound_boom_3.wav");

            AudioPlaybackEngine.Instance.AddCachedSound("electro_distortion", "Assets/sound_electro_distortion.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("gameover", "Assets/sound_gameover.wav");

            AudioPlaybackEngine.Instance.AddCachedSound("loading", "Assets/sound_loading.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("loading_2", "Assets/sound_loading_2.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("loading_3", "Assets/sound_loading_3.wav");

            AudioPlaybackEngine.Instance.AddCachedSound("missile_launch", "Assets/sound_missile_launch.wav");

            AudioPlaybackEngine.Instance.AddCachedSound("radar_close", "Assets/sound_radar_close.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("radar_detection", "Assets/sound_radar_detection.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("radar_detection_2", "Assets/sound_radar_detection_2.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("radar_far", "Assets/sound_radar_far.wav");

            AudioPlaybackEngine.Instance.AddCachedSound("shoot_laser", "Assets/sound_shoot_laser.wav");

            AudioPlaybackEngine.Instance.AddCachedSound("startgame", "Assets/sound_startgame.wav");
            AudioPlaybackEngine.Instance.AddCachedSound("startgame_2", "Assets/sound_startgame_2.wav");

            AudioPlaybackEngine.Instance.Volume = 1.0f;
            MusicPlaybackEngine.Instance.Volume = 1.0f;

            // parametry wyświetlania
            new RexMinus1().Construct(128, 64, 8, 8, FramerateMode.MaxFps);

            Exit();
        }

        private static void Exit()
        {
            // zwalnianie podsystemu audio
            AudioPlaybackEngine.Instance.Dispose();
            Environment.Exit(0);
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

            // inicjalizacja podstawowych elementow
#if DEBUG
            PlayerManager.Instance.IsMusicEnabled = false;
#else
            PlayerManager.Instance.IsMusicEnabled = true;
#endif

            // ładowanie poziomów i ekranów
            levelManager.Add(new Levels.MainScreen());
            levelManager.Add(new Levels.Intro());
            levelManager.Add(new Levels.Test());

            levelManager.GameOverLose = 0;
            levelManager.GameOverWin = 0;
            levelManager.Intro = 1;
            levelManager.Welcome = 0;

            // inicjalizacja podstawowych rzeczy w poziomach
            // i ladowanie do pamięci wszystkiego
            levelManager.Initialize(Engine, modelRenderer, spriteRenderer, animationRenderer);

            // przejście do pierwszego poziomu, ekranu powitalnego
            levelManager.GoTo(levelManager.Welcome);
        }

        // co każdą klatkę - tutaj obliczenia
        public override void Update()
        {
            levelManager.CurrentLevel.Update();

            if (Engine.GetKeyDown(ConsoleKey.F1))
                drawDebug = !drawDebug;

            if (Engine.GetKeyDown(ConsoleKey.Escape))
                Exit();
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