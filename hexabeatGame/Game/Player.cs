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
        private AudioAnalyzer _audioAnalyzer;
        private bool HasBeenClick = false;

        public Player(Hexabeat hexabeat, AudioAnalyzer audioAnalyzer)
        {
            _hexabeat = hexabeat;
            _audioAnalyzer = audioAnalyzer;
            _audioAnalyzer.OnBeatDetected += HandleBeatDetected;
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

                
                if(HasBeenClick == false){
                    // Create a new GameObject
                    HasBeenClick = true;
                    GameObject spawnedObject = new GameObject("SpawnedObject", GameObject.Position, new Vector3(1, 1, 1), "Graphics/Resources/moneybagSprite.png", _hexabeat.Renderer, GameObject.Engine, layer: 2);
                    spawnedObject.AddScript(new DestroySelf(spawnedObject));
                    // Add the spawned object to the current level
                    _hexabeat.Scene.GetCurrentLevel().AddGameObject(spawnedObject);
                    Console.WriteLine(HasBeenClick);
                }else{
                    HasBeenClick = true;
                    Console.WriteLine(HasBeenClick);
                }
                
                
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

        private void HandleBeatDetected()
        {
            SpawnObject(GameObject.Position);
        }

        private void SpawnObject(Vector3 position)
        {
            GameObject spawnedObject = new GameObject("SpawnedObject", position, new Vector3(1, 1, 1), "Graphics/Resources/moneybagSprite.png", _hexabeat.Renderer, GameObject.Engine, layer: 2);
            spawnedObject.AddScript(new DestroySelf(spawnedObject));
            _hexabeat.Scene.GetCurrentLevel().AddGameObject(spawnedObject);
        }


        public override void OnCollision(GameObject other)
        {
            GameObject.Destroy();
            other.Destroy();
            Console.WriteLine($"{this.GetType().Name} collided with {other.GetType().Name}");
        }
    }
}
