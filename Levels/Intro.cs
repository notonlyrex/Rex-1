using ConsoleGameEngine;
using System.Numerics;

namespace RexMinus1.Levels
{
    internal class Intro : Level
    {
        private int counter;

        public override void Create()
        {
            models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(0, 0, 3), RotationX = 1.241f });

            counter = 0;

            base.Create();
        }

        public override void Update()
        {
            counter++;
            if (counter > 60)
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