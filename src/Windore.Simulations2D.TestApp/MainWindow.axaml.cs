using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading;
using Windore.Simulations2D.GUI;
using Windore.Simulations2D.TestApp;

namespace Windore.Simulations2D.TestApp
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            SimulationWindow window = new SimulationWindow();
            ExampleSimulationManager manager = new ExampleSimulationManager(window);

            window.Show();
            manager.StartSimulation();

            window.Closed += (_, __) => manager.StopSimulation();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
