using System.Collections.Generic;
using Windore.Simulations2D.Util.SMath;
using Windore.Simulations2D.Util;

namespace Windore.Simulations2D
{
    /// <summary>
    /// Represents an object in a SimulationScene
    /// </summary>
    public abstract class SimulationObject
    {
        /// <summary>
        /// Indicates wheter this SimulationObject is removed or not
        /// </summary>
        public bool IsRemoved { get; internal set; } = false;

        /// <summary>
        /// Gets the current scene the SimulationObject is in.
        /// </summary>
        public SimulationScene Scene { get; internal set; }

        /// <summary>
        /// Gets or sets the current shape of the SimulationObject
        /// </summary>
        public Shape Shape { get; set; }

        /// <summary>
        /// Gets or sets the current position of the SimulationObject. 
        /// <br></br>
        /// If the SimulationObject is in a SimulationScene the position will be clamped to fit the scene
        /// </summary>
        public Point Position
        {
            get => Shape.Position;
            set
            {
                Point newPos = value;
                if (Scene != null)
                {
                    newPos = new Point(SMath.Clamp(value.X, 0, Scene.Width), SMath.Clamp(value.Y, 0, Scene.Height));
                }

                Shape = Shape.Move(newPos);
            }
        }

        /// <summary>
        /// Gets or sets the current Color of the SimulationObject
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Initializes a new SimulationObject instance with the set position, shape, color and size.
        /// </summary>
        protected SimulationObject(Shape shape, Color color)
        {
            Shape = shape;
            Color = color;
        }

        /// <summary>
        /// Update occurs every time SimulationScene is updated
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Returns true when the current SimulationObject is overlapping with a specified SimulationObject.
        /// </summary>
        /// <param name="simulationObject">A SimulationObject to test for overlapping</param>
        /// <returns>True when the current SimulationObject is overlapping with a specified SimulationObject</returns>
        public bool OverlappingWith(SimulationObject simulationObject)
        {
            return Shape.Overlaps(simulationObject.Shape);
        }

        /// <summary>
        /// Returns all nearby SimulationObjects that are in the specified range.
        /// </summary>
        /// <param name="range">The range to check for SimulationObjects</param>
        /// <returns>A list of SimulationObjects in the specified range</returns>
        public List<SimulationObject> GetSimulationObjectsInRange(double range)
        {
            if (Scene != null)
            {
                return Scene.GetSimulationObjectsInRange(Position, range, this);
            }
            return new List<SimulationObject>();
        }

        /// <summary>
        /// Marks this SimulationObject for removal
        /// </summary>
        public void Remove()
        {
            IsRemoved = true;
        }

        /// <summary>
        /// Moves the SimulationObject towards the target, moving approximately the maximum of distance units.
        /// <br></br>
        /// If the target is reached the SimulationObject will not move past it.
        /// </summary>
        /// <param name="target">Target position</param>
        /// <param name="distance">Maximum distance to move</param>
        public void MoveTowards(Point target, double distance)
        {
            double magnitude = Position.DistanceTo(target);
            if (magnitude <= distance || magnitude == 0f)
            {
                Position = new Point(target.X, target.Y);
            }
            else
            {
                Point a = new Point(target.X - Position.X, target.Y - Position.Y);
                Position = new Point(Position.X + a.X / magnitude * distance, Position.Y + a.Y / magnitude * distance);
            }
        }

        /// <summary>
        /// Occurs every time this SimulationObject is selected
        /// </summary>
        protected internal virtual void OnSelect() { }
    }
}
