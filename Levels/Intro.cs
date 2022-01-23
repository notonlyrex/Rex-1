﻿using ConsoleGameEngine;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RexMinus1.Levels
{
    internal class Intro : Level
    {
        private Timer timer;
        private List<(Animation, string)> anims = new List<(Animation, string)>();
        private int currentAnim = -1;

        public override void Start()
        {
            UseStartingAnimation = false;
            timer.Reset();
            base.Start();
        }

        public override void Create()
        {
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

            models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/obj_mine.obj"), Position = new Vector3(0, 0, 3), RotationX = 1.241f });

            base.Create();
        }

        public override void Update()
        {
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

            if (Engine.GetKeyDown(Raylib_cs.KeyboardKey.KEY_SPACE))
            {
                LevelManager.GoToNext();
            }

            models[0].RotationY += 0.05f;

            base.Update();
        }

        public override void Render()
        {
            base.Render();
        }
    }
}