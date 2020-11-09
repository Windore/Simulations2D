using Simulations2D.Util.SMath;
using System.Collections.Generic;

namespace Simulations2D
{
    // This class is used to optimize the GetSimulationObjectsInRange method.
    // However currently this class most likely isn't very well optimized, but it works.
    internal class SimulationObjectsContainer
    {
        private readonly List<SimulationObject> simulationObjects;
        private readonly List<SimulationObjectsContainer> containers;

        private readonly Point centerPoint;
        private readonly float width;
        private readonly float height;
        private readonly bool holdsContainers;

        internal SimulationObjectsContainer(float width, float height) : this(new Point(width / 2f, height / 2f), width, height, 4) { }

        private SimulationObjectsContainer(Point centerPoint, float width, float height, int depth)
        {
            this.centerPoint = centerPoint;
            this.width = width;
            this.height = height;
            holdsContainers = depth > 1;
            simulationObjects = new List<SimulationObject>();
            containers = new List<SimulationObjectsContainer>();


            if (holdsContainers)
            {
                //Divide the container to 16 (4 * 4) inner containers

                float innerContainerWidth = width / 4f;
                float innerContainerHeight = height / 4f;

                float currentY = centerPoint.Y - height / 2f + innerContainerHeight / 2f;
                for (int i = 0; i < 4; i++)
                {
                    float currentX = centerPoint.X - width / 2f + innerContainerWidth / 2f;
                    for (int j = 0; j < 4; j++)
                    {
                        containers.Add(new SimulationObjectsContainer(new Point(currentX, currentY), innerContainerWidth, innerContainerHeight, depth - 1));
                        currentX += innerContainerWidth;
                    }
                    currentY += innerContainerHeight;
                }
            }
        }

        /// <summary>
        /// Returns all SimulationObjects in this Container that are in range from the specified point
        /// </summary>
        internal List<SimulationObject> GetInRange(Point from, double range, SimulationObject ignored)
        {
            //Test if this container is in range
            if (SMath.CircleOverlapsRectangle(from, range, centerPoint, width, height))
            {
                List<SimulationObject> simulationObjectsInRange = new List<SimulationObject>();
                //If this container holds containers go through the inner containers
                if (holdsContainers)
                {
                    foreach (SimulationObjectsContainer c in containers)
                    {
                        simulationObjectsInRange.AddRange(c.GetInRange(from, range, ignored));
                    }
                }
                else //If the container holds simulation objects go through them testing if they are in range
                {
                    foreach (SimulationObject o in simulationObjects)
                    {
                        if (o.Position.DistanceToSqr(from) <= range * range && o != ignored && !o.IsRemoved)
                        {
                            simulationObjectsInRange.Add(o);
                        }
                    }
                }
                return simulationObjectsInRange;
            }
            else
            {
                return new List<SimulationObject>();
            }
        }

        internal void Clear()
        {
            if (holdsContainers)
            {
                foreach (SimulationObjectsContainer c in containers)
                {
                    c.Clear();
                }
            }
            else
            {
                simulationObjects.Clear();
            }
        }

        internal void Add(SimulationObject simulationObject)
        {
            if (holdsContainers)
            {
                foreach (SimulationObjectsContainer c in containers)
                {
                    //If the SimulationObject's position is in the container add the object to that container
                    bool xInContainer = c.centerPoint.X - (c.width / 2D) <= simulationObject.Position.X
                        && simulationObject.Position.X <= c.centerPoint.X + (c.width / 2D);

                    bool yInContainer = c.centerPoint.Y - (c.height / 2D) <= simulationObject.Position.Y
                        && simulationObject.Position.Y <= c.centerPoint.Y + (c.height / 2D);
                    if (xInContainer && yInContainer)
                    {
                        c.Add(simulationObject);
                        break;
                    }
                }
            }
            else
            {
                simulationObjects.Add(simulationObject);
            }
        }
    }
}
