using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace Project.Graphics
{
    public class Shader
{
    public int Handle { get; private set; }

    public Shader(string vertexPath, string fragmentPath)
    {
        var vertexShader = CompileShader(vertexPath, ShaderType.VertexShader);
        var fragmentShader = CompileShader(fragmentPath, ShaderType.FragmentShader);

        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        GL.LinkProgram(Handle);

        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int status);
        if (status == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            throw new Exception($"Program linking failed: {infoLog}");
        }
    }

    private int CompileShader(string path, ShaderType type)
    {
        string source = File.ReadAllText(path);
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
        if (status == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"{type} compilation failed: {infoLog}");
        }

        return shader;
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public int GetAttribLocation(string name)
    {
        return GL.GetAttribLocation(Handle, name);
    }
}
}
