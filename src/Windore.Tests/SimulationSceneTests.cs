using System;
using System.Collections.Generic;
using NUnit.Framework;
using Windore.Simulations2D.Util.SMath;
using Windore.Simulations2D.Util;

namespace Windore.Simulations2D.Tests
{
    [TestFixture]
    public class SimulationSceneTests
    {
        private class SimulationObjectTestClass : SimulationObject
        {
            private readonly List<int> list;

            public int Number { get; private set; }
            public SimulationObjectTestClass(int number, List<int> callOrderList = null) : base(new Shape(new Point(0, 0), 10, 10, true), new Color())
            {
                Number = number;
                list = callOrderList;
            }

            public override void Update()
            {
                if (list != null)
                {
                    list.Add(Number);
                }
                Number++;
            }
        }

        [Test]
        public void SimulationScene_WidthOrHeightNonPositive()
        {
            Assert.Throws<ArgumentException>(() => { new SimulationScene(-1, 0); } );
        }

        [Test]
        public void UpdateTest()
        {
            SimulationScene scene = new SimulationScene(100, 100);

            SimulationObjectTestClass o1 = new SimulationObjectTestClass(0);
            SimulationObjectTestClass o2 = new SimulationObjectTestClass(1);
            SimulationObjectTestClass o3 = new SimulationObjectTestClass(2);

            scene.AddAll(o1, o2);
            scene.Add(o3);

            scene.Update();

            int[] expected = { 1, 2, 3 };
            int[] result = { o1.Number, o2.Number, o3.Number };

            CollectionAssert.AreEqual(expected, result);
            Assert.AreEqual(1, scene.Age);
        }

        [Test]
        public void RunInOrderTest1()
        {
            SimulationScene scene = new SimulationScene(100, 100);
            List<int> callOrderList = new List<int>();

            SimulationObjectTestClass o1 = new SimulationObjectTestClass(0, callOrderList);
            SimulationObjectTestClass o2 = new SimulationObjectTestClass(1, callOrderList);
            SimulationObjectTestClass o3 = new SimulationObjectTestClass(2, callOrderList);
            SimulationObjectTestClass o4 = new SimulationObjectTestClass(3, callOrderList);
            SimulationObjectTestClass o5 = new SimulationObjectTestClass(4, callOrderList);
            SimulationObjectTestClass o6 = new SimulationObjectTestClass(5, callOrderList);

            scene.AddAll(o1, o2, o3, o4, o5, o6);

            scene.Update();

            CollectionAssert.AreEqual(new int[] { 0, 1, 2, 3, 4, 5 }, callOrderList);
        }

        [Test]
        public void RemoveTest1()
        {
            SimulationScene scene = new SimulationScene(100, 100);
            SimulationObjectTestClass o1 = new SimulationObjectTestClass(0);
            scene.Add(o1);
            o1.Remove();

            Assert.IsTrue(scene.SimulationObjects.Contains(o1));
        }

        [Test]
        public void RemoveTest2()
        {
            SimulationScene scene = new SimulationScene(100, 100);
            SimulationObjectTestClass o1 = new SimulationObjectTestClass(0);
            scene.Add(o1);
            o1.Remove();
            scene.Update();

            Assert.IsFalse(scene.SimulationObjects.Contains(o1));
        }
    }
}
