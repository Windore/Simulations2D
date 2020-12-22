using System;

namespace Windore.Simulations2D.UI
{
    /// <summary>
    /// Represents the visual shape of a SimulationObject in UI.
    /// <para>
    /// For simplicity all shapes are calculated as circles.
    /// </para>
    /// </summary>
    [Serializable]
    public enum Shape
    {
        /// <summary>
        /// Indicates a Circle shape
        /// </summary>
        Circle,
        /// <summary>
        /// Indicates a Square shape
        /// </summary>
        Square,
        /// <summary>
        /// Indicates a Triangle shape
        /// </summary>
        Triangle
    }
}
