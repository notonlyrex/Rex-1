using ConsoleGameEngine;
using System;

namespace RexMinus1.Levels
{
    internal class MainScreen : Level
    {
        private Timer timer;
        private Animation anim;
        private FigletFont font;

        public override void Start()
        {
            UseStartingAnimation = false;

            timer.Reset();

            PlayerManager.Instance.Shield = 1;
            PlayerManager.Instance.Energy = 1;
            PlayerManager.Instance.Heat = 0;
            PlayerManager.Instance.HeatToAccelerate = 0.1f;
            PlayerManager.Instance.HeatToShoot = 0.1f;
            PlayerManager.Instance.Acceleration = 0.02f;
            PlayerManager.Instance.Deceleration = 0.01f;
            PlayerManager.Instance.EnergyToAccelerate = 0.15f;
            PlayerManager.Instance.EnergyToDecelerate = 0.15f;
            PlayerManager.Instance.EnergyToShoot = 0.2f;

            if (PlayerManager.Instance.IsMusicEnabled)
                MusicPlaybackEngine.Instance.PlayMusic("Assets/song_main.ogg");

            base.Start();

            anim.Reset();
            PlayAnimation(anim);
        }

        public override void Create()
        {
            timer = new Timer { Span = TimeSpan.FromSeconds(1) };

            anim = new ScramblingAnimation { Intensity = 200, IsPaused = false };

            font = FigletFont.Load("Assets/caligraphy.flf");

            base.Create();
        }

        public override void Update()
        {
            if (Engine.GetKeyDown(ConsoleKey.Spacebar))
            {
                LevelManager.GoTo(LevelManager.Intro);
            }

            base.Update();
        }

        public override void Render()
        {
            base.Render();
            SpriteRenderer.RenderSingle(new Point(0, 0), Sprite.FromFile("Assets/sprite_logo.png"));

            Engine.WriteTextCenteredHorizontally("Press SPACE to continue", 63, 15, 8);

            //Engine.WriteFiglet(new Point(40, 20), "Rex -1", font, 15);
        }
    }
}