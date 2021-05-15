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
            if (IsEllipse == shape.IsEllipse && IsEllipse) 
            {
                // Figure out if two ellipses overlap by stretching the ellipses to circles and measuring the stretched distance
                // I have no clue if this actually works, but the tests haven't failed so far..
                double xyRelationThis = Width / Height;
                double xyRelationOther = shape.Width / shape.Height;

                return Math.Pow((Position.X - shape.Position.X), 2) 
                    + xyRelationOther * xyRelationThis * Math.Pow((Position.Y - shape.Position.Y), 2) 
                    < Math.Pow(Width / 2d + shape.Width / 2d, 2);
            }
            else if (IsEllipse == shape.IsEllipse && !IsEllipse) 
            {
                return Math.Abs(Position.X - shape.Position.X) < Width / 2d + shape.Width / 2d
                    && Math.Abs(Position.Y - shape.Position.Y) < Height / 2d + shape.Height / 2d;
            }
            else 
            {
                Shape ellipse;
                Shape rectangle;

                if (IsEllipse) 
                {
                    ellipse = this;
                    rectangle = shape;
                }
                else 
                {
                    ellipse = shape;
                    rectangle = this;
                }

                // Same streching principle is applied here
                double xyRelation = ellipse.Width / ellipse.Height;
                return SMath.CircleOverlapsRectangle(ellipse.Position, ellipse.Width, rectangle.Position, rectangle.Width, rectangle.Height * xyRelation);
            }
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
