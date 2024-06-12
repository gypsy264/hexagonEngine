using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


namespace Project.Graphics
{
    public class Renderer
    {
        private Shader _shader;
        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private Matrix4 _projectionMatrix;

        private readonly float[] _vertices =
        {
            // Positions         // Texture coordinates
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
           -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
           -0.5f,  0.5f, 0.0f, 0.0f, 1.0f
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public Renderer()
        {
            Initialize();
        }

        private void Initialize()
        {
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _shader = new Shader("Graphics/Shaders/shader.vert", "Graphics/Shaders/shader.frag");

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        public void SetProjectionMatrix(Matrix4 projectionMatrix)
        {
            _projectionMatrix = projectionMatrix;
        }

        public void Render(Texture texture, Matrix4 modelMatrix)
        {
            _shader.Use();

            texture.Use(TextureUnit.Texture0);

            int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
            GL.UniformMatrix4(modelLocation, false, ref modelMatrix);

            int projectionLocation = GL.GetUniformLocation(_shader.Handle, "projection");
            GL.UniformMatrix4(projectionLocation, false, ref _projectionMatrix);

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public Vector3 AdjustScale(Texture texture, Vector3 baseScale)
        {
            float textureAspectRatio = (float)texture.Width / texture.Height;
            float screenAspectRatio = (float)1920 / 1080; // Assuming a window resolution of 1280x720

            Vector3 adjustedScale = baseScale;

            if (screenAspectRatio > textureAspectRatio)
            {
                adjustedScale.X *= screenAspectRatio / textureAspectRatio;
            }
            else
            {
                adjustedScale.Y *= textureAspectRatio / screenAspectRatio;
            }

            return adjustedScale;
        }
    }
}
