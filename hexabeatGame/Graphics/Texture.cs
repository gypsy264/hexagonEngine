using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Project.Graphics
{
    public class Texture
    {
        public int Handle { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Texture(string path)
        {
            Handle = GL.GenTexture();
            Use();

            using (Image<Rgba32> image = Image.Load<Rgba32>(path))
            {
                Width = image.Width;
                Height = image.Height;

                image.Mutate(x => x.Flip(FlipMode.Vertical));

                var pixels = new byte[4 * image.Width * image.Height];
                image.CopyPixelDataTo(pixels);

                GL.TexImage2D(TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Rgba,
                    PixelType.UnsignedByte,
                    pixels);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public static Texture LoadFromFile(string path)
        {
            return new Texture(path);
        }
    }
}
