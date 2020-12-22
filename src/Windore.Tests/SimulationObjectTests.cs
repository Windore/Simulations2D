using System.Collections.Generic;
using NUnit.Framework;
using Windore.Simulations2D;
using Windore.Simulations2D.UI;
using Windore.Simulations2D.Util.SMath;

namespace Tests
{
    [TestFixture]
    public class SimulationObjectsTests
    {
        private class SimulationObjectTestClass : SimulationObject
        {
            public SimulationObjectTestClass(Point position, Shape shape, SFML.Graphics.Color color, int size) : base(position, shape, color, size)
            {
            }

            public override void Update()
            {
                
            }
        }

        [Test]
        public void OverlappingTest1()
        {
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(new Point(0, 0), Shape.Circle, new SFML.Graphics.Color(), 5);
            SimulationObjectTestClass simulationObject1 = new SimulationObjectTestClass(new Point(10, 0), Shape.Circle, new SFML.Graphics.Color(), 5);

            Assert.AreEqual(false, simulationObject.OverlappingWith(simulationObject1));
        }

        [Test]
        public void OverlappingTest2()
        {
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(new Point(0, 0), Shape.Circle, new SFML.Graphics.Color(), 10);
            SimulationObjectTestClass simulationObject1 = new SimulationObjectTestClass(new Point(10, 0), Shape.Circle, new SFML.Graphics.Color(), 11);

            Assert.AreEqual(true, simulationObject.OverlappingWith(simulationObject1));
        }

        [Test]
        public void OverlappingTest3()
        {
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(new Point(0, 0), Shape.Circle, new SFML.Graphics.Color(), 10);
            SimulationObjectTestClass simulationObject1 = new SimulationObjectTestClass(new Point(10, 0), Shape.Circle, new SFML.Graphics.Color(), 11);

            simulationObject.Remove();

            Assert.AreEqual(false, simulationObject.OverlappingWith(simulationObject1));
        }

        [Test]
        public void DontMoveOutsideLimitsTest()
        {
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(new Point(0, 0), Shape.Circle, new SFML.Graphics.Color(), 1);
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
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(point, Shape.Circle, new SFML.Graphics.Color(), 1);

            simulationObject.MoveTowards(targetPoint, 5f);
            Assert.AreEqual(expectedResultPoint.X, simulationObject.Position.X, 0.1);
            Assert.AreEqual(expectedResultPoint.Y, simulationObject.Position.Y, 0.1);
        }

        [Test]
        public void MoveTowardsTest2()
        {
            Point point = new Point(5, 3);
            Point point1 = new Point(9, 6);
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(point, Shape.Circle, new SFML.Graphics.Color(), 1);

            simulationObject.MoveTowards(point1, 8f);
            Assert.AreEqual(point1.X, simulationObject.Position.X, 0.1);
            Assert.AreEqual(point1.Y, simulationObject.Position.Y, 0.1);
        }

        [Test]
        public void GetInRangeTest1()
        {
            SimulationObjectTestClass simulationObject = new SimulationObjectTestClass(new Point(10, 10), Shape.Circle, new SFML.Graphics.Color(), 1);
            SimulationObjectTestClass simulationObject1 = new SimulationObjectTestClass(new Point(20, 10), Shape.Circle, new SFML.Graphics.Color(), 1);
            SimulationObjectTestClass simulationObject2 = new SimulationObjectTestClass(new Point(30, 10), Shape.Circle, new SFML.Graphics.Color(), 1);

            SimulationScene scene = new SimulationScene(100, 100);
            scene.AddAll(simulationObject, simulationObject1, simulationObject2);

            scene.Update();

            List<SimulationObject> objects = simulationObject.GetSimulationObjectsInRange(15);
            Assert.AreEqual(true, objects.Contains(simulationObject1));
            Assert.AreEqual(false, objects.Contains(simulationObject2));
        }
    }
}
