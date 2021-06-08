using System.IO;
using System.Diagnostics;
using Windore.Simulations2D.Util;
using Windore.Simulations2D.Data;
using System.Collections.Generic;
using System.Linq;

namespace Windore.Simulations2D.TestApp
{
    // Custom SimulationManagers are not necessary but using them allows doing more things
    class ExampleSimulationManager : SimulationManager 
    {
        private static ExampleSimulationManager? ins;
        // A singelton is used in this example to make the SimulationManager accessible everywhere. 
        // This is not necessarily the best way to do this, but it's used here for simplicity
        public static ExampleSimulationManager Instance 
        {
            get 
            {
                if (ins == null) 
                {
                    throw new System.Exception();
                }
                return ins;
            }
            set => ins = value;
        }

        public Dictionary<string, DataCollector.Data> Data { get; private set; } = new Dictionary<string, DataCollector.Data>();

        // This will be used to count the number of infected SimulationObjects in the scene
        [DataPoint("InfectedCounter")]
        public int InfectedCounter { get; set;}

        [DataPoint("SimulationObjectsCount")]
        public int SimulationObjectsCount { get => SimulationScene.SimulationObjects.Count; }

        [DataPoint("Age")]
        public ulong Age { get => SimulationScene.Age; }

        // A custom random class is used to keep track of the randomness seed of the simulation
        public SRandom SimulationRandom { get; private set; }

        int qty = 0;
        double avgUps = 0;
        private FileLogger logger = new FileLogger(Path.Combine(Path.GetTempPath(), "test-simulation-log"), "Age", "SimulationObjectsCount", "InfectedCounter", "JustANumber");
        private DataCollector collector = new DataCollector();

        Stopwatch s = new Stopwatch();

        // Constructor calls the base costructor and creates a new SimulationScene with a size of 1000²
        public ExampleSimulationManager() : base(new SimulationScene(1000, 1000)) 
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

            s.Reset();
            s.Start();

            if (SimulationScene.Age % 100 == 0) 
            {
                Data = new Dictionary<string, DataCollector.Data>();

                collector.CollectData<ExampleSimulationObject>(SimulationScene.SimulationObjects
                    .Select(obj => (ExampleSimulationObject)obj))
                    .ToList()
                    .ForEach(obj => Data.Add(obj.Key, obj.Value));

                collector.CollectSingleValueData<ExampleSimulationManager>(this)
                    .ToList()
                    .ForEach(obj => Data.Add(obj.Key, obj.Value));

                logger.Log(Data);  
            }
        }
    }
}