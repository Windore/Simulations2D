using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using Windore.Simulations2D.GUI;

namespace Windore.Simulations2D.TestApp
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                ExampleSimulationManager manager = new ExampleSimulationManager();
                SimulationWindow window = new SimulationWindow(manager);
                window.SidePanelWidth = 400;
                window.Show();

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(0.5d);
                timer.Tick += (_, __) => UpdateSidePanel(window);
                timer.Start();

                desktop.MainWindow = window;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void UpdateSidePanel(SimulationWindow window) 
        {
            window.SetSidePanelContent(new TextBlock() 
            {
                Margin = new Avalonia.Thickness(5),
                FontSize = 16,
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Text = "Test simulaton"
            });

            SimulationDataView view = new SimulationDataView() 
            {
                Data = ((ExampleSimulationManager)window.SimulationManager).Data,
                HideSingleValueData = false,
                Rounding = true
            };

            window.AddSidePanelContent(view);

            window.AddSidePanelContent(new TextBlock() 
            {
                Margin = new Avalonia.Thickness(5),
                FontSize = 16,
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Text = "Selected object"
            });

            SimulationObject? selected = window.SimulationManager.SimulationScene.SelectedSimulationObject;

            if (selected == null || selected.IsRemoved) 
            {
                window.AddSidePanelContent(new TextBlock() 
                {
                    Margin = new Avalonia.Thickness(5),
                    Text = "No selection"
                });
            }
            else if (selected is ExampleSimulationObject selectedEx) 
            {
                window.AddSidePanelContent(new TextBlock() 
                {
                    Margin = new Avalonia.Thickness(5),
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                    Text = $"Target Position: {selectedEx.TargetPosition}\nIs Infected: {selectedEx.IsInfected}\nJust A Number: {selectedEx.Number}"
                });
            }
        }
    }
}
