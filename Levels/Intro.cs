using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1.Levels
{
    internal class Intro : Level
    {
        private Timer timer;
        private Animation anim;

        public override void Start()
        {
            timer.Reset();

            PlayerManager.Instance.Shield = 1;
            PlayerManager.Instance.Energy = 1;
            PlayerManager.Instance.Heat = 0;

            base.Start();

            anim.Reset();
            PlayAnimation(anim);
        }

        public override void Create()
        {
            models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/obj_mine.obj"), Position = new Vector3(0, 0, 3), RotationX = 1.241f });

            timer = new Timer { Span = TimeSpan.FromSeconds(1) };

            //anim = new HorizontalTextAnimation() { Text = "Hello, world!", Origin = new Point(10, 20), Color = 7, Speed = 1 };
            //anim = new BorderedTextAnimation() { Color = 4, BackgroundColor = 0, Speed = 5, Text = "PROXIMITY ALERT" };
            //anim = new NoiseAnimation() { Intensity = 200 };
            anim = new ShakeAnimation(10, 10, 120);
            anim.IsPaused = false;

            base.Create();
        }

        public override void Update()
        {
            models[0].RotationY += 0.05f;

            if (timer.Elapsed)
            {
                LevelManager.GoTo(1);
            }

            base.Update();
        }

        public override void Render()
        {
            Engine.WriteText(new Point(50, 50), "Hello, world.", 15);
            base.Render();
        }
    }
}