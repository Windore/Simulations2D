using System.Collections.Generic;
using Windore.Simulations2D.Util.SMath;
using System.Linq;
using System.Reflection;
using System;

namespace Windore.Simulations2D.Data
{
    /// <summary>
    /// Provides a class for collecting data from objects with DataPoint attributes
    /// </summary>
    public class DataCollector
    {
        /// <summary>
        /// Represents data collected by a DataCollector
        /// </summary>
        public class Data
        {
            /// <summary>
            /// Gets the name of the data
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Gets if the data is a value from a single source
            /// </summary>
            public bool IsSingleValue { get; }
            /// <summary>
            /// Gets the value of the data provided that the data is from a single source
            /// </summary>
            public double Value { get; }
            /// <summary>
            /// Gets the average value of the data collected from multiple sources
            /// </summary>
            public double Average
            {
                get
                {
                    if (valuePoints.Count == 0)
                    {
                        return 0;
                    }
                    return valuePoints.Average();
                }
            }
            /// <summary>
            /// Gets the standard deviation of the data collected from multiple sources
            /// </summary>
            public double StandardDeviation { get => SMath.StandardDeviation(valuePoints); }
            /// <summary>
            /// Gets the amount of data points
            /// </summary>
            public int ValuePointAmount { get => valuePoints.Count; }

            private List<double> valuePoints = new List<double>();

            /// <summary>
            /// Initializes a new multiple value source data instance with a given name.
            /// </summary>
            /// <param name="name">The name of the data</param>
            public Data(string name)
            {
                Name = name;
                Value = -1;
                IsSingleValue = false;
            }

            /// <summary>
            /// Initializes a new single value source data instance with a given name and a given value.
            /// </summary>
            /// <param name="name">The name of the data</param>
            /// <param name="value">The value of the data</param>
            public Data(string name, double value)
            {
                Name = name;
                IsSingleValue = true;
                Value = value;
                valuePoints.Add(value);
            }

            /// <summary>
            /// Adds a new value to the data
            /// </summary>
            /// <param name="value">The value added to the data</param>
            public void AddValue(double value)
            {
                if (IsSingleValue)
                {
                    throw new ArgumentException("Cannot add a value to data that is from a single source.");
                }
                valuePoints.Add(value);
            }

            /// <summary>
            /// Returns a deep copy of the data
            /// </summary>
            /// <returns>a deep copy of the data</returns>
            public Data DeepCopy()
            {
                if (IsSingleValue)
                {
                    return new Data(Name, Value);
                }
                Data dt = new Data(Name);
                foreach (double val in valuePoints)
                {
                    dt.AddValue(val);
                }
                return dt;
            }
        }

        /// <summary>
        /// Collects multiple value data from an IEnumerable containing objects with DataPoint attributes
        /// </summary>
        /// <param name="objects">The objects from which to collect the data</param>
        public Dictionary<string, Data> CollectData<T>(IEnumerable<T> objects)
        {
            Dictionary<string, Data> data = new Dictionary<string, Data>();
            List<PropertyInfo> dataPointProperties = new List<PropertyInfo>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                DataPointAttribute attr = property.GetCustomAttribute<DataPointAttribute>();
                if (attr != null)
                {
                    dataPointProperties.Add(property);
                    data.Add(attr.Name, new Data(attr.Name));
                }
            }

            foreach (T obj in objects)
            {
                foreach (PropertyInfo property in dataPointProperties)
                {
                    string name = property.GetCustomAttribute<DataPointAttribute>().Name;
                    object value = property.GetValue(obj);

                    data[name].AddValue(Convert.ToDouble(value));
                }
            }

            return data;
        }

        /// <summary>
        /// Collects single value data from an object with DataPoint attributes
        /// </summary>
        /// <param name="obj">The object from which to collect the data</param>
        public Dictionary<string, Data> CollectSingleValueData<T>(T obj)
        {
            Dictionary<string, Data> data = new Dictionary<string, Data>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                DataPointAttribute attr = property.GetCustomAttribute<DataPointAttribute>();
                if (attr != null)
                {
                    string name = property.GetCustomAttribute<DataPointAttribute>().Name;
                    object value = property.GetValue(obj);

                    data.Add(name, new Data(name, Convert.ToDouble(value)));
                }
            }

            return data;
        }

        /// <summary>
        /// Gets all DataPoint titles in a single type
        /// </summary>
        /// <param name="type">The type in which to get DataPoint titles</param>
        public List<string> GetTypeDataPointTitles(Type type)
        {
            List<string> titles = new List<string>();

            foreach (PropertyInfo property in type.GetProperties())
            {
                DataPointAttribute attr = property.GetCustomAttribute<DataPointAttribute>();
                if (attr != null)
                {
                    string name = property.GetCustomAttribute<DataPointAttribute>().Name;
                    titles.Add(name);
                }
            }

            return titles;
        }
    }
}