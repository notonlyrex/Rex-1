using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGameEngine
{
    public class ModelRenderer
    {
        private readonly ConsoleEngine Engine;

        public ModelRenderer(ConsoleEngine engine)
        {
            lightDirection = Vector3.Normalize(lightDirection);

            CameraPosition = new Vector3(0, 0, -4.0f);
            CameraRotation = 0.0f;

            CameraForward = new Vector3(0, 0, 1);
            CameraLeft = new Vector3(0, 0, 1);
            CameraRight = new Vector3(0, 0, -1);

            //LookDirection = new Vector3(0, 0, -1.0f);

            this.consoleHeight = engine.WindowSize.Y;
            this.consoleWidth = engine.WindowSize.X;
            this.Engine = engine;

            aspectRatio = (float)consoleHeight / (float)consoleWidth;

            projectionMatrix = Matrix.ProjectionMatrix(fov, aspectRatio, near, far);
        }

        private readonly int consoleHeight;
        private readonly int consoleWidth;
        private readonly float aspectRatio;

        public Vector3 CameraPosition { get; set; }

        public Vector3 CameraForward { get; set; }
        public Vector3 CameraLeft { get; set; }
        public Vector3 CameraRight { get; set; }
        public float CameraRotation { get; set; }

        public float near = 0.1f;
        public float far = 180.0f;
        public float fov = 60.0f;

        //public Vector3 LookDirection { get; set; }
        private Vector3 lightDirection = new Vector3(0.0f, 0.0f, 0f);

        private List<Triangle> trianglesToRaster = new List<Triangle>();

        public bool DrawWireframes { get; set; } = false;

        private Matrix projectionMatrix;
        private Matrix viewMatrix;

        public void UpdateCameraRotation(float r)
        {
            CameraRotation += r;

            CameraForward = CustomMath.SimpleRotateVectorByRadian(CameraForward, r);
            CameraLeft = CustomMath.SimpleRotateVectorByDegree(CameraForward, 90);
            CameraRight = CustomMath.SimpleRotateVectorByDegree(CameraForward, -90);

            projectionMatrix *= Matrix.RotationMatrixY(r);
        }

        public void UpdateCameraMovement(float mov_forward, float mov_pane)
        {
            CameraPosition = new Vector3(CameraPosition.X, CameraPosition.Y, CameraPosition.Z) + CameraForward * mov_forward + CameraLeft * mov_pane;
        }

        public void UpdateViewMatrix()
        {
            viewMatrix = Matrix.LookAtLH(CameraPosition, CameraForward, Vector3.UnitY);
            viewMatrix *= Matrix.RotationMatrixY(CameraRotation);
        }

        public void UpdateFOV(float f)
        {
            fov += f;
            projectionMatrix = Matrix.ProjectionMatrix(fov, aspectRatio, near, far);
        }

        public void UpdateVisibleFaces(List<Model> batch)
        {
            foreach (var model in batch)
            {
                var mesh = model.Mesh;
                var modelPosition = model.Position;
                var yRot = model.RotationY;
                var xRot = model.RotationX;

                var modelMatrix = Matrix.RotationMatrixY(yRot) * Matrix.RotationMatrixX(xRot) *
                                      Matrix.Translation(modelPosition);

                var transformMatrix = modelMatrix * viewMatrix * projectionMatrix;

                for (int i = 0; i < mesh.Triangles.Length; i++)
                {
                    Triangle tri = mesh.Triangles[i];
                    Triangle transformed = tri.MatMul(transformMatrix);

                    Vector3 normal, line1, line2;

                    line1 = transformed.p[1] - transformed.p[0];
                    line2 = transformed.p[2] - transformed.p[0];

                    normal = Vector3.Cross(line1, line2);
                    normal = Vector3.Normalize(normal);

                    if (Vector3.Dot(normal, transformed.p[0] - CameraPosition) < 0.0f)
                    {
                        // projekcja 3D -> 2D
                        Triangle projected = new Triangle(null);
                        projected = transformed.MatMul(projectionMatrix);

                        Vector3 offsetView = new Vector3(1, 1, 0);
                        projected.p[0] += offsetView;
                        projected.p[1] += offsetView;
                        projected.p[2] += offsetView;

                        projected.p[0].X *= 0.5f * consoleWidth; projected.p[0].Y *= 0.5f * consoleHeight;
                        projected.p[1].X *= 0.5f * consoleWidth; projected.p[1].Y *= 0.5f * consoleHeight;
                        projected.p[2].X *= 0.5f * consoleWidth; projected.p[2].Y *= 0.5f * consoleHeight;

                        float l = Vector3.Dot(lightDirection, normal);
                        ConsoleCharacter character;
                        if (l > 1) character = ConsoleCharacter.Full;
                        else if (l > 0.7) character = ConsoleCharacter.Dark;
                        else if (l > 0.4) character = ConsoleCharacter.Medium;
                        else character = ConsoleCharacter.Light;
                        projected.c = character;

                        trianglesToRaster.Add(projected);
                    }
                }
            }

            // sortowanie
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

            trianglesToRaster.Clear();
        }
    }
}