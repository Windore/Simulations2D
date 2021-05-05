namespace Windore.Simulations2D.Util
{
    /// <summary>
    /// Represents a RGBA Color
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Gets the value of the red channel
        /// </summary>
        public byte Red { get; }
        /// <summary>
        /// Gets the value of the green channel
        /// </summary>
        public byte Green { get; }
        /// <summary>
        /// Gets the value of the blue channel
        /// </summary>
        public byte Blue { get; }
        /// <summary>
        /// Gets the value of the alpha (opacity) channel
        /// </summary>
        public byte Alpha { get; }

        /// <summary>
        /// Initializes a new Color with the set channel values
        /// </summary>
        /// <param name="red">Value of the red channel</param>
        /// <param name="green">Value of the green channel</param>
        /// <param name="blue">Value of the blue channel</param>
        /// <param name="alpha">Value of the alpha (opacity) channel</param>
        public Color(byte red, byte green, byte blue, byte alpha=255) 
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }
}
