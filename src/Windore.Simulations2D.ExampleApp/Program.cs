using System.Threading;

namespace Windore.Simulations2D.ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a SimulationManager to manage the program. Here a custom manager is used, but using one is not necessary.
            // Custom managers are recommended, though.
            ExampleSimulationManager manager = new ExampleSimulationManager();

            // All simulation objects are added within the manager

            // Now the simulation can be started. This will start the simulation in a different thread.
            manager.StartSimulation();

            // If wanted the simulation could be started with the following:
            //
            // manager.StartSimulation(60);
            //
            // This would limit the maximum ups to be  approx. 60.
            // The limit is not precise.

            // And a 1000 by 1000 window of the simulation is opened. 
            // Note that the size of the window doesn't need to match the size of the SimulationScene. Everything will be scaled
            manager.OpenSimulationWindow(1000, 1000, "Simulation window");  

            // Let the simulation run for 100 seconds
            Thread.Sleep(100_000);

            // Close the simulation and the simulation window.
            // The simulation or the simulationWindow run independently of each other so either of them can run alone.
            manager.StopSimulation();
            manager.CloseSimulationWindow();
        }
    }
}
