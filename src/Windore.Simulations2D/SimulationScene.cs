using System;
using System.Collections.Generic;
using Windore.Simulations2D.Util.SMath;

namespace Windore.Simulations2D
{
    /// <summary>
    /// Represents a scene of a set size that contains SimulationObjects
    /// </summary>
    public class SimulationScene
    {
        private readonly object simulationObjectsLock = new object();
        private readonly object simulationContainerLock = new object();

        private readonly List<SimulationObject> simulationObjects;
        private readonly SimulationObjectsContainer container;

        /// <summary>
        /// Gets all SimulationObjects in the scene as a list
        /// </summary>
        public List<SimulationObject> SimulationObjects
        {
            get
            {
                lock (simulationObjectsLock)
                {
                    return new List<SimulationObject>(simulationObjects);
                }
            }
        }

        /// <summary>
        /// Gets the width of the SimulationScene
        /// </summary>
        public double Width { get; }

        /// <summary>
        /// Gets the height of the SimulationScene
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// Gets the amount of updates this SimulationScene has done
        /// </summary>
        public ulong Age { get; private set; } = 0;

        /// <summary>
        /// Initializes a new SimulationScene instance of the set width and height.
        /// </summary>
        /// <param name="width">The Width of the new SimulationScene</param>
        /// <param name="height">The Height of the new SimulationScene</param>
        /// <exception cref="ArgumentException">Width or height is set below zero</exception>
        public SimulationScene(double width, double height)
        {
            if (width < 0 || height < 0)
            {
                throw new ArgumentException("Width or height cannot be negative");
            }

            Width = width;
            Height = height;
            simulationObjects = new List<SimulationObject>();
            container = new SimulationObjectsContainer(width, height);
        }

        /// <summary>
        /// Calls Update on all SimulationObjects in the scene, 
        /// adds all new SimulationObjects to the scene and removes all SimulationObjects that were marked for removal
        /// </summary>
        public void Update()
        {
            Age++;

            // This lock is not very optimal but the container shouldn't be usually accessed from multiple threads.
            lock (simulationContainerLock)
            {
                container.Clear();
                foreach (SimulationObject obj in SimulationObjects)
                {
                    if (!obj.IsRemoved)
                    {
                        container.Add(obj);
                    }
                }
            }

            foreach (SimulationObject simulationObject in SimulationObjects)
            {
                if (!simulationObject.IsRemoved)
                {
                    simulationObject.Update();
                }
                else
                {
                    lock (simulationObjectsLock)
                        simulationObjects.Remove(simulationObject);
                }
            }
        }

        /// <summary>
        /// Adds a SimulationObject to the scene.
        /// </summary>
        /// <param name="simulationObject">SimulationObject to add to the scene</param>
        public void Add(SimulationObject simulationObject)
        {
            lock (simulationObjectsLock) 
            {
                simulationObjects.Add(simulationObject);
                simulationObject.Scene = this;
            }
        }

        /// <summary>
        /// Adds all SimulationObjects to the scene.
        /// </summary>
        /// <param name="simulationObjects">SimulationObjects to add to the scene</param>
        public void AddAll(params SimulationObject[] simulationObjects)
        {
            foreach (SimulationObject obj in simulationObjects)
            {
                Add(obj);
            }
        }

        /// <summary>
        /// Returns all SimulationObjects that are within the specified range from the specified point. Ignores the specified SimulationObject
        /// </summary>
        /// <param name="point">The specified point</param>
        /// <param name="range">The specified range</param>
        /// <param name="simulationObjectToIgnore">A SimulationObject not to be included in the returned list.</param>
        /// <returns>All SimulationObjects that are within the specified range from the specified point</returns>
        internal List<SimulationObject> GetSimulationObjectsInRange(Point point, double range, SimulationObject simulationObjectToIgnore = null)
        {
            lock (simulationContainerLock) 
                return container.GetInRange(point, range, simulationObjectToIgnore);
        }
    }
}
