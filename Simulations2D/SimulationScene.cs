using Simulations2D.Util.SMath;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Simulations2D
{
    /// <summary>
    /// Represents a scene of a set size that contains SimulationObjects
    /// </summary>
    public class SimulationScene
    {
        private readonly object simulationObjectsLock = new object();

        private readonly List<SimulationObject> simulationObjects;
        private ReadOnlyCollection<SimulationObject> simulationObjectsCopy;
        private readonly SimulationObjectsContainer container;

        /// <summary>
        /// Gets all SimulationObjects in the scene as a list
        /// </summary>
        public List<SimulationObject> SimulationObjects
        {
            get
            {
                lock(simulationObjectsLock)
                {
                    List<SimulationObject> objs = new List<SimulationObject>();
                    objs.AddRange(simulationObjectsCopy);
                    return objs;
                }
            }
        }

        /// <summary>
        /// Gets the width of the SimulationScene
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// Gets the height of the SimulationScene
        /// </summary>
        public float Height { get; }

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
        public SimulationScene(float width, float height)
        {
            if (width < 0 || height < 0)
            {
                throw new ArgumentException("Width or height cannot be negative");
            }

            Width = width;
            Height = height;
            simulationObjects = new List<SimulationObject>();
            simulationObjectsCopy = new List<SimulationObject>().AsReadOnly();
            container = new SimulationObjectsContainer(width, height);
        }

        /// <summary>
        /// Calls Update on all SimulationObjects in the scene, 
        /// adds all new SimulationObjects to the scene and removes all SimulationObjects that were marked for removal
        /// </summary>
        public void Update()
        {
            Age++;
            /*
             * Before each update SimulationObjects are copied into a list.
             * This List is used to call updates on SimulationObjects
             */
            CopySimulationObjects();

            container.Clear();
            foreach (SimulationObject obj in simulationObjectsCopy)
            {
                if (!obj.IsRemoved)
                {
                    container.Add(obj);
                }
            }

            foreach (SimulationObject simulationObject in simulationObjectsCopy)
            {
                // IsRemoved is checked here even though SimulationObjects that are removed should not be found here just in case
                // a SimulationObject which is removed is readded for some reason
                if (!simulationObject.IsRemoved) 
                { 
                    simulationObject.Update(); 
                }
                else
                {
                    simulationObjects.Remove(simulationObject);
                }
            }

            // This is re-done here so all SimulationObject removals are applied
            CopySimulationObjects();
        }

        private void CopySimulationObjects()
        {
            lock (simulationObjectsLock)
            {
                List<SimulationObject> objects = new List<SimulationObject>();
                objects.AddRange(simulationObjects);
                simulationObjectsCopy = objects.AsReadOnly();
            }
        }

        // We don't want to expose the list containing all SimulationObjects so adding is done through these methods

        /// <summary>
        /// Adds a SimulationObject to the scene.
        /// </summary>
        /// <param name="simulationObject">SimulationObject to add to the scene</param>
        public void Add(SimulationObject simulationObject)
        {
            simulationObjects.Add(simulationObject);
            simulationObject.Scene = this;
        }

        /// <summary>
        /// Adds all SimulationObjects to the scene.
        /// </summary>
        /// <param name="simulationObjects">SimulationObjects to add to the scene</param>
        public void AddAll(params SimulationObject[] simulationObjects)
        {
            foreach(SimulationObject obj in simulationObjects)
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
            return container.GetInRange(point, range, simulationObjectToIgnore);
        }
    }
}
