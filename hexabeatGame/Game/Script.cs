using Project.Core;

namespace Project.Game
{
    public abstract class Script
    {
        public GameObject GameObject { get; set; }

        public virtual void Initialize() { }

        public virtual void Update(float deltaTime) { }

        public virtual void Render() { }

        public virtual void UnloadContent() { }

        public virtual void OnCollision(GameObject other) { }

        public virtual void OnDestroy() {}

        
    }
}
