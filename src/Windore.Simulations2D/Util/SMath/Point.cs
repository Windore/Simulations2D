using System;

namespace Windore.Simulations2D.Util.SMath
{
    /// <summary>
    /// Represents a location in a two-dimensional plane.
    /// </summary>
    [Serializable]
    public struct Point
    {
        /// <summary>
        /// Gets or sets the x-coordinate of the point.
        /// </summary>
        public float X { get; private set; }
        /// <summary>
        /// Gets or sets the y-coordinate of the point.
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// Initializes a new Point instance with the set x and y coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate</param>
        /// <param name="y">The y-coordinate</param>
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the distance to a specified point.
        /// </summary>
        /// <param name="p">The point to calculate the distance to</param>
        /// <returns>The distance to a specified point</returns>
        public float DistanceTo(Point p)
        {
            return (float)Math.Sqrt(DistanceToSqr(p));
        }

        /// <summary>
        /// Calculates the squared distance to a specified point.
        /// </summary>
        /// <param name="p">The point to calculate the distance to</param>
        /// <returns>The distance to a specified point squared</returns>
        public double DistanceToSqr(Point p)
        {
            double a = Math.Abs(p.X - X);
            double b = Math.Abs(p.Y - Y);

            return Math.Pow(a, 2) + Math.Pow(b, 2);
        }

        /// <summary>
        /// Returns a string that represents the current Point.
        /// </summary>
        /// <returns>a string that represents the current Point.</returns>
        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }


        /// <summary>
        /// Determines whether the specified object is equal to the current Point.
        /// </summary>
        /// <param name="obj">The object to compare with the current Point.</param>
        /// <returns>true if the specified object is equal to the current Point; otherwise, false.</returns>    
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Point p = (Point)obj;
            return p.X == X && p.Y == Y;
        }

        /// <summary>
        /// Gets hash code for this Point.
        /// </summary>
        /// <returns>A hash code for the current Point.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
