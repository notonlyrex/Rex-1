using ConsoleGameEngine;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RexMinus1
{
    internal class RexMinus1 : ConsoleGame
    {
        private ModelRenderer modelRenderer;
        private SpriteRenderer spriteRenderer;

        private Sprite test;
        private List<Model> models = new List<Model>();

        private static void Main(string[] args)
        {
            // parametry wyświetlania
            new RexMinus1().Construct(128, 64, 8, 8, FramerateMode.MaxFps);
        }

        // na początku działania gry
        public override void Create()
        {
            // paleta domyślna
            Engine.SetPalette(Palettes.Default);

            // docelowy framerate
            TargetFramerate = 30;

            // ładowanie modeli
            modelRenderer = new ModelRenderer(Engine);
            models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(0, 0, 2), RotationX = 1.241f });
            models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(4, 0, 2), RotationX = 2.151f });
            models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(-4, 0, 2), RotationX = 0.21f }); ;

            // ładowanie spriteów
            spriteRenderer = new SpriteRenderer(Engine);
            test = Sprite.FromFile("Assets/mood-kid.png");
        }

        // co każdą klatkę - tutaj obliczenia
        public override void Update()
        {
            models[0].RotationY += 0.1f;
            models[1].RotationY += 0.1f;
            models[2].RotationY += 0.1f;

            if (Engine.GetKeyDown(ConsoleKey.LeftArrow))
            {
                modelRenderer.CameraRotation += 0.05f;
            }

            if (Engine.GetKeyDown(ConsoleKey.RightArrow))
            {
                modelRenderer.CameraRotation -= 0.05f;
            }

            if (Engine.GetKeyDown(ConsoleKey.UpArrow))
            {
                float move = 0.1f;
                modelRenderer.CameraPosition = new Vector3(modelRenderer.CameraPosition.X, modelRenderer.CameraPosition.Y, modelRenderer.CameraPosition.Z + move);
            }

            if (Engine.GetKeyDown(ConsoleKey.DownArrow))
            {
                float move = -0.1f;
                modelRenderer.CameraPosition = new Vector3(modelRenderer.CameraPosition.X, modelRenderer.CameraPosition.Y, modelRenderer.CameraPosition.Z + move);
            }

            modelRenderer.UpdateViewMatrix();
            modelRenderer.UpdateVisibleFaces(models);
        }

        // co każdą klatkę - tutaj rendering
        public override void Render()
        {
            Engine.ClearBuffer();

            modelRenderer.Render();
            spriteRenderer.RenderSingle(new Point(0, 0), test);

            Engine.DisplayBuffer();
        }
    }
}