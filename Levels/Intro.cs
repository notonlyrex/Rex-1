using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1.Levels
{
    internal class Intro : Level
    {
        private Timer timer;

        public override void Create()
        {
            models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(0, 0, 3), RotationX = 1.241f });

            timer = new Timer { Span = TimeSpan.FromSeconds(1) };

            base.Create();
        }

        public override void Update()
        {
            if (timer.Elapsed)
                LevelManager.GoToNext();

            base.Update();
        }

        public override void Render()
        {
            Engine.WriteText(new Point(0, 0), "Hello, world!", 8);

            base.Render();
        }
    }
}