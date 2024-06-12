using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Project.Core;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
//using Avalonia.ReactiveUI;

namespace Project
{
    public static class Program
    {
        private static void Main()
        {

            //var hotReloadService = new HotReloadService();
            //var scriptDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Game");
            //var scriptWatcher = new ScriptWatcher(scriptDirectoryPath, hotReloadService);
            var gameWindowSettings = GameWindowSettings.Default;
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1920, 1080),
                WindowState = WindowState.Maximized,
                Title = "Hexabeat",
                Flags = ContextFlags.ForwardCompatible,
                //Flags = ContextFlags.ForwardCompatible,
            };

            using (var engine = new Engine(gameWindowSettings, nativeWindowSettings))
            {
                var scene = new Scene();
                var game = new Hexabeat(scene, engine);

                engine.SetHexabeat(game);
                engine.LoadScene(scene);
                engine.Run();
            }
        }
    }
}
