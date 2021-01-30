using ConsoleGameEngine;

namespace RexMinus1
{
    internal class RexMinus1 : ConsoleGame
    {
        private ModelRenderer modelRenderer;
        private SpriteRenderer spriteRenderer;

        private LevelManager levelManager;

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
            levelManager = new LevelManager();

            // ładowanie poziomów i ekranów
            levelManager.Add(new Levels.Intro());
            levelManager.Add(new Levels.Test());

            // inicjalizacja podstawowych rzeczy w poziomach
            // i ladowanie do pamięci wszystkiego
            levelManager.Initialize(Engine, modelRenderer, spriteRenderer);
        }

        // co każdą klatkę - tutaj obliczenia
        public override void Update()
        {
            levelManager.CurrentLevel.Update();
        }

        // co każdą klatkę - tutaj rendering
        public override void Render()
        {
            Engine.ClearBuffer();

            // SCENE RENDER
            levelManager.CurrentLevel.Render();

            // HUD RENDER
            levelManager.CurrentLevel.DrawDebug();

            Engine.DisplayBuffer();
        }
    }
}