using System.Threading;
using System;
using System.Diagnostics;

namespace Windore.Simulations2D
{
    /// <summary>
    /// Provides a class for controlling a SimulationScene and managing UI
    /// </summary>
    public class SimulationManager
    {
        private readonly ISimulationUI ui;
        private Thread simulationThread;
        private volatile bool simulationRunning = false;
        private int maxUps = 0;

        /// <summary>
        /// Gets the current managed SimulationScene
        /// </summary>
        public SimulationScene SimulationScene { get; private set; }

        /// <summary>
        /// Gets the current simulation UI
        /// </summary>
        protected ISimulationUI UI { get => ui; }

        /// <summary>
        /// Gets whether SimulationScene is updated
        /// </summary>
        public bool SimulationRunning { get => simulationRunning; private set => simulationRunning = value; }

        /// <summary>
        /// Initializes a new SimulationManager instance
        /// </summary>
        /// <param name="scene">A SimulationScene to manage</param>
        /// <param name="ui">An UI to update with the simulation</param>
        public SimulationManager(SimulationScene scene, ISimulationUI ui)
        {
            SimulationScene = scene;
            this.ui = ui;
            ui.SetSimulationManager(this);
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
            ui.Start();
        }

        /// <summary>
        /// Stops the simulation
        /// </summary>
        public void StopSimulation()
        {
            if (!SimulationRunning) return;

            SimulationRunning = false;
            ui.Stop();
        }

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
    }
}
