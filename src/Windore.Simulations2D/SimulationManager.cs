using System.Threading;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Windore.Simulations2D
{
    /// <summary>
    /// Provides a class for controlling a SimulationScene and managing UI
    /// </summary>
    public class SimulationManager
    {
        private Thread simulationThread;
        private volatile bool simulationRunning = false;
        private volatile int maxUps = 0;
        private Queue<double> upsQueue = new Queue<double>();

        /// <summary>
        /// Gets or sets the maximum amount of updates per second. Zero or less indicated no limit.
        /// </summary>
        public int MaxUps { get => maxUps; set => maxUps = value; }

        /// <summary>
        /// Gets the current average updates per second value
        /// </summary>
        public double UPS { get; private set; }

        /// <summary>
        /// Gets the current managed SimulationScene
        /// </summary>
        public SimulationScene SimulationScene { get; private set; }

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
        /// Occurs every time before calling update
        /// </summary>
        public virtual void BeforeUpdate() {}

        private void UpdateLoop()
        {            
            Stopwatch stopwatch = new Stopwatch();

            while (SimulationRunning)
            {
                stopwatch.Start();

                int currentMaxUps = MaxUps;
                bool isMaxUpsSet = currentMaxUps > 0;

                BeforeUpdate();
                SimulationScene.Update();
                AfterUpdate();

                // This is used to reduce the speed of the simulation.
                if (isMaxUpsSet) 
                {
                    long updateLengthInMillis = (long)Math.Round(1d / currentMaxUps * 1000d);
                    long timeLeft = updateLengthInMillis - stopwatch.ElapsedMilliseconds;
                    if (timeLeft > 0)
                        Thread.Sleep((int)timeLeft);
                }

                double currentUps = 1d / stopwatch.Elapsed.TotalSeconds;

                if (upsQueue.Count >= 50) 
                {
                    upsQueue.Dequeue();
                }

                upsQueue.Enqueue(currentUps);

                UPS = upsQueue.Average();

                stopwatch.Reset();
            }
        }

        /// <summary>
        /// Occurs every time after calling update
        /// </summary>
        public virtual void AfterUpdate() {}
    }
}
