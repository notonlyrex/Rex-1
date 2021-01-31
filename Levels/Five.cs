using ConsoleGameEngine;
using RexMinus1.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RexMinus1.Levels
{
    internal class Five : Level
    {
        private Animations.Laser laser;
        private Timer timer;
        private List<(Animation, string)> anims = new List<(Animation, string)>();
        private int currentAnim = -1;

        public override void Start()
        {
            PlayerManager.Instance.HeatToAccelerate = 0.1f;
            PlayerManager.Instance.HeatToShoot = 0.1f;
            PlayerManager.Instance.Acceleration = 0.02f;
            PlayerManager.Instance.Deceleration = 0.01f;
            PlayerManager.Instance.EnergyToAccelerate = 0.15f;
            PlayerManager.Instance.EnergyToDecelerate = 0.15f;
            PlayerManager.Instance.EnergyToShoot = 0.2f;

            models.Clear();

            models.Add(new MovingMine
            {
                Position = new Vector3(120, 0, 50),
                RotationX = 1.241f,
                DetectionRange = 500
            });

            models.Add(new MovingMine
            {
                Position = new Vector3(50, 0, 100),
                RotationX = 2.11f,
                DetectionRange = 500
            });

            models.Add(new MovingMine
            {
                Position = new Vector3(20, 0, 300),
                RotationX = 3.41f,
                DetectionRange = 500
            });

            models.Add(new Astronaut
            {
                Position = new Vector3(100, 0, 180),
                DetectionRange = 500
            });

            models.Add(new Astronaut
            {
                Position = new Vector3(50, 0, 0),
                DetectionRange = 500
            });

            //AudioPlaybackEngine.Instance.PlayCachedSound("startgame");

            speed = 0;

            base.Start();
        }

        public override void Create()
        {
            laser = new Animations.Laser();
            timer = new Timer() { Span = TimeSpan.FromSeconds(5) };

            //anims.Add((new HorizontalTextAnimation() { Origin = new Point(9, 54), Color = 4, Text = "WARNUNG: COMPUTER SYSTEM COMPROMISED." }, null));
            //anims.Add((new ScramblingAnimation() { Intensity = 100 }, null));

            base.Create();
        }

        public override void Update()
        {
            // animacje wyjaśniające
            if (timer.Elapsed)
            {
                currentAnim++;
                if (currentAnim < anims.Count)
                {
                    var (anim, sound) = anims[currentAnim];
                    if (sound != null)
                        AudioPlaybackEngine.Instance.PlayCachedSound(sound);

                    if (anim != null)
                        PlayAnimation(anim);

                    timer.Reset();
                }
            }

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
                if (PlayerManager.Instance.Energy > PlayerManager.Instance.EnergyToAccelerate)
                {
                    speed += PlayerManager.Instance.Acceleration;
                    PlayerManager.Instance.Energy -= PlayerManager.Instance.EnergyToAccelerate;
                    PlayerManager.Instance.Heat += PlayerManager.Instance.HeatToAccelerate;
                }
            }

            if (Engine.GetKeyDown(ConsoleKey.DownArrow) || Engine.GetKeyDown(ConsoleKey.S))
            {
                if (PlayerManager.Instance.Energy > PlayerManager.Instance.EnergyToAccelerate)
                {
                    speed -= PlayerManager.Instance.Deceleration;
                    PlayerManager.Instance.Energy -= PlayerManager.Instance.EnergyToDecelerate;
                    PlayerManager.Instance.Heat += PlayerManager.Instance.HeatToAccelerate;
                }
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

            // aktualizacja pozycji
            ModelRenderer.UpdateCameraMovement(speed, 0.0f);

            // strzał
            if (Engine.GetKeyDown(ConsoleKey.Spacebar) && PlayerManager.Instance.Energy > PlayerManager.Instance.EnergyToShoot)
            {
                // animacja lasera
                laser.Reset();
                laser.IsPaused = false;
                PlayAnimation(laser);
                AudioPlaybackEngine.Instance.PlayCachedSound("shoot_laser");

                // aktualizacja energii
                PlayerManager.Instance.Energy -= PlayerManager.Instance.EnergyToShoot;
                PlayerManager.Instance.Heat += PlayerManager.Instance.HeatToShoot;

                // sprawdzenie warunków trafienia
                foreach (var enemy in models.OfType<Enemy>())
                {
                    enemy.Hit(ModelRenderer.CameraPosition, ModelRenderer.CameraForward);
                }
            }

            // sprawdzenie warunków zwycięstwa i przegranej
            if (base.CheckWin())
            {
                LevelManager.GoToNext();
            }

            if (base.CheckLose())
                LevelManager.GoTo(LevelManager.GameOverLose);

            // kolizje, poruszanie obiektami
            base.Update();
        }

        public override void Render()
        {
            //base.Render();
            ModelRenderer.Render();

            DrawCompassBar();
            DrawHud();
            AnimationRenderer.Render();
        }
    }
}