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


        int location_temp_x = 0;
        int location_temp_y = 0;






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


            // ładowanie spriteów
            spriteRenderer = new SpriteRenderer(Engine);
            test = Sprite.FromFile("Assets/mood-kid.png");

            modelRenderer.UpdateCameraRotation(0.0f);

        }

        // co każdą klatkę - tutaj obliczenia
        public override void Update()
        {
            models[0].RotationY += 0.05f;
            //models[1].RotationY += 0.1f;
            //models[2].RotationY += 0.1f;

            if (Engine.GetKeyDown(ConsoleKey.LeftArrow) || Engine.GetKeyDown(ConsoleKey.A))
            {
                location_temp_x--;
                modelRenderer.UpdateCameraRotation(-0.05f);
            }

            if (Engine.GetKeyDown(ConsoleKey.RightArrow) || Engine.GetKeyDown(ConsoleKey.D))
            {
                location_temp_x++;
                modelRenderer.UpdateCameraRotation(0.05f);
            }

            if (Engine.GetKeyDown(ConsoleKey.UpArrow) || Engine.GetKeyDown(ConsoleKey.W))
            {
                location_temp_y--;
                modelRenderer.UpdateCameraMovement(0.1f,0.0f);
            }

            if (Engine.GetKeyDown(ConsoleKey.DownArrow) || Engine.GetKeyDown(ConsoleKey.S))
            {
                location_temp_y++;
                modelRenderer.UpdateCameraMovement(-0.1f, 0.0f);
            }

            if (Engine.GetKeyDown(ConsoleKey.Q))
            {
                location_temp_y--;
                modelRenderer.UpdateCameraMovement(0.0f,-0.1f);
            }

            if (Engine.GetKeyDown(ConsoleKey.E))
            {
                location_temp_y++;
                modelRenderer.UpdateCameraMovement(0.0f, 0.1f);
            }
                modelRenderer.UpdateViewMatrix();
            modelRenderer.UpdateVisibleFaces(models);
        }

        public void DrawCompassBar()
        {

            var angle2 = CustomMath.ConvertRadiansToDegrees(CustomMath.SimpleAngleBetweenTwoVectors(modelRenderer.CameraForward, models[0].Position - modelRenderer.CameraPosition));

            //angle2 = angle2 * (-1) ;

            //angle2 = (angle2 > 180)? angle2 - 360: angle2;

            Engine.WriteText(new Point(2, 4), "" + angle2, 14);

            var compass_position_test = (angle2 / 180 * 64 ) + 64;

            Engine.WriteText(new Point((int)compass_position_test, 2), "X" , 14);


        }

        public void DrawDebug()
        {
            Engine.WriteText(new Point(0, 0), "" + modelRenderer.CameraRotation, 14);
            Engine.WriteText(new Point(0, 1), "" + modelRenderer.CameraForward.X, 14);
            Engine.WriteText(new Point(0, 2), "" + modelRenderer.CameraForward.Z, 14);
            Engine.WriteText(new Point(0, 3), "" + Vector3.Distance(modelRenderer.CameraPosition, models[0].Position), 14);

            Engine.WriteText(new Point(0, 5), "" + modelRenderer.CameraPosition.X +" "+ modelRenderer.CameraPosition.Y +" "+ modelRenderer.CameraPosition.Z, 14);
            Engine.WriteText(new Point(0, 6), "" + CustomMath.SimpleAngleBetweenTwoVectors(modelRenderer.CameraForward, new Vector3(1,0,0)), 14);

            Engine.WriteText(new Point(0, 8), "" + modelRenderer.CameraForward.X + " " + modelRenderer.CameraForward.Y + " " + modelRenderer.CameraForward.Z, 14);
            Engine.WriteText(new Point(0, 9), "" + modelRenderer.CameraLeft.X + " " + modelRenderer.CameraLeft.Y + " " + modelRenderer.CameraLeft.Z, 14);
            Engine.WriteText(new Point(0, 10), "" + modelRenderer.CameraRight.X + " " + modelRenderer.CameraRight.Y + " " + modelRenderer.CameraRight.Z, 14);
        }


        // co każdą klatkę - tutaj rendering
        public override void Render()
        {
            Engine.ClearBuffer();

            // SCENE RENDER
            modelRenderer.Render();



            // HUD RENDER

            DrawCompassBar();

            // renderowanie pojedynczego sprite
            //spriteRenderer.RenderSingle(new Point(0, 0), test);

            // jakieś testy
            //Engine.Line(new Point(0, 0), new Point(50, 50), 4, ConsoleCharacter.BoxDrawingL_DR);
            //Engine.Arc(new Point(70, 60), 5, 2, arc: 360, ConsoleCharacter.Medium);

            // DEBUG DRAW

            DrawDebug();


 

            Engine.DisplayBuffer();
        }
    }
}