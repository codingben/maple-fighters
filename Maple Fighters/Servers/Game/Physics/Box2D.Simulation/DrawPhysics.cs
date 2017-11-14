using Box2DX.Dynamics;
using System;
using Box2DX.Common;
using OpenTK.Graphics.OpenGL;
using Color = Box2DX.Dynamics.Color;
using Math = System.Math;

namespace Physics.Box2D.PhysicsSimulation
{
    public class DrawPhysics : DebugDraw
    {
        public override void DrawPolygon(Vec2[] vertices, int vertexCount, Color color)
        {
            GL.Color4(color.R, color.G, color.B, 0.5f);
            GL.Begin(PrimitiveType.LineLoop);

            for (var i = 0; i < vertexCount; i++)
            {
                var v = vertices[i];
                GL.Vertex2(v.X, v.Y);
            }

            GL.End();
        }

        public override void DrawSolidPolygon(Vec2[] vertices, int vertexCount, Color color)
        {
            GL.Color4(color.R, color.G, color.B, 0.5f);
            GL.Begin(PrimitiveType.TriangleFan);

            for (var i = 0; i < vertexCount; i++)
            {
                var v = vertices[i];
                GL.Vertex2(v.X, v.Y);
            }

            GL.End();
        }

        public override void DrawCircle(Vec2 center, float radius, Color color)
        {
            const float K_SEGMENTS = 16.0f;
            const int VERTEX_COUNT = 16;
            var kIncrement = 2.0f * Settings.Pi / K_SEGMENTS;
            var theta = 0.0f;

            GL.Color4(color.R, color.G, color.B, 0.5f);
            GL.Begin(PrimitiveType.LineLoop);

            GL.VertexPointer(VERTEX_COUNT * 2, VertexPointerType.Float, 0, IntPtr.Zero);

            for (var i = 0; i < K_SEGMENTS; ++i)
            {
                var v = center + radius * new Vec2((float)Math.Cos(theta), (float)Math.Sin(theta));
                GL.Vertex2(v.X, v.Y);
                theta += kIncrement;
            }

            GL.End();
        }

        public override void DrawSolidCircle(Vec2 center, float radius, Vec2 axis, Color color)
        {
            const float K_SEGMENTS = 16.0f;
            const int VERTEX_COUNT = 16;
            var kIncrement = 2.0f * Settings.Pi / K_SEGMENTS;
            var theta = 0.0f;

            GL.Color4(color.R, color.G, color.B, 0.5f);
            GL.Begin(PrimitiveType.TriangleFan);

            GL.VertexPointer(VERTEX_COUNT * 2, VertexPointerType.Float, 0, IntPtr.Zero);

            for (var i = 0; i < K_SEGMENTS; ++i)
            {
                var v = center + radius * new Vec2((float)Math.Cos(theta), (float)Math.Sin(theta));
                GL.Vertex2(v.X, v.Y);
                theta += kIncrement;
            }

            GL.End();

            DrawSegment(center, center + radius * axis, color);
        }

        public override void DrawSegment(Vec2 p1, Vec2 p2, Color color)
        {
            GL.Color4(color.R, color.G, color.B, 1);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(p1.X, p1.Y);
            GL.Vertex2(p2.X, p2.Y);
            GL.End();
        }

        public override void DrawXForm(XForm xf)
        {
            const float K_AXIS_SCALE = 0.4f;
            var p1 = xf.Position;

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex2(p1.X, p1.Y);

            var p2 = p1 + K_AXIS_SCALE * xf.R.Col1;

            GL.Vertex2(p2.X, p2.Y);
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex2(p1.X, p1.Y);

            p2 = p1 + K_AXIS_SCALE * xf.R.Col2;

            GL.Vertex2(p2.X, p2.Y);
            GL.End();
        }
    }
}