using ConsoleGameEngine;
using RexMinus1.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RexMinus1.Levels
{
    internal class One : Level
    {
        private Animations.Laser laser;
        private Timer timer;
        private List<(Animation, string)> anims = new List<(Animation, string)>();
        private int currentAnim = -1;

        public override void Start()
        {
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

            models.Clear();

            models.Add(new Mine
            {
                Position = new Vector3(0, 0, 60),
                RotationX = 1.241f
            });

            models.Add(new Astronaut
            {
                Mesh = Mesh.LoadFromObj("Assets/obj_astro.obj"),
                Position = new Vector3(0, 0, 30),
            });

            AudioPlaybackEngine.Instance.PlayCachedSound("startgame");

            speed = 0;

            base.Start();
        }

        public override void Create()
        {
            laser = new Animations.Laser();
            timer = new Timer() { Span = TimeSpan.FromSeconds(2) };

            anims.Add((new HorizontalTextAnimation() { Origin = new Point(9, 48), Color = 7, Text = "Use arrows or WSAD to control the ship." }, "beep_3"));
            anims.Add((new HorizontalTextAnimation() { Origin = new Point(9, 49), Color = 7, Text = "When closing to the object it will be identified." }, "beep_3"));
            anims.Add((new HorizontalTextAnimation() { Origin = new Point(9, 50), Color = 7, Text = "Check out your radar on the top." }, "beep_3"));
            anims.Add((new HorizontalTextAnimation() { Origin = new Point(9, 51), Color = 7, Text = "Go near the green objects to take astronauts." }, "beep_3"));
            anims.Add((new HorizontalTextAnimation() { Origin = new Point(9, 52), Color = 7, Text = "Avoid enemies, marked red." }, "beep_3"));
            anims.Add((new HorizontalTextAnimation() { Origin = new Point(9, 53), Color = 7, Text = "Press SPACE to shoot laser beams." }, "beep_3"));
            anims.Add((new HorizontalTextAnimation() { Origin = new Point(9, 54), Color = 6, Text = "Rescue the astronaut and destroy the space mine." }, null));

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

            if (Engine.GetKeyDown(ConsoleKey.F2))
            {
                LevelManager.GoTo(0);
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
            if (models.OfType<Astronaut>().Count() == 0 && models.OfType<Mine>().Count() == 0)
                LevelManager.GoToNext();

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