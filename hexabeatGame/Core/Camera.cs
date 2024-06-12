using OpenTK.Mathematics;

namespace Project.Core
{
    public class Camera
    {
        private Vector3 _position;
        private float _aspectRatio;

        public Camera(Vector3 position, float aspectRatio)
        {
            _position = position;
            _aspectRatio = aspectRatio;
        }

        public void Update(Vector3 targetPosition, float deltaTime)
        {
            // Implement logic to follow the targetPosition
            _position = Vector3.Lerp(_position, targetPosition, deltaTime);
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(_position, _position + Vector3.UnitZ, Vector3.UnitY);
        }
    }
}
