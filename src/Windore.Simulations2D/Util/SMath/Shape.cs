using System;

namespace Windore.Simulations2D.Util.SMath
{
    /// <summary>
    /// Represents a simple shape in a 2-dimensional plane. A shape can be either a rectangle or an ellipse.
    /// </summary>
    public struct Shape
    {
        /// <summary>
        /// Gets the current position of the shape
        /// </summary>
        public Point Position { get; }
        /// <summary>
        /// Gets if the current shape is an ellipse or a rectangle
        /// </summary>
        public bool IsEllipse { get; }
        /// <summary>
        /// Gets the width of the shape.
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// Gets the height of the shape
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// Initializes a new shape with a width and a height.
        /// </summary>
        /// <param name="position">The position of the shape</param>
        /// <param name="width">The width of the shape</param>
        /// <param name="height">The height of the shape</param>
        /// <param name="isEllipse">Is the shape an ellipse or a rectangle.</param>
        public Shape(Point position, double width, double height, bool isEllipse) 
        {
            Position = position;
            Width = width;
            Height = height;
            IsEllipse = isEllipse;
        }

        /// <summary>
        /// Returns true if the current and the given shape overlap.
        /// </summary>
        /// <param name="shape">A given shape to check for overlapping.</param>
        /// <returns>True if the current and the given shape overlap.</returns>
        public bool Overlaps(Shape shape) 
        {
            double avrg = (Width + Height) / 2d;
            return Position.DistanceToSqr(shape.Position) < avrg*avrg;
        }

        /// <summary>
        /// Returns the same shape, but with a new position.
        /// </summary>
        /// <param name="newPosition">The new position of the shape.</param>
        /// <returns>The same shape, but with a new position.</returns>
        public Shape Move(Point newPosition) 
        {
            return new Shape(newPosition, Width, Height, IsEllipse);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current shape.
        /// </summary>
        /// <param name="obj">The object to compare with the current shape.</param>
        /// <returns>true if the specified object is equal to the current shape; otherwise, false.</returns>    
        public override bool Equals(object obj)
        {
            return obj is Shape shape &&
                   IsEllipse == shape.IsEllipse &&
                   Width == shape.Width &&
                   Height == shape.Height;
        }

        /// <summary>
        /// Gets hash code for this shape.
        /// </summary>
        /// <returns>A hash code for the current shape.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(IsEllipse, Width, Height);
        }
    }
}
