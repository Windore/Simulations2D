using System;
using Simulations2D.Util.SMath;

namespace Simulations2D.Util
{
    /// <summary>
    /// Provides methods for pseudo-random value generation and helps to track and set the seed the generator.
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
        /// Returns true with the given probability, otherwise returns false.
        /// </summary>
        /// <param name="percentage">Probability of returning true</param>
        /// <returns>True with the given probability, otherwise returns false.</returns>
        public bool Boolean(Percentage percentage)
        {
            return random.NextDouble() <= percentage;
        }
    }
}
