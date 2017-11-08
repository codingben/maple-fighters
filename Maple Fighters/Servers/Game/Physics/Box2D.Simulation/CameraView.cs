using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Physics.Box2D.PhysicsSimulation
{
    public class CameraView
    {
        public Vector2 Position;
        public float Zoom;

        public CameraView(Vector2 position, float zoom)
        {
            Position = position;
            Zoom = zoom;
        }

        public void Update()
        {
            GL.LoadIdentity();

            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-Position.X, -Position.Y, 0));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1.0f));

            GL.MultMatrix(ref transform);
        }
    }
}