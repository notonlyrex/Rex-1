using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1.Levels
{
    internal class Intro : Level
    {
        private Timer timer;
        private HorizontalTextAnimation anim;

        public override void Start()
        {
            timer.Reset();

            PlayerManager.Instance.Shields = 1;
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

            anim = new HorizontalTextAnimation() { Text = "Hello, world!", Origin = new Point(10, 20), Color = 7, Speed = 1 };
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
            base.Render();
        }
    }
}