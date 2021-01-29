using ConsoleGameEngine;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RexMinus1
{
    internal class Model
    {
        public Mesh Mesh { get; set; }
        public Vector3 Position { get; set; }

        public float RotationY { get; set; }
        public float RotationX { get; set; }
    }

    internal class MeshProjection
    {
        private readonly ConsoleEngine Engine;

        public List<Model> Models { get; set; }

        public MeshProjection(ConsoleEngine engine)
        {
            Models = new List<Model>();
            lightDirection = Vector3.Normalize(lightDirection);

            CameraPosition = new Vector3(0, 0, -4.0f);
            LookDirection = new Vector3(0, 0, -1.0f);

            this.consoleHeight = engine.WindowSize.Y;
            this.consoleWidth = engine.WindowSize.X;
            this.Engine = engine;

            aspectRatio = (float)consoleHeight / (float)consoleWidth;
        }

        private readonly int consoleHeight;
        private readonly int consoleWidth;
        private readonly float aspectRatio;

        public Vector3 CameraPosition { get; set; }
        public float CameraRotation { get; set; }

        public Vector3 LookDirection { get; set; }
        private Vector3 lightDirection = new Vector3(0.0f, 0.0f, 0f);

        private List<Triangle> trianglesToRaster = new List<Triangle>();

        public bool DrawWireframes { get; set; } = false;

        private Matrix projectionMatrix;
        private Matrix viewMatrix;

        public void UpdateViewMatrix()
        {
            viewMatrix = Matrix.LookAtLH(CameraPosition, LookDirection, Vector3.UnitY);

            viewMatrix *= Matrix.RotationMatrixY(CameraRotation);

            float near = 0.1f;
            float far = 100.0f;
            float fov = 70.0f;

            projectionMatrix = Matrix.ProjectionMatrix(fov, aspectRatio, near, far);
            projectionMatrix *= Matrix.RotationMatrixY(CameraRotation);
        }

        public void UpdateVisibleFaces()
        {
            foreach (var model in Models)
            {
                var mesh = model.Mesh;
                var modelPosition = model.Position;
                var yRot = model.RotationY;
                var xRot = model.RotationX;

                for (int i = 0; i < mesh.Triangles.Length; i++)
                {
                    Triangle vertex = mesh.Triangles[i];

                    var worldMatrix = Matrix.RotationMatrixY(yRot) * Matrix.RotationMatrixX(xRot) *
                                      Matrix.Translation(modelPosition);

                    var transformMatrix = worldMatrix * viewMatrix * projectionMatrix;

                    Triangle transformed = vertex.MatMul(transformMatrix);

                    Vector3 normal, line1, line2;
                    line1 = transformed.p[1] - transformed.p[0];
                    line2 = transformed.p[2] - transformed.p[0];

                    normal = Vector3.Cross(line1, line2);
                    normal = Vector3.Normalize(normal);

                    if (Vector3.Dot(normal, transformed.p[0] - CameraPosition) < 0.0f)
                    {
                        float l = Vector3.Dot(lightDirection, normal);

                        ConsoleCharacter character = ConsoleCharacter.Light;
                        if (l > 0.4) character = ConsoleCharacter.Medium;
                        if (l > 0.7) character = ConsoleCharacter.Dark;
                        if (l > 1) character = ConsoleCharacter.Full;

                        // projekterar från 3D -> 2D
                        Triangle projected = new Triangle(null);
                        projected = transformed.MatMul(projectionMatrix);

                        Vector3 offsetView = new Vector3(1, 1, 0);
                        projected.p[0] += offsetView;
                        projected.p[1] += offsetView;
                        projected.p[2] += offsetView;

                        projected.p[0].X *= 0.5f * consoleWidth; projected.p[0].Y *= 0.5f * consoleHeight;
                        projected.p[1].X *= 0.5f * consoleWidth; projected.p[1].Y *= 0.5f * consoleHeight;
                        projected.p[2].X *= 0.5f * consoleWidth; projected.p[2].Y *= 0.5f * consoleHeight;

                        projected.c = character;
                        trianglesToRaster.Add(projected);
                    }
                }
            }

            // sortera
            trianglesToRaster.Sort((t1, t2) => ((t2.p[0].Z + t2.p[1].Z + t2.p[2].Z).CompareTo((t1.p[0].Z + t1.p[1].Z + t1.p[2].Z))));
        }

        public void Render()
        {
            foreach (Triangle t in trianglesToRaster)
            {
                Point a = new Point((int)t.p[0].X, (int)t.p[0].Y);
                Point b = new Point((int)t.p[1].X, (int)t.p[1].Y);
                Point c = new Point((int)t.p[2].X, (int)t.p[2].Y);

                if (DrawWireframes)
                    Engine.Triangle(b, a, c, 9, t.c);
                else
                    Engine.Triangle(b, a, c, 10, t.c);
            }

            Engine.SetPixel(new Point(0, 0), 4);

            trianglesToRaster.Clear();
        }
    }

    internal class HelloWorld : ConsoleGame
    {
        private MeshProjection meshProjection;

        private Glyph[,] g;

        private static void Main(string[] args)
        {
            new HelloWorld().Construct(128, 64, 8, 8, FramerateMode.MaxFps);
        }

        public override void Create()
        {
            Engine.SetPalette(Palettes.Default);
            TargetFramerate = 30;

            meshProjection = new MeshProjection(Engine);
            meshProjection.Models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(0, 0, 2), RotationX = 1.241f });
            meshProjection.Models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(4, 0, 2), RotationX = 2.151f });
            meshProjection.Models.Add(new Model { Mesh = Mesh.LoadFromObj("Assets/Rock2.obj"), Position = new Vector3(-4, 0, 2), RotationX = 0.21f }); ;
        }

        public override void Update()
        {
            meshProjection.Models[0].RotationY += 0.1f;
            meshProjection.Models[1].RotationY += 0.1f;
            meshProjection.Models[2].RotationY += 0.1f;

            if (Engine.GetKeyDown(ConsoleKey.LeftArrow))
            {
                meshProjection.CameraRotation += 0.05f;
            }

            if (Engine.GetKeyDown(ConsoleKey.RightArrow))
            {
                meshProjection.CameraRotation -= 0.05f;
            }

            if (Engine.GetKeyDown(ConsoleKey.UpArrow))
            {
                float move = 0.1f;
                meshProjection.CameraPosition = new Vector3(meshProjection.CameraPosition.X, meshProjection.CameraPosition.Y, meshProjection.CameraPosition.Z + move);
            }

            if (Engine.GetKeyDown(ConsoleKey.DownArrow))
            {
                float move = -0.1f;
                meshProjection.CameraPosition = new Vector3(meshProjection.CameraPosition.X, meshProjection.CameraPosition.Y, meshProjection.CameraPosition.Z + move);
            }

            meshProjection.UpdateViewMatrix();
            meshProjection.UpdateVisibleFaces();
        }

        public override void Render()
        {
            Engine.ClearBuffer();

            meshProjection.Render();

            Engine.DisplayBuffer();
        }
    }
}