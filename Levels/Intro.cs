using ConsoleGameEngine;
using System;
using System.Collections.Generic;

namespace RexMinus1.Levels
{
    internal class Intro : Level
    {
        private Timer timer;
        private List<(Animation, string)> anims = new List<(Animation, string)>();
        private int currentAnim = -1;

        public override void Start()
        {
            timer.Reset();

            //PlayerManager.Instance.Shield = 1;
            //PlayerManager.Instance.Energy = 1;
            //PlayerManager.Instance.Heat = 0;
            //PlayerManager.Instance.HeatToAccelerate = 0.1f;
            //PlayerManager.Instance.HeatToShoot = 0.1f;
            //PlayerManager.Instance.Acceleration = 0.02f;
            //PlayerManager.Instance.Deceleration = 0.01f;
            //PlayerManager.Instance.EnergyToAccelerate = 0.15f;
            //PlayerManager.Instance.EnergyToDecelerate = 0.15f;
            //PlayerManager.Instance.EnergyToShoot = 0.2f;

            base.Start();
        }

        public override void Create()
        {
            //models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/obj_mine.obj"), Position = new Vector3(0, 0, 3), RotationX = 1.241f });

            timer = new Timer { Span = TimeSpan.FromSeconds(2) };

            anims.Add((new HorizontalTextAnimation() { Text = "Warning!", Origin = new Point(10, 20), Color = 7, Speed = 1, IsPaused = true }, "beep_danger"));
            anims.Add((new BorderedTextAnimation() { Text = "PROXIMITY ALERT", Color = 0, IsPaused = true, BackgroundColor = 12, IsCentered = true, Speed = 1 }, "radar_close"));
            anims.Add((new HorizontalTextAnimation() { Text = "The ship is under attack!", Origin = new Point(10, 22), Color = 7, Speed = 1, IsPaused = true }, "radar_close"));
            anims.Add((new HorizontalTextAnimation() { Text = "Grabons are here! It's a trap!", Origin = new Point(10, 22), Color = 7, Speed = 1, IsPaused = true }, "beep_danger"));
            anims.Add((new HorizontalTextAnimation() { Text = "HULL INTEGRITY BREACH!", Origin = new Point(10, 25), Color = 12, Speed = 1, IsPaused = true }, "beep_danger"));
            anims.Add((new HorizontalTextAnimation() { Text = "OXYGEN LEAK!", Origin = new Point(10, 26), Color = 12, Speed = 1, IsPaused = true }, "beep_error_2"));
            anims.Add((new HorizontalTextAnimation() { Text = "Wake up, commander!", Origin = new Point(20, 30), Color = 7, Speed = 1, IsPaused = true }, "boom_2"));
            anims.Add((new HorizontalTextAnimation() { Text = "Wake up, Rex!", Origin = new Point(20, 31), Color = 7, Speed = 2, IsPaused = true }, "beep_1"));
            anims.Add((new HorizontalTextAnimation() { Text = "Wake up, Rex!", Origin = new Point(20, 32), Color = 7, Speed = 2, IsPaused = true }, "beep_1"));
            anims.Add((new HorizontalTextAnimation() { Text = "Wake up, Remiligius, for the god's sake!", Origin = new Point(20, 34), Color = 7, Speed = 2, IsPaused = true }, "boom"));
            anims.Add((new HorizontalTextAnimation() { Text = "The ship has been attacked! Our astronauts are floating in space!", Origin = new Point(20, 37), Color = 7, Speed = 2, IsPaused = true }, "beep_3"));
            anims.Add((new HorizontalTextAnimation() { Text = "Sensors not working!", Origin = new Point(20, 38), Color = 7, Speed = 2, IsPaused = true }, "beep_1"));
            anims.Add((new HorizontalTextAnimation() { Text = "Switching to the sensors package from 1981.", Origin = new Point(20, 40), Color = 15, Speed = 2, IsPaused = true }, null));
            anims.Add((new ScramblingAnimation() { Intensity = 50 }, "startgame"));
            anims.Add((null, null));

            base.Create();
        }

        public override void Update()
        {
            //models[0].RotationY += 0.05f;

            if (timer.Elapsed)
            {
                currentAnim++;
                if (currentAnim >= anims.Count)
                {
                    LevelManager.GoToNext();
                }
                else
                {
                    var (anim, sound) = anims[currentAnim];
                    if (sound != null)
                        AudioPlaybackEngine.Instance.PlayCachedSound(sound);

                    if (anim != null)
                        PlayAnimation(anim);

                    timer.Reset();
                }
            }

            if (Engine.GetKeyDown(ConsoleKey.Spacebar))
            {
                LevelManager.GoToNext();
            }

            base.Update();
        }

        public override void Render()
        {
            base.Render();
        }
    }
}