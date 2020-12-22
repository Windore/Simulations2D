using SFML.Graphics;
using SFML.Window;
using System.Threading;

namespace Windore.Simulations2D.UI
{
    internal class SimulationWindow
    {
        private RenderWindow window;
        private readonly SimulationManager manager;
        private bool shouldClose = false;

        private SimulationObject selectedObject;

        private readonly float xScale;
        private readonly float yScale;
        private readonly float sizeScale;

        internal SimulationWindow(uint width, uint height, SimulationManager manager, string title)
        {
            this.manager = manager;

            /*
             * If the window is larger / smaller than the simulation scene then the sizes and positions of SimulationObjects
             * need to be adjusted.
             */
            xScale = width / manager.SimulationScene.Width;
            yScale = height / manager.SimulationScene.Height;
            sizeScale = (xScale + yScale) / 2f;

            Thread windowThread = new Thread(new ThreadStart(() => Open(width, height, title)));
            windowThread.Start();
        }

        internal void Close()
        {
            shouldClose = true;
        }

        private void Open(uint width, uint height, string title)
        {
            window = new RenderWindow(new VideoMode(width, height), title, Styles.Close);
            window.Closed += (sender, e) => window.Close();
            MainLoop();
        }

        private void MainLoop()
        {
            while(window.IsOpen)
            {
                if (manager.SimulationRunning)
                {
                    window.DispatchEvents();
                    window.Clear(Color.White);

                    DrawSimulation();

                    window.Display();
                }
                else
                {
                    Thread.Sleep(100);
                    window.DispatchEvents();
                }

                if (shouldClose)
                {
                    window.Close();
                }
            }

            manager.OnSimulationWindowClose();
            manager.UIRunning = false;
            window.Dispose();
        }

        private void DrawSimulation()
        {
            foreach(SimulationObject obj in manager.SimulationScene.SimulationObjects)
            {
                CircleShape shape = obj.Shape switch
                {
                    Shape.Circle => new CircleShape(obj.Size * sizeScale),
                    Shape.Square => new CircleShape(obj.Size * sizeScale, 4),
                    Shape.Triangle => new CircleShape(obj.Size * sizeScale, 3),
                    _ => new CircleShape(obj.Size * sizeScale),
                };

                shape.Position = new SFML.System.Vector2f(xScale * obj.Position.X, yScale * obj.Position.Y);
                shape.FillColor = obj.Color;

                if (Mouse.IsButtonPressed(Mouse.Button.Left) && obj != selectedObject)
                {
                    SFML.System.Vector2f pos = window.MapPixelToCoords(Mouse.GetPosition(window));
                    if (shape.GetGlobalBounds().Contains(pos.X, pos.Y))
                    {
                        selectedObject = obj;
                        obj.OnSelect();
                    }
                }

                if (obj == selectedObject)
                {
                    shape.OutlineThickness = 3;
                    shape.OutlineColor = Color.Black;
                }

                window.Draw(shape);
            }
        }
    }
}
