using SFML.Graphics;
using SFML.Window;
using System.Threading;

namespace Windore.Simulations2D.UI
{
    internal class SimulationWindow
    {
        private RenderWindow window;
        private readonly SimulationManager manager;
        private volatile bool shouldClose = false;

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
            xScale = width / (float)manager.SimulationScene.Width;
            yScale = height / (float)manager.SimulationScene.Height;
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

            // Yes, this is required, or I atleast think it is
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
                    // This should be fixed later. Currently at idle the cpu is being used which is not good. 
                    // Also there is a small latency when selecting objects because of the Thread.Sleep()

                    Thread.Sleep(100);
                    window.DispatchEvents();

                    window.Clear(Color.White);
                    DrawSimulation();
                    window.Display();

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
            bool isSelectedSet = false;

            foreach(SimulationObject obj in manager.SimulationScene.SimulationObjects)
            {
                CircleShape shape = obj.Shape switch
                {
                    Shape.Circle => new CircleShape((float)obj.Size * sizeScale),
                    Shape.Square => new CircleShape((float)obj.Size * sizeScale, 4),
                    Shape.Triangle => new CircleShape((float)obj.Size * sizeScale, 3),
                    _ => new CircleShape((float)obj.Size * sizeScale),
                };

                shape.Position = new SFML.System.Vector2f(xScale * (float)obj.Position.X, yScale * (float)obj.Position.Y);
                shape.FillColor = obj.Color;

                if (Mouse.IsButtonPressed(Mouse.Button.Left) && obj != selectedObject)
                {
                    SFML.System.Vector2f pos = window.MapPixelToCoords(Mouse.GetPosition(window));
                    if (shape.GetGlobalBounds().Contains(pos.X, pos.Y) && !isSelectedSet)
                    {
                        selectedObject = obj;
                        obj.OnSelect();
                        isSelectedSet = true;
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
