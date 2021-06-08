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
        public double X { get; }
        /// <summary>
        /// Gets or sets the y-coordinate of the point.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Initializes a new Point instance with the set x and y coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate</param>
        /// <param name="y">The y-coordinate</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the distance to a specified point.
        /// </summary>
        /// <param name="p">The point to calculate the distance to</param>
        /// <returns>The distance to a specified point</returns>
        public double DistanceTo(Point p)
        {
            return Math.Sqrt(DistanceToSqr(p));
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
            return string.Format("({0}, {1})", X, Y);
        }

        /// <summary>
        /// Returns a string that represents the current Point rounded to three decimal digits.
        /// </summary>
        /// <returns>a string that represents the current Point rounded to three decimal digits.</returns>
        public string ToRoundedString() 
        {
            return string.Format("({0}, {1})", Math.Round(X,3), Math.Round(Y,3));
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
