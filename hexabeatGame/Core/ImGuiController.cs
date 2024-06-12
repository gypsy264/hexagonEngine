using System;
using ImGuiNET;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

public class ImGuiController : IDisposable
{
    private IntPtr _context;

    public ImGuiController(int width, int height)
    {
        Console.WriteLine("Creating ImGui context...");
        _context = ImGui.CreateContext();
        ImGui.SetCurrentContext(_context);

        var io = ImGui.GetIO();
        io.Fonts.AddFontDefault();

        ImGui.StyleColorsDark();

        GL.Viewport(0, 0, width, height);
    }

    public void Update(GameWindow window, float deltaTime)
    {
        try
        {
            Console.WriteLine("Updating ImGui frame...");
            ImGui.SetCurrentContext(_context);
            ImGui.NewFrame();

            var io = ImGui.GetIO();
            io.DisplaySize = new System.Numerics.Vector2(window.Size.X, window.Size.Y);
            io.DeltaTime = deltaTime;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Update: {ex}");
            throw;
        }
    }

    public unsafe void Render()
    {
        try
        {
            Console.WriteLine("Rendering ImGui frame...");
            ImGui.Render();
            ImDrawDataPtr drawDataPtr = ImGui.GetDrawData();
            IntPtr drawDataIntPtr = (IntPtr)drawDataPtr.NativePtr;

            RenderDrawData(drawDataIntPtr);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Render: {ex}");
            throw;
        }
    }

    public void WindowResized(int width, int height)
    {
        GL.Viewport(0, 0, width, height);
    }

    public void Dispose()
    {
        Console.WriteLine("Destroying ImGui context...");
        ImGui.DestroyContext(_context);
    }

    [DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
    private static extern void RenderDrawData(IntPtr draw_data);
}