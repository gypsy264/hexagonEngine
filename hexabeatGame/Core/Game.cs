namespace Project.Core
{
    public abstract class Game
    {
        public abstract void Initialize();
        public abstract void Update(float deltaTime);
        public abstract void Render();
        public abstract void LoadContent();
        public abstract void UnloadContent();
    }
}
