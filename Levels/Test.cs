using ConsoleGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RexMinus1.Levels
{
    internal class Test : Level
    {
        private Laser laser;

        private void DrawCompassBar()
        {
            if (models.Count == 0)
                return;

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
            models.Add(new Enemy { Mesh = Mesh.LoadFromObj("Assets/obj_astro.obj"), Position = new Vector3(0, 0, 3), RotationX = 1.241f, Shield = 1f });

            laser = new Laser();

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
                speed += 0.05f;
                PlayerManager.Instance.Energy -= 0.01f;
                PlayerManager.Instance.Heat += 0.1f;
            }

            if (Engine.GetKeyDown(ConsoleKey.DownArrow) || Engine.GetKeyDown(ConsoleKey.S))
            {
                speed -= 0.04f;
                PlayerManager.Instance.Energy -= 0.01f;
                PlayerManager.Instance.Heat += 0.1f;
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

            if (Engine.GetKeyDown(ConsoleKey.F2))
            {
                LevelManager.GoTo(0);
            }

            if (PlayerManager.Instance.Heat > 0)
                PlayerManager.Instance.Heat -= 0.01f;

            if (PlayerManager.Instance.Energy < 1)
                PlayerManager.Instance.Energy += 0.01f;

            if (PlayerManager.Instance.Energy <= 0.05)
                LevelManager.GoTo(0); // gameover

            if (PlayerManager.Instance.Heat >= 0.95)
                LevelManager.GoTo(0); // gameover

            ModelRenderer.UpdateCameraMovement(speed, 0.0f);

            if (Engine.GetKeyDown(ConsoleKey.U))
            {
                laser.Reset();
                laser.IsPaused = false;
                PlayAnimation(laser);
                AudioPlaybackEngine.Instance.PlayCachedSound("shoot_laser");

                PlayerManager.Instance.Energy -= 0.1f;
                PlayerManager.Instance.Heat += 0.1f;

                var killed = new List<Model>();

                foreach (var enemy in models.OfType<Enemy>())
                {
                    enemy.Hit(ModelRenderer.CameraPosition);
                }

                models.RemoveAll(x => x.GetType() == typeof(Enemy) && (x as Enemy).Shield <= 0);
            }

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