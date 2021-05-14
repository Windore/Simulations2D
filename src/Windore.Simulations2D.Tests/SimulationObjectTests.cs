using System.Collections.Generic;
using NUnit.Framework;
using Windore.Simulations2D.Util;
using Windore.Simulations2D.Util.SMath;

namespace Windore.Simulations2D.Tests
{
    [TestFixture]
    public class SimulationObjectsTests
    {
        private class SimulationObjectTestClass : SimulationObject
        {
            public SimulationObjectTestClass(Shape shape, Color color) : base(shape, color)
            {
            }

            public override void Update()
            {
                
            }
        }

        [Test]
        public void DontMoveOutsideLimitsTest()
        {
            Shape shape = new Shape(new Point(0,0), 2, 2, false);
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(shape, new Color());
            new SimulationScene(100, 100).Add(simulationObject);
            simulationObject.Position = new Point(1001, 0);
            Assert.AreEqual(new Point(100, 0), simulationObject.Position);
        }

        [Test]
        public void MoveTowardsTest1()
        {
            Point point = new Point(5, 3);
            Point targetPoint = new Point(13, 9);
            Point expectedResultPoint = new Point(9, 6);
            Shape shape = new Shape(point, 2, 2, false);
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(shape, new Color());

            simulationObject.MoveTowards(targetPoint, 5f);
            Assert.AreEqual(expectedResultPoint.X, simulationObject.Position.X, 0.1);
            Assert.AreEqual(expectedResultPoint.Y, simulationObject.Position.Y, 0.1);
        }

        [Test]
        public void MoveTowardsTest2()
        {
            Point point = new Point(5, 3);
            Point point1 = new Point(9, 6);
            Shape shape = new Shape(point, 2, 2, false);
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(shape, new Color());

            simulationObject.MoveTowards(point1, 8f);
            Assert.AreEqual(point1.X, simulationObject.Position.X, 0.1);
            Assert.AreEqual(point1.Y, simulationObject.Position.Y, 0.1);
        }

        [Test]
        public void GetInRangeTest1()
        {
            
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(new Shape(new Point(10,10), 1, 1, true), new Color());
            SimulationObjectTestClass simulationObject1 = new SimulationObjectTestClass(new Shape(new Point(20, 10), 1, 1, true), new Color());
            SimulationObjectTestClass simulationObject2 = new SimulationObjectTestClass(new Shape(new Point(30,10), 1, 1, true), new Color());

            SimulationScene scene = new SimulationScene(100, 100);
            scene.AddAll(simulationObject, simulationObject1, simulationObject2);

            scene.Update();

            List<SimulationObject> objects = simulationObject.GetSimulationObjectsInRange(15);
            CollectionAssert.AreEquivalent(new SimulationObjectTestClass[] { simulationObject1 }, objects);
        }

        [Test]
        public void GetInRangeTest2()
        {
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(new Shape(new Point(4, 2), 1, 1, true), new Color());
            SimulationObjectTestClass simulationObject1 = new SimulationObjectTestClass(new Shape(new Point(2, 12), 2, 2, true), new Color());
            SimulationObjectTestClass simulationObject2 = new SimulationObjectTestClass(new Shape(new Point(10, 8), 3, 2, true), new Color());
            SimulationObjectTestClass simulationObject3 = new SimulationObjectTestClass(new Shape(new Point(4, 3), 1.5, 1, true), new Color());

            SimulationScene scene = new SimulationScene(100, 100);
            scene.AddAll(simulationObject, simulationObject1, simulationObject2, simulationObject3);

            scene.Update();

            List<SimulationObject> objects = simulationObject.GetSimulationObjectsInRange(10);
            CollectionAssert.AreEquivalent(new SimulationObjectTestClass[] { simulationObject2, simulationObject3 }, objects);
        }
    }
}
