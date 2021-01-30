using ConsoleGameEngine;
using System.Collections.Generic;
using System.Numerics;

namespace RexMinus1
{
    public abstract class Level
    {
        protected List<Model> models = new List<Model>();

        public ModelRenderer ModelRenderer { get; set; }

        public ConsoleEngine Engine { get; set; }

        public SpriteRenderer SpriteRenderer { get; set; }

        public void DrawDebug()
        {
            Engine.WriteText(new Point(0, 0), "X: " + ModelRenderer.CameraRotation, 14);
            Engine.WriteText(new Point(0, 3), "D: " + Vector3.Distance(ModelRenderer.CameraPosition, models[0].Position), 14);

            Engine.WriteText(new Point(0, 5), "P: " + ModelRenderer.CameraPosition.X + " " + ModelRenderer.CameraPosition.Y + " " + ModelRenderer.CameraPosition.Z, 14);
            Engine.WriteText(new Point(0, 6), "A: " + CustomMath.SimpleAngleBetweenTwoVectors(ModelRenderer.CameraForward, new Vector3(1, 0, 0)), 14);

            Engine.WriteText(new Point(0, 8), "F: " + ModelRenderer.CameraForward.X + " " + ModelRenderer.CameraForward.Y + " " + ModelRenderer.CameraForward.Z, 14);
            Engine.WriteText(new Point(0, 9), "L: " + ModelRenderer.CameraLeft.X + " " + ModelRenderer.CameraLeft.Y + " " + ModelRenderer.CameraLeft.Z, 14);
            Engine.WriteText(new Point(0, 10), "R: " + ModelRenderer.CameraRight.X + " " + ModelRenderer.CameraRight.Y + " " + ModelRenderer.CameraRight.Z, 14);
        }

        public virtual void Create()
        {
            ModelRenderer.UpdateCameraRotation(0.0f);
        }

        public virtual void Update()
        {
            ModelRenderer.UpdateViewMatrix();
            ModelRenderer.UpdateVisibleFaces(models);
        }

        public virtual void Render()
        {
            ModelRenderer.Render();
        }
    }
}