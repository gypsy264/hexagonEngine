using OpenTK.Mathematics;

namespace Project.Core
{
    public class AABB
    {
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }

        public AABB(Vector2 min, Vector2 size)
        {
            Min = min;
            Max = min + size;
        }

        public bool Intersects(AABB other)
        {
            return Max.X > other.Min.X && Min.X < other.Max.X &&
                   Max.Y > other.Min.Y && Min.Y < other.Max.Y;
        }

        public void Update(Vector3 position, Vector2 size)
        {
            Min = new Vector2(position.X - size.X / 2, position.Y - size.Y / 2);
            Max = new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
            //Console.WriteLine($"AABB Updated: Min({Min.X}, {Min.Y}), Max({Max.X}, {Max.Y})");
        }
    }
}
