using Windore.Simulations2D.UI;
using System.Threading;
using System;
using System.Diagnostics;

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
        private int maxUps = 0;

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
        /// <param name="maxUps">Maximum number of updates per second. Value of 0 or negative indicates that no maximum is set.</param>
        public void StartSimulation(int maxUps=0)
        {
            if (SimulationRunning) return;

            this.maxUps = maxUps;
            simulationThread = new Thread(new ThreadStart(UpdateLoop));
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

        private void UpdateLoop()
        {
            bool isMaxUpsSet = maxUps > 0;
            long updateLengthInMillis = 0;
            Stopwatch stopwatch = new Stopwatch();

            if (isMaxUpsSet) 
            {
                updateLengthInMillis = (long)Math.Round(1d / maxUps * 1000);
                stopwatch.Start();
            }

            while (SimulationRunning)
            {
                BeforeUpdate();
                SimulationScene.Update();
                AfterUpdate();

                // This is used to reduce the speed of the simulation.
                if (isMaxUpsSet) 
                {
                    stopwatch.Stop();
                    long timeLeft = updateLengthInMillis - stopwatch.ElapsedMilliseconds;

                    if (timeLeft > 0)
                        Thread.Sleep((int)timeLeft);

                    stopwatch.Reset();
                    stopwatch.Start();
                }
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
