using System;
using Windore.Simulations2D.Util.SMath;

namespace Windore.Simulations2D.Util
{
    /// <summary>
    /// Provides methods for pseudo-random value generation and helps to track and set the seed of the generator.
    /// </summary>
    public class SRandom
    {
        private Random random;
        private int seed;

        /// <summary>
        /// Get or set the current seed of the pseudo-random number generator.
        /// </summary>
        public int Seed 
        {
            get => seed;    
            set 
            {
                random = new Random(value);
                seed = value;
            }
        }

        /// <summary>
        /// Initializes a new SRandom instance with a given seed
        /// </summary>
        /// <param name="seed">A seed for the generator</param>
        public SRandom(int seed)
        {
            Seed = seed;
            random = new Random(Seed);
        }

        /// <summary>
        /// Initializes a new SRandom instance with a random seed
        /// </summary>
        public SRandom()
        {
            // At first this seems unneccessary, but It's required to keep track of the seed
            Random generator = new Random();
            Seed = generator.Next();
            random = new Random(Seed);
        }

        /// <summary>
        /// Returns a random integer between the min and the max value.
        /// </summary>
        /// <returns>A random integer between the min and the max value</returns>
        public int Integer(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Returns a random double between the min and the max value.
        /// </summary>
        /// <returns>A random double between the min and the max value</returns>
        public double Double(double min, double max) 
        {
            return random.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Returns true with the given probability, otherwise returns false.
        /// </summary>
        /// <param name="percentage">Probability of returning true</param>
        /// <returns>True with the given probability, otherwise returns false.</returns>
        public bool Boolean(Percentage percentage)
        {
            return random.NextDouble() <= percentage;
        }

        /// <summary>
        /// Returns a random positive point with a max X and Y value.
        /// </summary>
        /// <param name="maxX">Max X value</param>
        /// <param name="maxY">Max Y value</param>
        /// <returns>A random point with a max X and Y value.</returns>
        public Point Point(double maxX, double maxY) 
        {
            return Point(maxX, maxY, 0, 0);
        }

        /// <summary>
        /// Returns a random positive point with a max X and Y value.
        /// </summary>
        /// <param name="maxX">Max X value</param>
        /// <param name="maxY">Max Y value</param>
        /// <param name="minX">Min X value</param>
        /// <param name="minY">Min Y value</param>
        /// <returns>A random point with a max X and Y value.</returns>
        public Point Point(double maxX, double maxY, double minX, double minY)
        {
            double x = Double(minX, maxX);
            double y = Double(minY, maxY);

            return new Point(x, y);
        }
    }
}
