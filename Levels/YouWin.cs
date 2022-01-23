using ConsoleGameEngine;
using System;

namespace RexMinus1.Levels
{
    internal class YouWin : Level
    {
        private Timer timer;
        private Animation anim;
        private FigletFont font;

        public override void Start()
        {
            UseStartingAnimation = false;

            timer.Reset();

            base.Start();

            anim.Reset();
            PlayAnimation(anim);
        }

        public override void Create()
        {
            timer = new Timer { Span = TimeSpan.FromSeconds(1) };

            anim = new ScramblingAnimation { Intensity = 200, IsPaused = false };

            base.Create();
        }

        public override void Update()
        {
            if (Engine.GetKeyDown(SdlSharp.Input.Keycode.Space))
            {
                LevelManager.GoTo(LevelManager.Welcome);
            }

            base.Update();
        }

        public override void Render()
        {
            base.Render();
            SpriteRenderer.RenderSingle(new Point(0, 0), Sprite.FromFile("Assets/sprite_youwin.png"));

            Engine.WriteTextCenteredHorizontally("Press SPACE to continue", 62, 8, 0);
        }
    }
}