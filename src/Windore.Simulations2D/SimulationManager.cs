using Windore.Simulations2D.UI;
using System.Threading;

namespace Windore.Simulations2D
{
    /// <summary>
    /// Provides a class for controlling SimulationScenes and creating UI
    /// </summary>
    public class SimulationManager
    {
        private SimulationWindow window;
        private Thread simulationThread;
        private volatile bool uiRunning = false;
        private volatile bool simulationRunning = false;

        /// <summary>
        /// Gets the current managed SimulationScene
        /// </summary>
        public SimulationScene SimulationScene { get; private set; }

        /// <summary>
        /// Gets or sets whether the SimulationWindow is open
        /// </summary>
        public bool UIRunning { get => uiRunning; internal set => uiRunning = value; }

        /// <summary>
        /// Gets whether SimulationScene is updated
        /// </summary>
        public bool SimulationRunning { get => simulationRunning; private set => simulationRunning = value; }

        /// <summary>
        /// Initializes a new SimulationManager instance
        /// </summary>
        /// <param name="scene">A SimulationScene to manage</param>
        public SimulationManager(SimulationScene scene)
        {
            SimulationScene = scene;
        }

        /// <summary>
        /// Starts the simulation
        /// </summary>
        public void StartSimulation()
        {
            if (SimulationRunning) return;

            simulationThread = new Thread(new ThreadStart(Update));
            simulationThread.Start();
            SimulationRunning = true;
        }

        /// <summary>
        /// Stops the simulation
        /// </summary>
        public void StopSimulation()
        {
            if (!SimulationRunning) return;

            SimulationRunning = false;
        }

        /// <summary>
        /// Occurs when SimulationWindow is closed
        /// </summary>
        protected internal virtual void OnSimulationWindowClose() {}

        /// <summary>
        /// Occurs every time before calling update
        /// </summary>
        protected virtual void BeforeUpdate() {}

        private void Update()
        {
            while (SimulationRunning)
            {
                BeforeUpdate();
                SimulationScene.Update();
                AfterUpdate();
            }
        }

        /// <summary>
        /// Occurs every time after calling update
        /// </summary>
        protected virtual void AfterUpdate() {}

        /// <summary>
        /// Opens a SimulationWindow
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">The height of the window</param>
        /// <param name="title">The title of the window</param>
        public void OpenSimulationWindow(uint width, uint height, string title)
        {
            if (UIRunning)
                return;

            UIRunning = true;
            window = new SimulationWindow(width, height, this, title);
        }

        /// <summary>
        /// Closes the active SimulationWindow
        /// </summary>
        public void CloseSimulationWindow()
        {
            if (!UIRunning)
                return;

            UIRunning = false;
            window.Close();
        }
    }
}
