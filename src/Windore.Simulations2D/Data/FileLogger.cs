using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

namespace Windore.Simulations2D.Data
{
    /// <summary>
    /// Provides a class for logging collected data to a file
    /// </summary>
    public class FileLogger
    {
        private const string SEPERATOR = "/t";
        private const string FILE_EXTENSION = ".tsv";
        private int logCount = 0;
        private List<string> logTitles = new List<string>();
        private Dictionary<string, bool> areTitlesSingeValue = new Dictionary<string, bool>();
        
        /// <summary>
        /// Gets the current name of the log file
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Initializes a new file logger with a name and a list of values to be logged from data.
        /// </summary>
        /// <param name="fileName">The preliminary file name</param>
        /// <param name="loggedValues">The names of the data to be expected and logged.</param>
        public FileLogger(string fileName, params string[] loggedValues) 
        {
            // Find a file that doesn't exist by adding numbers in the end of the file name
            int num = 0;
            string addition = $"-0{FILE_EXTENSION}";

            while (File.Exists(fileName + addition))
            {
                num++;
                addition = $"-{num}{FILE_EXTENSION}";
            }
            FileName = fileName + addition;

            logTitles = loggedValues.ToList();
        }

        /// <summary>
        /// Appends the given data to the log
        /// </summary>
        /// <param name="data">The data to append to the log</param>
        public void Log(Dictionary<string, DataCollector.Data> data) 
        {
            if (logCount == 0) 
            {
                WriteTitles(data);
            }

            StringBuilder sBuilder = new StringBuilder();

            foreach (string key in logTitles) 
            {
                if (data[key].IsSingleValue != areTitlesSingeValue[key]) 
                {
                    throw new ArgumentException("Data cannot change from a single value to an average value or vice versa.");
                }

                if (data[key].IsSingleValue) 
                {
                    sBuilder.Append(data[key].Value);
                    sBuilder.Append(SEPERATOR);
                }
                else 
                {
                    sBuilder.Append(data[key].Average);
                    sBuilder.Append(SEPERATOR);

                    sBuilder.Append(data[key].StandardDeviation);
                    sBuilder.Append(SEPERATOR);
                }
            }

            sBuilder.AppendLine();
            File.AppendAllText(FileName, sBuilder.ToString());
            logCount++;
        }

        private void WriteTitles(Dictionary<string, DataCollector.Data> data) 
        {
            StringBuilder builder = new StringBuilder();
            foreach (string key in logTitles) 
            {
                areTitlesSingeValue.Add(key, data[key].IsSingleValue);
                if (data[key].IsSingleValue) 
                {
                    builder.Append(key);
                    builder.Append(SEPERATOR);
                }
                else 
                {
                    builder.Append(key);
                    builder.Append("-Average");
                    builder.Append(SEPERATOR);

                    builder.Append(key);
                    builder.Append("-StandardDeviation");
                    builder.Append(SEPERATOR);
                }
            }
            builder.AppendLine();
            File.AppendAllText(FileName, builder.ToString());
        }
    }
}