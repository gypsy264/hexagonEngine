using OpenTK.Mathematics;
using System;

namespace Project.Core
{
    public struct AxisAlignedBoundingBox
    {
        public Vector2 Min;
        public Vector2 Max;

        public AxisAlignedBoundingBox(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public bool Intersects(AxisAlignedBoundingBox other)
        {
            return Min.X <= other.Max.X && Max.X >= other.Min.X &&
                   Min.Y <= other.Max.Y && Max.Y >= other.Min.Y;
        }
    }

    public class BoxCollider
    {
        public GameObject GameObject { get; }
        public Vector2 Size { get; set; }
        public AxisAlignedBoundingBox AABB { get; private set; }

        public BoxCollider(GameObject gameObject, Vector2 size)
        {
            GameObject = gameObject;
            Size = size;
            Update();
        }

        public void Update()
        {
            Vector2 min = new Vector2(GameObject.Position.X - Size.X / 2, GameObject.Position.Y - Size.Y / 2);
            Vector2 max = new Vector2(GameObject.Position.X + Size.X / 2, GameObject.Position.Y + Size.Y / 2);
            AABB = new AxisAlignedBoundingBox(min, max);
            //Console.WriteLine($"AABB Updated: Min({AABB.Min.X}, {AABB.Min.Y}), Max({AABB.Max.X}, {AABB.Max.Y})");
        }

        public bool Intersects(BoxCollider other)
        {
            return AABB.Intersects(other.AABB);
        }
    }
}
