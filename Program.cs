using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1
{
    internal class HelloWorld : ConsoleGame
    {
        private ModelRenderer modelRenderer;

        private static void Main(string[] args)
        {
            new HelloWorld().Construct(128, 64, 8, 8, FramerateMode.MaxFps);
        }

        public override void Create()
        {
            Engine.SetPalette(Palettes.Default);
            TargetFramerate = 30;

            modelRenderer = new ModelRenderer(Engine);
            modelRenderer.Models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(0, 0, 2), RotationX = 1.241f });
            modelRenderer.Models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(4, 0, 2), RotationX = 2.151f });
            modelRenderer.Models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(-4, 0, 2), RotationX = 0.21f }); ;
        }

        public override void Update()
        {
            modelRenderer.Models[0].RotationY += 0.1f;
            modelRenderer.Models[1].RotationY += 0.1f;
            modelRenderer.Models[2].RotationY += 0.1f;

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
            modelRenderer.UpdateVisibleFaces();
        }

        public override void Render()
        {
            Engine.ClearBuffer();

            modelRenderer.Render();

            Engine.DisplayBuffer();
        }
    }
}