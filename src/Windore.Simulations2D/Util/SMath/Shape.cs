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
                // Testing if two ellipses overlap is rather complicated
                Shape first = new Shape(Position, Width, Height, IsEllipse);
                Shape second = new Shape(shape.Position, shape.Width, shape.Height, shape.IsEllipse);
                // First the first ellipse is placed to (0,0) and the second ellipse needs to be moved as well
                double xChange = -first.Position.X;
                double yChange = -first.Position.Y;

                first = first.Move(new Point(0, 0));
                second = second.Move(new Point(second.Position.X + xChange, second.Position.Y + yChange));

                // Scale the second ellipse to a circle.
                double relation = second.Width / second.Height;
                second = new Shape(new Point(second.Position.X, second.Position.Y * relation), second.Width, second.Height * relation, true);
                // The first ellipse doesn't change it's location but it's height does change
                first = new Shape(first.Position, first.Width, first.Height * relation, true);

                // The parameterized form for the first ellipse is used to discover the "radius" towards the second ellipse
                double alpha;
                if (second.Position.X == 0d)
                {
                    alpha = Math.PI / 2d;
                }
                else
                {
                    alpha = Math.Atan(second.Position.Y / second.Position.X);
                }

                double s = Math.Sqrt(Math.Pow(first.Width / 2d * Math.Cos(alpha), 2) + Math.Pow(first.Height / 2d * Math.Sin(alpha), 2d));

                // The radius of the second ellipse which now is a circle
                double r = second.Width / 2d;

                // Distance between the first and second ellipse center points:
                double d = first.Position.DistanceTo(second.Position);

                return s + r > d;
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

                // Scale the ellipse to a circle. Figuring out if a rectangle overlaps a circle is easy. The rectangle is scaled as well. 
                double xyRelation = ellipse.Width / ellipse.Height;
                return SMath.CircleOverlapsRectangle(new Point(ellipse.Position.X, ellipse.Position.Y * xyRelation), ellipse.Width, new Point(rectangle.Position.X, rectangle.Position.Y * xyRelation), rectangle.Width, rectangle.Height * xyRelation);
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
