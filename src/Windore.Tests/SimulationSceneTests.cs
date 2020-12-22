using System.Collections.Generic;
using NUnit.Framework;
using Windore.Simulations2D;
using Windore.Simulations2D.Util.SMath;

namespace Tests
{
    [TestFixture]
    public class SimulationSceneTests
    {
        private class SimulationObjectTestClass : SimulationObject
        {
            private readonly List<int> list;

            public int Number { get; private set; }
            public SimulationObjectTestClass(int number, List<int> callOrderList = null) : base(new Point(0, 0), Windore.Simulations2D.UI.Shape.Circle, new SFML.Graphics.Color(), 10)
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
        public void UpdateTest1()
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

            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != result[i])
                {
                    Assert.Fail(string.Format("Expected result {0} got {1}", expected[i], result[i]));
                    return;
                }
            }
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

            for (int i = 0; i < 6; i++)
            {
                if (i != callOrderList[i])
                {
                    Assert.Fail("Expected result {0} got {1}", i, callOrderList[i]);
                    return;
                }
            }
        }

        [Test]
        public void RemoveTest1()
        {
            SimulationScene scene = new SimulationScene(100, 100);
            SimulationObjectTestClass o1 = new SimulationObjectTestClass(0);
            scene.Add(o1);

            scene.Update();

            o1.Remove();

            Assert.IsTrue(scene.SimulationObjects.Contains(o1));
        }

        [Test]
        public void RemoveTest2()
        {
            SimulationScene scene = new SimulationScene(100, 100);
            SimulationObjectTestClass o1 = new SimulationObjectTestClass(0);
            scene.Add(o1);
            scene.Update();
            o1.Remove();
            scene.Update();

            Assert.IsTrue(!scene.SimulationObjects.Contains(o1));
        }

        [Test]
        public void UpdateTest()
        {
            TestObj obj = new TestObj
            {
                Position = new Point(100, 100)
            };
            TestObj obj1 = new TestObj
            {
                Position = new Point(95, 95)
            };
            TestObj obj2 = new TestObj
            {
                Position = new Point(40, 10)
            };
            TestObj obj3 = new TestObj
            {
                Position = new Point(105, 103)
            };

            SimulationScene scene = new SimulationScene(200, 200);
            scene.AddAll(obj, obj1, obj2, obj3);

            scene.Update();

            Assert.AreEqual(2, obj.ObjectsInRange);
        }

        private class TestObj : SimulationObject
        {
            public TestObj() : base(new Point(0, 0), Windore.Simulations2D.UI.Shape.Circle, new SFML.Graphics.Color(), 10)
            {
            }

            public override void Update()
            {
                ObjectsInRange = GetSimulationObjectsInRange(10).Count;
            }

            public int ObjectsInRange { get; set; }
        }
    }
}
