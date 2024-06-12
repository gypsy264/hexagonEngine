using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Project.Graphics;
using ImGuiNET;


namespace Project.Core
{
    public class Engine : GameWindow
    {

        
        //private ImGuiController _imGuiController;
        public Scene CurrentScene { get; private set; }
        private Hexabeat _hexabeat;
        private Matrix4 _projectionMatrix;
        private int _frameCount;
        private double _timeElapsed;

        public Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
           
            //_imGuiController = new ImGuiController(Size.X, Size.Y);
        }

        public void LoadScene(Scene scene)
        {
            Console.WriteLine("Hexagon Engine - Build 0.0.0.1 INTERNAL DEV");
            Console.WriteLine("Loading Scene/Context");
            CurrentScene?.UnloadContent();
            CurrentScene = scene;
            Console.WriteLine($"Scene DATA: {scene}");
            CurrentScene.LoadContent();
        }

        public void SetHexabeat(Hexabeat hexabeat)
        {
            _hexabeat = hexabeat;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            UpdateProjectionMatrix();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            

            CurrentScene?.Render();

            // ImGui Rendering
            try
            {
               // _imGuiController.Update(this, (float)e.Time);
                //_imGuiController.Render();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnRenderFrame: {ex}");
                throw;
            }

            SwapBuffers();

            // Calculate FPS
            _frameCount++;
            _timeElapsed += e.Time;
            if (_timeElapsed >= 1.0)
            {
                Title = $"Hexabeat - FPS: {_frameCount}";
                Console.WriteLine(Title);
                _frameCount = 0;
                _timeElapsed = 0.0;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }

            _hexabeat?.Update((float)e.Time);
            CurrentScene?.Update((float)e.Time);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            UpdateProjectionMatrix();
            _hexabeat?.OnWindowResize(Size.X, Size.Y);
            //_imGuiController.WindowResized(Size.X, Size.Y);
        }

        private void UpdateProjectionMatrix()
        {
            float aspectRatio = Size.X / (float)Size.Y;
            float height = 2.0f;
            float width = height * aspectRatio;

            _projectionMatrix = Matrix4.CreateOrthographic(width, height, 0.1f, 100.0f);

            // Update the projection matrix in Hexabeat
            _hexabeat?.SetProjectionMatrix(_projectionMatrix);
        }

        protected override void OnUnload()
        {
            CurrentScene?.UnloadContent();
            //_imGuiController?.Dispose();
            base.OnUnload();
        }
    }
}
