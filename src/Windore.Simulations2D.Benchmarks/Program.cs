using System;
using System.Collections.Generic;
using Windore.Simulations2D.Util;
using Windore.Simulations2D.Util.SMath;

namespace Windore.Simulations2D.Benchmarks
{
    public class Program
    {
        class SimObj : SimulationObject
        {
            readonly double rnge;
            public SimObj(Point point, double range) : base(new Shape(point, 1, 1, true), new Color())
            {
                rnge = range;
            }

            public override void Update()
            {
                GetSimulationObjectsInRange(rnge);
            }

            public double Sum() 
            {
                return rnge + Position.X + Position.Y;
            } 
        }

        // Randomness is used to set the test objs to random locations but by using the same seed the benchmarks should be same every time
        static List<SimObj> CreateObjs(SRandom random, int amount, double width, double height) 
        {
            List<SimObj> objs = new List<SimObj>();
            for (int i = 0; i < amount; i++)
            {
                objs.Add(new SimObj(random.Point(width, height), random.Double(1, Math.Min(width, height))));
            }
            return objs;
        }

        [Benchmark]
        public static void GetInRange100With1000Iter()
        {
            SRandom random = new SRandom(123);
            SimulationScene scene = new SimulationScene(1000, 1000);
            scene.AddAll(CreateObjs(random, 100, 1000, 1000).ToArray());

            for (int i = 0; i < 1000; i++)
            {
                scene.Update();
            }
        }

        [Benchmark]
        public static void GetInRange1000With100Iter()
        {
            SRandom random = new SRandom(123);
            SimulationScene scene = new SimulationScene(1000, 1000);
            scene.AddAll(CreateObjs(random, 1000, 1000, 1000).ToArray());

            for (int i = 0; i < 100; i++)
            {
                scene.Update();
            }
        }

        [Benchmark]
        public static void GetInRange10_000With10Iter()
        {
            SRandom random = new SRandom(123);
            SimulationScene scene = new SimulationScene(1000, 1000);
            scene.AddAll(CreateObjs(random, 10_000, 1000, 1000).ToArray());

            for (int i = 0; i < 10; i++)
            {
                scene.Update();
            }
        }
    }
}
