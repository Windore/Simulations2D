using System;
using System.Collections.Generic;
using System.Linq;

namespace Windore.Simulations2D.Util.SMath
{
    /// <summary>
    /// Collection of useful methods that are not found in the System.Math library
    /// </summary>
    public static class SMath
    {
        /// <summary>
        /// Clamps the number between the min and the max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>The clamped value</returns>
        public static double Clamp(double value, double min, double max)
        {
            if (value < min) { value = min; }
            if (value > max) { value = max; }
            return value;
        }

        /// <summary>
        /// Calculates the standard deviation of the given numbers
        /// </summary>
        /// <param name="numbers">The numbers of which to calculate the standard deviation</param>
        /// <returns>The standard deviation of the given numbers</returns>
        public static double StandardDeviation(params double[] numbers)
        {
            double avg = numbers.Average();
            double sum = 0;
            foreach (double num in numbers)
            {
                sum += (num - avg) * (num - avg);
            }
            return Math.Sqrt(sum / numbers.Length);
        }

        /// <summary>
        /// Calculates the standard deviation of the numbers in the collection
        /// </summary>
        /// <param name="numbers">The numbers of which to calculate the standard deviation</param>
        /// <returns>The standard deviation of the numbers in the collection</returns>
        public static double StandardDeviation(this ICollection<double> numbers)
        {
            return StandardDeviation(numbers.ToArray());
        }

        /// <summary>
        /// Determines if a circle overlaps with a rectangle
        /// <para>Note: the rectangle must be aligned with X and Y axis</para>
        /// </summary>
        /// <param name="circleCenter">Center point of the circle</param>
        /// <param name="circleRadius">Radius of the circle</param>
        /// <param name="rectangleCenter">Center point of the rectangle</param>
        /// <param name="rectangleWidth">Width of the rectangle</param>
        /// <param name="rectangleHeight">Height of the rectangle</param>
        /// <returns>True if the shapes overlap</returns>
        public static bool CircleOverlapsRectangle(Point circleCenter, double circleRadius, Point rectangleCenter, double rectangleWidth, double rectangleHeight)
        {
            double distToCircleX = Math.Abs(circleCenter.X - rectangleCenter.X);
            double distToCircleY = Math.Abs(circleCenter.Y - rectangleCenter.Y);

            if (distToCircleX > rectangleWidth / 2D + circleRadius)
                return false;
            if (distToCircleY > rectangleHeight / 2D + circleRadius)
                return false;

            if (distToCircleX <= rectangleWidth / 2D)
                return true;
            if (distToCircleY <= rectangleHeight / 2D)
                return true;

            double cornerDistSq = Math.Pow(distToCircleX - rectangleWidth / 2D, 2)
                + Math.Pow(distToCircleY - rectangleHeight / 2D, 2);

            return cornerDistSq <= circleRadius * circleRadius;
        }
    }
}
