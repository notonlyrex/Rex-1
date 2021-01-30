using ConsoleGameEngine;
using RexMinus1.GameObjects;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RexMinus1
{
    public abstract class Level
    {
        protected string ShieldText = "  SHIELD  ";
        protected string EnergyText = "  ENERGY  ";
        protected string HeatText = "   HEAT   ";

        protected List<Model> models = new List<Model>();

        private Sprite hud;
        private Sprite hud_left;
        private Sprite hud_middle;
        private Sprite hud_right;

        protected float speed;

        public ModelRenderer ModelRenderer { get; set; }

        public ConsoleEngine Engine { get; set; }

        public SpriteRenderer SpriteRenderer { get; set; }

        public LevelManager LevelManager { get; set; }

        public AnimationRenderer AnimationRenderer { get; set; }

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

        public void DrawHud()
        {
            SpriteRenderer.RenderSingle(new Point(0, 0), hud);
            SpriteRenderer.RenderSingle(new Point(12, 59), hud_left);
            SpriteRenderer.RenderSingle(new Point(47, 59), hud_middle);
            SpriteRenderer.RenderSingle(new Point(83, 59), hud_right);

            DrawBar(new Point(13, 59), 31, PlayerManager.Instance.Shield, 4);
            DrawBar(new Point(48, 59), 31, PlayerManager.Instance.Energy, 4);
            DrawBar(new Point(83, 59), 31, PlayerManager.Instance.Heat, 4);

            Engine.WriteText(new Point(24, 60), ShieldText, 2);
            Engine.WriteText(new Point(59, 60), EnergyText, 2);
            Engine.WriteText(new Point(94, 60), HeatText, 2);

            Engine.WriteText(new Point(7, 32), speed.ToString("F2"), 2);
        }

        public void DrawBar(Point origin, int size, float value, int color)
        {
            int end = (int)(size * value);
            Engine.Line(origin, new Point(origin.X + end, origin.Y), color);
        }

        protected void PlayAnimation(Animation anim)
        {
            anim.IsPaused = false;
            AnimationRenderer.Add(anim);
        }

        public virtual void Start()
        {
            AnimationRenderer.Clear();
            ModelRenderer.CameraPosition = ModelRenderer.StartingCameraPosition;
            ModelRenderer.CameraRotation = 0.0f;
        }

        public virtual void Create()
        {
            ModelRenderer.UpdateCameraRotation(0.0f);

            hud = Sprite.FromFile("Assets/hud.png");
            hud_left = Sprite.FromFile("Assets/hud_bottom_bar_left.png");
            hud_middle = Sprite.FromFile("Assets/hud_bottom_bar_middle.png");
            hud_right = Sprite.FromFile("Assets/hud_bottom_bar_right.png");
        }

        public virtual void MoveObjects()
        {
            // aktualizacja pozycji obiektów
            foreach (var item in models.OfType<IMoveable>())
            {
                item.Move();
            }
        }

        public virtual void CheckCollisions()
        {
            // uruchamiania efektów kolizji gracza ze wszystkimi obiektami
            foreach (var item in models.OfType<ICollision>())
            {
                if (item.Collision(ModelRenderer.CameraPosition))
                {
                    PlayerManager.Instance.Energy -= item.CollisionAttack;
                    PlayerManager.Instance.Shield -= item.CollisionAttack;
                }
            }
        }

        public virtual void RemoveObjects()
        {
            // usuwanie zniszczonych wrogów
            models.RemoveAll(x => x is Enemy && (x as Enemy).Shield <= 0);

            // usuwanie zebranych astronautów
            models.RemoveAll(x => x.GetType() == typeof(Astronaut) && (x as Astronaut).IsCollected);
        }

        public virtual bool CheckWin()
        {
            // sprawdzenie warunku zwycięstwa
            return models.OfType<Astronaut>().Count() == 0;
        }

        public virtual void UpdateHeatEnergy()
        {
            // aktualizacja ciepła i energii
            if (PlayerManager.Instance.Heat > 0)
                PlayerManager.Instance.Heat -= 0.01f;

            if (PlayerManager.Instance.Energy < 1)
                PlayerManager.Instance.Energy += 0.01f;
        }

        public virtual bool CheckLose()
        {
            // sprawdzenie warunków przegranej
            if (PlayerManager.Instance.Energy <= 0.05)
                return true;

            if (PlayerManager.Instance.Heat >= 0.95)
                return true;

            return false;
        }

        public virtual void Update()
        {
            MoveObjects();
            CheckCollisions();
            RemoveObjects();

            UpdateHeatEnergy();

            ModelRenderer.UpdateViewMatrix();
            ModelRenderer.UpdateVisibleFaces(models);
        }

        public virtual void Render()
        {
            ModelRenderer.Render();
            AnimationRenderer.Render();
        }
    }
}