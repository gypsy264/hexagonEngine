using Project.Core;
using Project.Graphics;
using Project.Game;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Project
{
    public class Hexabeat
    {
        private Renderer _renderer;
        private Matrix4 _projectionMatrix;
        private Scene _scene;
        private GameObject _controlledObject;
        private GameObject _background;
        private Camera _camera; // Add the camera instance
        public GameObject ControlledObject => _controlledObject;

        public Renderer Renderer => _renderer;
        public Scene Scene => _scene;
        public Level level1 { get; private set; }

        public Hexabeat(Scene scene, Engine engine)
        {
            _scene = scene;
            Initialize(engine);
        }

        public void Initialize(Engine engine)
        {
            _renderer = new Renderer();
            _camera = new Camera(Vector3.UnitZ * 3, 1280f / 720f); // Initialize the camera

            // Create levels
            Level level1 = CreateLevel1(engine);
            Level level2 = CreateLevel2(engine);

            // Add levels to the scene
            _scene.AddLevel(level1);
            _scene.AddLevel(level2);

            // Set the initial level
            _scene.SetCurrentLevel(0);
        }

        private Level CreateLevel1(Engine engine)
        {
            var level = new Level(_renderer, engine);

            // Create the Background GameObject
            Vector3 bgPosition = new Vector3(0.0f, 0.0f, 0.0f);
            string bgTexturePath = "./local/currentSong/photobg.png";
            _background = new GameObject("Background",bgPosition, Vector3.One, bgTexturePath, _renderer, engine, layer: -1);
            _scene.AddGameObject(_background);

           

            // Create other game objects for level 1
            Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 scale = new Vector3(0.2f, 0.2f, 1.0f); // Base scale
            string texturePath = "Graphics/Resources/tebi.png";
            _controlledObject = new GameObject("Tebi", position, scale, texturePath, _renderer, engine, layer: 1, colliderSize: new Vector2(0.2f, 0.2f));
            _controlledObject.AddScript(new Player(this, engine._audioAnalyzer));
            level.AddGameObject(_controlledObject);
            

            Vector3 posobject1 = new Vector3(0.5f, 0.0f, 0.0f);
            Vector3 Scaleobject1 = new Vector3(0.2f, 0.2f, 1.0f); // Base scale
            string obj1texturePath = "Graphics/Resources/container.png";
            var obj1Object= new GameObject("Sherk", posobject1, Scaleobject1, obj1texturePath, _renderer, engine, layer: 1, colliderSize: new Vector2(0.2f, 0.2f));
            level.AddGameObject(obj1Object);

            return level;
        }

        private Level CreateLevel2(Engine engine)
        {
            var level = new Level(_renderer, engine);

            // Create game objects for level 2
            Vector3 position = new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 scale = new Vector3(0.2f, 0.2f, 1.0f); // Base scale
            string texturePath = "Graphics/Resources/container.png";
            var otherObject = new GameObject("Tebi",position, scale, texturePath, _renderer, engine, layer: 1);
            level.AddGameObject(otherObject);

            

            return level;
        }

        public void SetProjectionMatrix(Matrix4 projectionMatrix)
        {
            _projectionMatrix = projectionMatrix;
            _renderer.SetProjectionMatrix(_projectionMatrix);
        }

        public void Update(float deltaTime)
        {
            // Update the camera to follow the controlled object
            _camera.Update(_controlledObject.Position, deltaTime);
            _scene.Update(deltaTime);
            //_scene.CheckCollisions();
        }

        public void OnWindowResize(float windowWidth, float windowHeight)
        {
            AdjustBackgroundScale(windowWidth, windowHeight);
        }

        private void AdjustBackgroundScale(float windowWidth, float windowHeight)
        {
            if (_background != null && _background.Texture != null)
            {
                float textureAspectRatio = (float)_background.Texture.Width / _background.Texture.Height;
                float windowAspectRatio = windowWidth / windowHeight;

                if (windowAspectRatio > textureAspectRatio)
                {
                    _background.Scale = new Vector3(2.0f * windowAspectRatio / textureAspectRatio, 2.0f, 1.0f);
                }
                else
                {
                    _background.Scale = new Vector3(2.0f, 2.0f * textureAspectRatio / windowAspectRatio, 1.0f);
                }
            }
        }
    }
}
