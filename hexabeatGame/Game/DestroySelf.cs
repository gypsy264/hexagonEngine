using Project.Core;
using Project.Core;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Project.Graphics;
using Project.Game;


namespace Project.Game
{
    public class DestroySelf : Script
    {
        public DestroySelf(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public override void Initialize()
        {
            Console.WriteLine("ScriptTest For Random Object");
        }

        public override void Update(float deltaTime)
        {
            // Update your script here
        }

        public override void Render()
        {
            // Render your script here
        }

        public override void UnloadContent()
        {
            // Unload content here
        }

        public override void OnCollision(GameObject other)
        {
            // Handle collision here
        }

        public override void OnDestroy()
        {
            // Handle destroy here
        }
    }
}