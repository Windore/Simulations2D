using System.Collections.Generic;
using Windore.Simulations2D.UI;
using Windore.Simulations2D.Util.SMath;

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
        internal bool IsRemoved { get; set; } = false;

        private Point position;
        private float size;

        /// <summary>
        /// Gets the current scene the SimulationObject is in.
        /// </summary>
        public SimulationScene Scene { get; internal set; }

        /// <summary>
        /// Gets or sets the current visual Shape of the SimulationObject
        /// </summary>
        public Shape Shape { get; set; }

        /// <summary>
        /// Gets or sets the current position of the SimulationObject. 
        /// <br></br>
        /// If the SimulationObject is in a SimulationScene the position will be clamped to fit the scene
        /// </summary>
        public Point Position
        {
            get => position;
            set
            {
                position = value;

                ClampPosition();
            }
        }

        /// <summary>
        /// Gets or sets the current Color of the SimulationObject
        /// </summary>
        public SFML.Graphics.Color Color { get; set; }
        /// <summary>
        /// Gets or sets the size of the SimulationObject. Affects calculations and visuals
        /// <para>For calculations the size expresses the diameter of the circle.</para>
        /// <para>For visuals the size may express different things: </para>
        /// <list type="bullet">
        ///     <item>Circle: diameter length</item>
        ///     <item>Square: side length</item>
        ///     <item>Triangle: side length</item>
        /// </list>
        /// </summary>
        public float Size
        {
            get => size;
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value) || double.IsNegative(value))
                    return;

                size = value;
            }
        }

        /// <summary>
        /// Initializes a new SimulationObject instance with the set position, shape, color and size.
        /// </summary>
        protected SimulationObject(Point position, Shape shape, SFML.Graphics.Color color, float size)
        {
            Position = position;
            Shape = shape;
            Color = color;
            Size = size;
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
            // If either or both SimulationObjects are removed they can't overlap
            if (this.IsRemoved || simulationObject.IsRemoved) return false;

            double combinedRadius = Size / 2D + simulationObject.Size / 2D;
            return Position.DistanceTo(simulationObject.Position) < combinedRadius;
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
        public void MoveTowards(Point target, float distance)
        {
            float magnitude = Position.DistanceTo(target);
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
        /// Clamps the SimulationObject's position to fit in the current SimulationScene
        /// </summary>
        internal void ClampPosition()
        {
            if (Scene != null)
            {
                position = new Point((float)SMath.Clamp(position.X, 0, Scene.Width), (float)SMath.Clamp(position.Y, 0, Scene.Height));
            }
        }

        /// <summary>
        /// Occurs every time this SimulationObject is selected
        /// </summary>
        protected internal virtual void OnSelect() { }
    }
}
