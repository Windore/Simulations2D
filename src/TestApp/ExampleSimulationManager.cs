using System;
using System.Diagnostics;
using Windore.Simulations2D.GUI;
using Windore.Simulations2D.Util;

namespace Windore.Simulations2D.TestApp
{
    // Custom SimulationManagers are not necessary but using them allows doing more things
    class ExampleSimulationManager : SimulationManager 
    {
        // A singelton is used in this example to make the SimulationManager accessible everywhere. 
        // This is not necessarily the best way to do this, but it's used here for simplicity
        public static ExampleSimulationManager Instance { get; private set; }

        // This will be used to count the number of infected SimulationObjects in the scene
        public int InfectedCounter { get; set;}

        // This is the selected SimulationObject. You can select simulationObjects by clicking them.
        public ExampleSimulationObject Selected { get; set; }

        // A custom random class is used to keep track of the randomness seed of the simulation
        public SRandom SimulationRandom { get; private set; }

        int qty = 0;
        double avgUps = 0;

        Stopwatch s = new Stopwatch();

        // Constructor calls the base costructor and creates a new SimulationScene with a size of 1000²
        public ExampleSimulationManager(ISimulationUI ui) : base(new SimulationScene(1000, 1000), ui) 
        {
            Instance = this;

            // A pre-defined randomness seed is used here.
            // This seed should leave a single healthy SimulationObject bouncing around the Scene.
            // If it doesn't please open a issue as it would be a bug
            SimulationRandom = new SRandom(50626451);

            // 1000 healthy custom SimulationObjects are added to the scene
            for(int i = 0; i < 1000; i++)
                SimulationScene.Add(new ExampleSimulationObject(false));
            
            /// And 100 infected custom SimulationObjects are added to the scene
            for (int i = 0; i < 100; i++)
                SimulationScene.Add(new ExampleSimulationObject(true));
            s.Start();
        }

        // This method runs every time before an update in the simulation. 
        protected override void BeforeUpdate() 
        {
            // Reset the infected counter
            InfectedCounter = 0;
        }

        // There is a AfterUpdate method as well.
        protected override void AfterUpdate() 
        {
            // stopwatch is used to get the milliseconds between updates
            s.Stop();
            long elapsed = s.ElapsedMilliseconds;
            qty++;
            double ups = 1d / (elapsed / 1000d);

            if (qty == 1000) 
            {
                avgUps = ups;
                qty = 1;
            }

            avgUps += (ups - avgUps) / qty;

            return;

            // Write data about the simulation to the console
            Console.Clear();
            Console.WriteLine("Simulation Objects: " + SimulationScene.SimulationObjects.Count);
            Console.WriteLine("Infected: " + InfectedCounter);
            Console.WriteLine("Average Updates Per Second: " + Math.Round(avgUps, 3));
            Console.WriteLine("Randomness Seed: " + SimulationRandom.Seed);

            // Now if a simulatioObject is selected and is not removed...
            // (The object needs to be checked for removal because when it's removed from the scene the object here wont be deleted)
            if (Selected != null && !Selected.IsRemoved) 
            {
                // ...data about the selected object can be written on the console
                Console.WriteLine();
                Console.WriteLine("Selected Simulation Object:");
                Console.WriteLine("Is Infected: " + Selected.IsInfected);
                Console.WriteLine("Target Position: " + Selected.TargetPosition);
            }

            s.Reset();
            s.Start();
        }
    }
}