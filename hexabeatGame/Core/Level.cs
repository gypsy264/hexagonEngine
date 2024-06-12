using Project.Core;
using Project.Graphics;
using System.Collections.Generic;
using OpenTK.Mathematics;
using Project.Game;


namespace Project
{
    public class Level
    {
        private Renderer _renderer;
        private Engine _engine;
        private List<GameObject> _gameObjects;
        

        public Level(Renderer renderer, Engine engine)
        {
            _renderer = renderer;
            _engine = engine;
            _gameObjects = new List<GameObject>();
            _renderer = new Renderer(); // Initialize the renderer here
        }

        public void Initialize()
        {
            // Initialize level-specific objects here
        }

        public void AddGameObject(GameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        public void LoadContent()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.LoadContent();
            }
        }

        public void AddGameObjectDynamic(string tag, Vector3 position, Vector3 scale, string texturePath, int layer)
        {
            var gameObject = new GameObject(tag, position, scale, texturePath, _renderer, _engine, layer);
            _gameObjects.Add(gameObject);
        }

public void Update(float deltaTime)
{
    var gameObjectsToRemove = new List<GameObject>();

    for (int i = _gameObjects.Count - 1; i >= 0; i--)
    {
        var gameObject = _gameObjects[i];
        gameObject.Update(deltaTime);

        if (gameObject.ShouldRemove)
        {
            gameObjectsToRemove.Add(gameObject);
        }
    }

    foreach (var gameObject in gameObjectsToRemove)
    {
        _gameObjects.Remove(gameObject);
    }

    //_currentLevel?.Update(deltaTime);
    CheckCollisions();
}

        public void Render()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Render();
            }
        }

        

        private void CheckCollisions()
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                //Console.WriteLine($"Checking collisions for {_gameObjects[i].Tag} (GameObject {i})");

                var colliderA = _gameObjects[i].Collider;
                if (colliderA == null)
                {
                    //Console.WriteLine($"{_gameObjects[i].Tag} does not have a collider.");
                    continue;
                }

                colliderA.Update();
                //Console.WriteLine($"AABB Updated: Min({colliderA.AABB.Min.X}, {colliderA.AABB.Min.Y}), Max({colliderA.AABB.Max.X}, {colliderA.AABB.Max.Y})");

                for (int j = 0; j < _gameObjects.Count; j++)
        {
                if (j == i) continue; // skip self-collision
                var colliderB = _gameObjects[j].Collider;
                if (colliderB == null)
                {
                    //Console.WriteLine($"{_gameObjects[j].Tag} does not have a collider.");
                    continue;
                }

                    colliderB.Update();
                     if (colliderA.Intersects(colliderB))
                    {
                        Console.WriteLine($"{_gameObjects[i].Tag} collided with {_gameObjects[j].Tag}");
                        var script = _gameObjects[i].GetScript<Player>();
                        if (script != null)
                        {
                            script.OnCollision(_gameObjects[j]);
                        }
                        script = _gameObjects[j].GetScript<Player>();
                        if (script != null)
                        {
                            script.OnCollision(_gameObjects[i]);
                        }
                    }
                }
            }
        }

        public void UnloadContent()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.UnloadContent();
            }
        }
    }
}
