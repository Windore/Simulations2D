using System;

namespace Windore.Simulations2D.Data
{
    /// <summary>
    /// Indicates that a given property is a data point.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataPointAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the data this property contains
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new DataPoint instance with a given name for the data the marked property contains.
        /// </summary>
        /// <param name="name">The name for the data the marked property contains.</param>
        public DataPointAttribute(string name) 
        {
            Name = name;
        }
    }
}