using Project.Core;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Project.Graphics;
using Project.Game;

namespace Project.Game
{
    public class Player : Script
    {

        private Hexabeat _hexabeat;

        public Player(Hexabeat hexabeat)
        {
            _hexabeat = hexabeat;
        }

        public override void Update(float deltaTime)
        {



            if (GameObject.Collider != null)
            {
                GameObject.Collider.Update();
            }

            var keyboardState = GameObject.Engine.KeyboardState; // Get the keyboard state from the engine

            if (keyboardState.IsKeyDown(Keys.T))
            {

                
                    // Create a new GameObject
                GameObject spawnedObject = new GameObject("SpawnedObject", GameObject.Position, new Vector3(1, 1, 1), "Graphics/Resources/moneybagSprite.png", _hexabeat.Renderer, GameObject.Engine, layer: 1);
                // Add the spawned object to the current level
                _hexabeat.Scene.GetCurrentLevel().AddGameObject(spawnedObject);
                
            }

            if (keyboardState.IsKeyDown(Keys.Q))
            {
                GameObject.Destroy();
            }
            if (keyboardState.IsKeyDown(Keys.E))
            {
                GameObject.Engine.CurrentScene.SetCurrentLevel(1); // Change level
            }
            if (keyboardState.IsKeyDown(Keys.R))
            {
                GameObject.Engine.CurrentScene.SetCurrentLevel(0); // Change level
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                GameObject.Position += new Vector3(0, 1f, 0) * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                GameObject.Position += new Vector3(0, -1f, 0) * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                GameObject.Position += new Vector3(-1f, 0, 0) * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                GameObject.Position += new Vector3(1f, 0, 0) * deltaTime;
            }
        }

        public override void OnCollision(GameObject other)
        {
            GameObject.Destroy();
            other.Destroy();
            Console.WriteLine($"{this.GetType().Name} collided with {other.GetType().Name}");
        }
    }
}
