using System;

namespace Simulations2D.Util.SMath
{
    /// <summary>
    /// Represents a percentage amount.
    /// </summary>
    [Serializable]
    public class Percentage
    {
        private readonly double value = 0;

        /// <summary>
        /// Initializes a new Percentage instance from a percentage amount e.g 20.
        /// </summary>
        /// <param name="percentage">A percentage amount</param>
        public Percentage(double percentage)
        {
            value = percentage / 100d;
        }

        /// <summary>
        /// Initializes a new Percentage instance from a number e.g 0.2.
        /// </summary>
        /// <param name="number">A double number</param>
        /// <returns></returns>
        public static Percentage FromDecimal(double number)
        {
            return new Percentage(number * 100d);
        }

        /// <summary>
        /// Returns a string that represents the current Percentage.
        /// </summary>
        /// <returns>a string that represents the current Percentage.</returns>
        public override string ToString()
        {
            return string.Format("{0}%", value * 100D);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current Percentage.
        /// </summary>
        /// <param name="obj">The object to compare with the current Percentage.</param>
        /// <returns>true if the specified object is equal to the current Percentage; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is Percentage percentage &&
                   value == percentage.value;
        }

        /// <summary>
        /// Gets the hash code for the current Percentage.
        /// </summary>
        /// <returns>A hash code for the current Percentage.</returns>
        public override int GetHashCode()
        {
            return -1584136870 + value.GetHashCode();
        }

        //I don't really think these need a summary, and I'm too lazy to create it
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static implicit operator double(Percentage percentage) => percentage.value;
        public static Percentage operator +(Percentage a) => a;
        public static Percentage operator -(Percentage a) => FromDecimal(a.value);
        public static Percentage operator +(Percentage a, Percentage b) => FromDecimal(a.value + b.value);
        public static Percentage operator -(Percentage a, Percentage b) => FromDecimal(a.value - b.value);
        public static Percentage operator *(Percentage a, Percentage b) => FromDecimal(a.value * b.value);
        public static Percentage operator /(Percentage a, Percentage b) => FromDecimal(a.value / b.value);
        public static bool operator <(Percentage a, Percentage b) => a.value < b.value;
        public static bool operator >(Percentage a, Percentage b) => a.value > b.value;
        public static bool operator ==(Percentage a, Percentage b) => a.Equals(b);
        public static bool operator !=(Percentage a, Percentage b) => !a.Equals(b);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
