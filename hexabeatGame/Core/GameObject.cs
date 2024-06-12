using OpenTK.Mathematics;
using Project.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using Project.Game;

namespace Project.Core
{
    public class GameObject
    {

        public string Tag { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public string TexturePath { get; set; }
        public int Layer { get; set; }
        public Texture Texture { get; private set; }
        public Engine Engine { get; private set; }
        private Renderer _renderer;
        private List<Script> _scripts;
        public BoxCollider Collider { get; private set; }
        public bool IsDestroyed { get; private set; }
        public bool ShouldRemove { get; set; }

        public Renderer Renderer => _renderer;
        

        public GameObject(string tag, Vector3 position, Vector3 scale, string texturePath, Renderer renderer, Engine engine, int layer = 0, Vector2? colliderSize = null)
        {
            Tag = tag;
            Position = position;
            Scale = scale;
            TexturePath = texturePath;
            _renderer = renderer;
            Engine = engine;
            Layer = layer;
            _scripts = new List<Script>();
            LoadContent();

            if (colliderSize.HasValue)
            {
                EnableCollision(colliderSize.Value);
            }
        }

        public void AddScript(Script script)
        {
            script.GameObject = this;
            _scripts.Add(script);
            script.Initialize();
        }

        public void EnableCollision(Vector2 size)
        {
            Collider = new BoxCollider(this, size);
        }

        public void DisableCollision()
        {
            Collider = null;
        }

        public void Destroy()
        {
            
            IsDestroyed = true;
            ShouldRemove = true;
            foreach (var script in _scripts)
            {
                script.OnDestroy();
            }
            _scripts.Clear();
            //Collider?.Dispose();
            Collider = null;
            
        }

        public virtual void LoadContent()
        {
            Texture = Texture.LoadFromFile(TexturePath);
            AdjustScale();
        }

        public T GetScript<T>() where T : Script
        {
            return _scripts.Find(s => s is T) as T;
        }

        public virtual void Update(float deltaTime)
        {
            if (IsDestroyed) return;

            foreach (var script in _scripts)
            {
                script.Update(deltaTime);
            }

            if (Collider != null)
            {
                Collider.Update();
            }
        }


        public virtual void Render()
        {
            if (IsDestroyed) return;

            Texture.Use(TextureUnit.Texture0);
            _renderer.Render(Texture, GetModelMatrix());
        }

        public virtual void UnloadContent()
        {
            foreach (var script in _scripts)
            {
                script.UnloadContent();
            }
        }

        private void AdjustScale()
        {
            if (Texture != null)
            {
                float aspectRatio = (float)Texture.Width / Texture.Height;
                Scale = new Vector3(Scale.X * aspectRatio, Scale.Y, Scale.Z);
            }
        }

        private Matrix4 GetModelMatrix()
        {
            return Matrix4.CreateScale(Scale) * Matrix4.CreateTranslation(Position);
        }

        public virtual void OnCollision(GameObject other)
        {
            //Console.WriteLine($"{this.GetType().Name} collided with {other.GetType().Name}");
            // Override this method in derived classes to handle collisions
        }
    }
}