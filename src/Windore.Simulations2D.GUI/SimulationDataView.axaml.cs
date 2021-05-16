using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using Windore.Simulations2D.Data;
using System.Text;
using System;

namespace Windore.Simulations2D.GUI
{
    public class SimulationDataView : UserControl
    {
        private Grid grid;
        private Dictionary<string, DataCollector.Data> data = new Dictionary<string, DataCollector.Data>(); 
        public Dictionary<string, DataCollector.Data> Data 
        {
            get => data;
            set
            {
                data = value;
                CreateGrid();
            }
        }
        public bool HideSingleValueData { get; set; }
        public bool Rounding { get; set; }

        public SimulationDataView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            grid = this.Find<Grid>("grid");
        }

        private void CreateGrid() 
        {
            grid.Children.Clear();
            CreateRows();

            int row = 0;
            if (!HideSingleValueData) 
            {
                AddHeaders(row, new string[]{"Name", "Value"});
                row++;

                AddSingleValueData(ref row);
            }

            AddHeaders(row, new string[]{"Name", "Average", "Standard Deviation"});
            row++;

            AddData(row);
        }

        private void AddData(int row) 
        {
            foreach(string key in Data.Keys) 
            {
                if (Data[key].IsSingleValue) continue;
                
                TextBlock name = new TextBlock() 
                {
                    Text=key,
                    TextWrapping=Avalonia.Media.TextWrapping.Wrap
                };

                TextBlock average = new TextBlock() 
                {
                    Text=Math.Round(Data[key].Average, 3).ToString(),
                    TextWrapping=Avalonia.Media.TextWrapping.Wrap
                };

                TextBlock sd = new TextBlock() 
                {
                    Text=Math.Round(Data[key].StandardDeviation, 3).ToString(),
                    TextWrapping=Avalonia.Media.TextWrapping.Wrap
                };

                grid.Children.Add(name);
                grid.Children.Add(average);
                grid.Children.Add(sd);

                Grid.SetColumn(name, 0);
                Grid.SetColumn(average, 1);
                Grid.SetColumn(sd, 2);

                Grid.SetRow(name, row);
                Grid.SetRow(average, row);
                Grid.SetRow(sd, row);

                row++;
            }
        }

        private void AddSingleValueData(ref int row) 
        {
            foreach(string key in Data.Keys) 
            {
                if (!Data[key].IsSingleValue) continue;
                
                TextBlock name = new TextBlock() 
                {
                    Text=key,
                    TextWrapping=Avalonia.Media.TextWrapping.Wrap
                };

                TextBlock value = new TextBlock() 
                {
                    Text=Math.Round(Data[key].Value, 3).ToString(),
                    TextWrapping=Avalonia.Media.TextWrapping.Wrap
                };

                grid.Children.Add(name);
                grid.Children.Add(value);

                Grid.SetColumn(name, 0);
                Grid.SetColumn(value, 1);

                Grid.SetRow(name, row);
                Grid.SetRow(value, row);

                row++;
            }
        }

        private void AddHeaders(int row, string[] headers) 
        {
            int col = 0;

            foreach(string header in headers) 
            {
                TextBlock headerTB = new TextBlock() 
                {
                    Text=header,
                    FontSize=16,
                    FontWeight=Avalonia.Media.FontWeight.Bold,
                    TextWrapping=Avalonia.Media.TextWrapping.Wrap
                };

                grid.Children.Add(headerTB);

                Grid.SetColumn(headerTB, col);
                Grid.SetRow(headerTB, row);
                col++;
            }
        }

        private void CreateRows() 
        {
            StringBuilder rowDefinitionsBuilder = new StringBuilder();
            rowDefinitionsBuilder.Append("1*");

            if (!HideSingleValueData) 
            {
                rowDefinitionsBuilder.Append(",1*");
            }

            foreach(string key in Data.Keys) 
            {
                if (Data[key].IsSingleValue && HideSingleValueData) continue;

                rowDefinitionsBuilder.Append(",1*");
            }

            grid.RowDefinitions = new RowDefinitions(rowDefinitionsBuilder.ToString());
        }
    }
}