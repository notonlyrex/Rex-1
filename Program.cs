using ConsoleGameEngine;
using System.Collections.Generic;

namespace RexMinus1
{
    internal class RexMinus1 : ConsoleGame
    {
        private ModelRenderer modelRenderer;
        private SpriteRenderer spriteRenderer;

        private Sprite test;
        private List<Level> levels = new List<Level>();
        private int currentLevel = 0;

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

            // ładowanie rendererów
            modelRenderer = new ModelRenderer(Engine);
            spriteRenderer = new SpriteRenderer(Engine);

            // ładowanie poziomów i ekranów
            levels.Add(new Levels.Test());

            // inicjalizacja podstawowych rzeczy w poziomach
            // i ladowanie do pamięci wszystkiego
            foreach (var item in levels)
            {
                item.Engine = Engine;
                item.ModelRenderer = modelRenderer;
                item.SpriteRenderer = spriteRenderer;

                item.Create();
            }
        }

        // co każdą klatkę - tutaj obliczenia
        public override void Update()
        {
            levels[currentLevel].Update();
        }

        // co każdą klatkę - tutaj rendering
        public override void Render()
        {
            Engine.ClearBuffer();

            // SCENE RENDER
            levels[currentLevel].Render();

            // HUD RENDER
            levels[currentLevel].DrawDebug();

            Engine.DisplayBuffer();
        }
    }
}