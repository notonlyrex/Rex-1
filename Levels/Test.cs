using ConsoleGameEngine;
using System;
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

            foreach (var item in models)
            {
                var angle2 = CustomMath.ConvertRadiansToDegrees(CustomMath.SimpleAngleBetweenTwoVectors(ModelRenderer.CameraForward, item.Position - ModelRenderer.CameraPosition));
                var compass_position = (angle2 / 180 * 64) + 64;

                var distance = Math.Abs(Vector3.Dot(ModelRenderer.CameraPosition, item.Position));
                if (distance > 999)
                    distance = 999;

                int color = 14;
                if (item.GetType() == typeof(Enemy) && (item as Enemy).IsIdentified)
                    color = 12;

                if (item.GetType() == typeof(Astronaut))
                    color = 10;

                Engine.WriteText(new Point((int)compass_position, 2), "X", color);
                Engine.WriteText(new Point((int)compass_position - 1, 3), distance.ToString("000"), color);
            }
        }

        public override void Start()
        {
            if (PlayerManager.Instance.IsMusicEnabled)
                MusicPlaybackEngine.Instance.PlayMusic("Assets/song_main.ogg");

            AudioPlaybackEngine.Instance.PlayCachedSound("startgame");

            speed = 0;

            base.Start();
        }

        public override void Create()
        {
            models.Add(new Enemy
            {
                Mesh = Mesh.LoadFromObj("Assets/obj_mine.obj"),
                Position = new Vector3(0, 0, 23),
                RotationX = 1.241f,
                Shield = 1f
            });

            models.Add(new Astronaut
            {
                Mesh = Mesh.LoadFromObj("Assets/obj_astro.obj"),
                Position = new Vector3(15, 0, 3),
            });

            laser = new Laser();

            base.Create();
        }

        public override void Update()
        {
            // aktualizacja pozycji obiektów
            //models[0].RotationY += 0.05f;
            //models[1].RotationY += 0.1f;
            //models[2].RotationY += 0.1f;

            // zmiana pozycji gracza
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

            // aktualizacja pozycji
            ModelRenderer.UpdateCameraMovement(speed, 0.0f);

            // strzał
            if (Engine.GetKeyDown(ConsoleKey.U))
            {
                // animacja lasera
                laser.Reset();
                laser.IsPaused = false;
                PlayAnimation(laser);
                AudioPlaybackEngine.Instance.PlayCachedSound("shoot_laser");

                PlayerManager.Instance.Energy -= 0.1f;
                PlayerManager.Instance.Heat += 0.1f;

                // sprawdzenie warunków trafienia
                foreach (var enemy in models.OfType<Enemy>())
                {
                    enemy.Hit(ModelRenderer.CameraPosition);
                }
            }

            // uruchamiania efektów kolizji gracza ze wszystkimi obiektami
            foreach (var item in models.OfType<ICollision>())
            {
                item.Collision(ModelRenderer.CameraPosition);
            }

            // usuwanie zniszczonych wrogów
            models.RemoveAll(x => x.GetType() == typeof(Enemy) && (x as Enemy).Shield <= 0);

            // usuwanie zebranych astronautów
            models.RemoveAll(x => x.GetType() == typeof(Astronaut) && (x as Astronaut).IsCollected);

            // sprawdzenie warunku zwycięstwa
            if (models.OfType<Astronaut>().Count() == 0)
                LevelManager.GoTo(LevelManager.GameOverWin);

            // aktualizacja ciepła i energii
            if (PlayerManager.Instance.Heat > 0)
                PlayerManager.Instance.Heat -= 0.01f;

            if (PlayerManager.Instance.Energy < 1)
                PlayerManager.Instance.Energy += 0.01f;

            // sprawdzenie warunków przegranej
            if (PlayerManager.Instance.Energy <= 0.05)
                LevelManager.GoTo(0);

            if (PlayerManager.Instance.Heat >= 0.95)
                LevelManager.GoTo(0);

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