using System.Collections.Generic;

namespace ConsoleGameEngine
{
    public class AnimationRenderer
    {
        private readonly ConsoleEngine engine;

        private List<Animation> batch;

        public AnimationRenderer(ConsoleEngine engine)
        {
            this.engine = engine;
            batch = new List<Animation>();
        }

        public void Add(Animation anim)
        {
            anim.Engine = engine;

            if (!batch.Contains(anim))
                batch.Add(anim);
        }

        public void Remove(Animation anim)
        {
            batch.Remove(anim);
        }

        public void Clear()
        {
            batch.Clear();
        }

        public void Render()
        {
            foreach (var item in batch)
            {
                item.Render();
            }
        }
    }
}