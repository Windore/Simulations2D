using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
                SimulationWindow window = new SimulationWindow();
                window.SidePanelWidth = 400;
                ExampleSimulationManager manager = new ExampleSimulationManager(window);

                window.Show();
                manager.StartSimulation();

                window.Closed += (_, __) => manager.StopSimulation();

                desktop.MainWindow = window;
            }

            base.OnFrameworkInitializationCompleted();
        }

        public class MainWindow{}
    }
}
