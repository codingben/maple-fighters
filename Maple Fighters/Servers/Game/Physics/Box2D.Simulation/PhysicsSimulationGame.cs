using System;
using System.ComponentModel;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Box2DX.Dynamics;
using OpenTK.Graphics;
using OpenTK.Input;
using Color = System.Drawing.Color;

namespace Physics.Box2D.PhysicsSimulation
{
    public class PhysicsSimulationWindow : GameWindow
    {
        public World World { get; set; }

        private const string WINDOW_TITLE = "Physics Simulation";
        private const float MOUSE_ZOON_SPEED = 0.001f;
        private const float MOUSE_WHEEL_SENSITIVITY = 0.1f;

        private bool canMoveCameraView;
        private float moveSpeedViaKeyboard = 10;

        private readonly CameraView cameraView;

        public PhysicsSimulationWindow(int width, int height)
            : base(width, height, GraphicsMode.Default, WINDOW_TITLE, GameWindowFlags.FixedWindow)
        {
            cameraView = new CameraView(Vector2.Zero, MOUSE_ZOON_SPEED);

            SubscribeToInputEvents();
        }

        private void SubscribeToInputEvents()
        {
            MouseWheel += OnMouseWheelToZoom;
            MouseDown += OnLeftMouseDown;
            KeyDown += OnEscapeKeyDown;
        }

        private void UnsubscribeFromInputEvents()
        {
            MouseWheel -= OnMouseWheelToZoom;
            MouseDown -= OnLeftMouseDown;
            KeyDown -= OnEscapeKeyDown;
        }

        private void OnMouseWheelToZoom(object sender, MouseWheelEventArgs mouseWheelEventArgs)
        {
            var value = (float)mouseWheelEventArgs.Value / 1000;
            if (value < MOUSE_ZOON_SPEED)
            {
                value = MOUSE_ZOON_SPEED;
            }

            cameraView.Zoom = value;
        }

        private void OnLeftMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.Button != MouseButton.Left)
            {
                return;
            }

            canMoveCameraView = !canMoveCameraView;

            if (canMoveCameraView)
            {
                CursorVisible = false;
            }
        }

        private void OnEscapeKeyDown(object sender, KeyboardKeyEventArgs keyboardKeyEventArgs)
        {
            if (keyboardKeyEventArgs.Key != Key.Escape)
            {
                return;
            }

            if (canMoveCameraView)
            {
                CursorVisible = true;
                canMoveCameraView = false;
            }
        }

        private void SimulateWorld()
        {
            // Prepare for simulation. Typically we use a time step of 1/60 of a
            // second (60Hz) and 10 iterations. This provides a high quality simulation
            // in most game scenarios.
            const float TIME_STEP = 1.0f / 60.0f;
            const int VELOCITY_ITERATIONS = 8;
            const int POSITION_ITERATIONS = 3;

            // Instruct the world to perform a single step of simulation. It is
            // generally best to keep the time step and iterations fixed.
            World.Step(TIME_STEP, VELOCITY_ITERATIONS, POSITION_ITERATIONS);
        }

        private void MoveCameraViewViaMouse()
        {
            if (!canMoveCameraView)
            {
                return;
            }

            var position = new Vector2(OpenTK.Input.Mouse.GetState().X * MOUSE_WHEEL_SENSITIVITY, -OpenTK.Input.Mouse.GetState().Y * MOUSE_WHEEL_SENSITIVITY);
            cameraView.Position = position;
        }

        private void MoveCameraViewViaKeyboard()
        {
            if (canMoveCameraView)
            {
                return;
            }

            if (Keyboard.GetState().IsKeyDown(Key.Left))
            {
                cameraView.Position += new Vector2(-1, 0) * moveSpeedViaKeyboard;
            }

            if (Keyboard.GetState().IsKeyDown(Key.Right))
            {
                cameraView.Position += new Vector2(1, 0) * moveSpeedViaKeyboard;
            }

            if (Keyboard.GetState().IsKeyDown(Key.Up))
            {
                cameraView.Position += new Vector2(0, 1) * moveSpeedViaKeyboard;
            }

            if (Keyboard.GetState().IsKeyDown(Key.Down))
            {
                cameraView.Position += new Vector2(0, -1) * moveSpeedViaKeyboard;
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (Keyboard.GetState().IsKeyDown(Key.Plus))
            {
                moveSpeedViaKeyboard += 1;
            }

            if (Keyboard.GetState().IsKeyDown(Key.Minus))
            {
                if (Math.Abs(moveSpeedViaKeyboard) < 0)
                {
                    return;
                }

                moveSpeedViaKeyboard -= 1;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Title = $"{WINDOW_TITLE} - FPS: {RenderFrequency:0.0} - Move Speed: {moveSpeedViaKeyboard}";

            MoveCameraViewViaKeyboard();
            MoveCameraViewViaMouse();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            cameraView.Update();

            if (World != null)
            {
                SimulateWorld();
            }

            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            World?.Dispose();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            UnsubscribeFromInputEvents();
        }
    }
}