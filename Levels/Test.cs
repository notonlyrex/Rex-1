using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1.Levels
{
    internal class Test : Level
    {
        private void DrawCompassBar()
        {
            var angle2 = CustomMath.ConvertRadiansToDegrees(CustomMath.SimpleAngleBetweenTwoVectors(ModelRenderer.CameraForward, models[0].Position - ModelRenderer.CameraPosition));

            //angle2 = angle2 * (-1) ;
            //angle2 = (angle2 > 180)? angle2 - 360: angle2;

            Engine.WriteText(new Point(2, 4), "" + angle2, 14);

            var compass_position_test = (angle2 / 180 * 64) + 64;

            Engine.WriteText(new Point((int)compass_position_test, 2), "X", 14);
        }

        public override void Start()
        {
            if (PlayerManager.Instance.IsMusicEnabled)
                MusicPlaybackEngine.Instance.PlayMusic("Assets/song_main.ogg");

            AudioPlaybackEngine.Instance.PlayCachedSound("startgame");

            base.Start();
        }

        public override void Create()
        {
            models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/test5.obj"), Position = new Vector3(0, 0, 3), RotationX = 1.241f });

            base.Create();
        }

        public override void Update()
        {
            //models[0].RotationY += 0.05f;
            //models[1].RotationY += 0.1f;
            //models[2].RotationY += 0.1f;

            if (Engine.GetKeyDown(ConsoleKey.LeftArrow) || Engine.GetKeyDown(ConsoleKey.A))
            {
                ModelRenderer.UpdateCameraRotation(-0.05f);
            }

            if (Engine.GetKeyDown(ConsoleKey.RightArrow) || Engine.GetKeyDown(ConsoleKey.D))
            {
                ModelRenderer.UpdateCameraRotation(0.05f);
            }

            if (Engine.GetKeyDown(ConsoleKey.UpArrow) || Engine.GetKeyDown(ConsoleKey.W))
            {
                ModelRenderer.UpdateCameraMovement(0.1f, 0.0f);
            }

            if (Engine.GetKeyDown(ConsoleKey.DownArrow) || Engine.GetKeyDown(ConsoleKey.S))
            {
                ModelRenderer.UpdateCameraMovement(-0.1f, 0.0f);
            }

            if (Engine.GetKeyDown(ConsoleKey.Q))
            {
                ModelRenderer.UpdateCameraMovement(0.0f, -0.1f);
            }

            if (Engine.GetKeyDown(ConsoleKey.E))
            {
                ModelRenderer.UpdateCameraMovement(0.0f, 0.1f);
            }

            if (Engine.GetKeyDown(ConsoleKey.R))
            {
                ModelRenderer.UpdateFOV(0.1f);
            }

            if (Engine.GetKeyDown(ConsoleKey.F))
            {
                ModelRenderer.UpdateFOV(-0.1f);
            }

            if (Engine.GetKeyDown(ConsoleKey.U))
                AudioPlaybackEngine.Instance.PlayCachedSound("beep1");

            if (Engine.GetKeyDown(ConsoleKey.I))
                AudioPlaybackEngine.Instance.PlayCachedSound("boom");

            base.Update();
        }

        public override void Render()
        {
            base.Render();

            DrawCompassBar();
            DrawHud();
        }
    }
}