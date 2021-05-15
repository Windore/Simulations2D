using System.Collections.Generic;
using NUnit.Framework;
using Windore.Simulations2D;
using Windore.Simulations2D.Data;
using Windore.Simulations2D.Util.SMath;
using Windore.Simulations2D.Util;
using System.Linq;

namespace Windore.Simulations2D.Tests
{
    [TestFixture]
    public class DataCollectionTests
    {
        private class TestObj : SimulationObject
        {
            [DataPoint("Number")]
            public int Number { get; private set; }

            [DataPoint("Value")]
            public double Value { get; }

            public TestObj(int number, double value) : base(new Shape(new Point(0,0), 7, 7, true), new Color()) 
            {
                Number = number;
                Value = value;
            }

            public override void Update() 
            {
                Number++;
            }
        }

        private class SingleTestObj 
        {
            [DataPoint("SingleValue")]
            public float Value { get; }

            public SingleTestObj(float value) 
            { 
                Value = value;
            }
        }

        private SimulationScene scene;
        private DataCollector collector;
        double allowedError = 0.000000000001d;


        [SetUp]
        public void SetUp() 
        {
            scene = new SimulationScene(100, 100);
            collector = new DataCollector();

            TestObj obj1 = new TestObj(1, 9);
            TestObj obj2 = new TestObj(2, 18);
            TestObj obj3 = new TestObj(3, 27);
            TestObj obj4 = new TestObj(4, 36);
            TestObj obj5 = new TestObj(5, 45);

            scene.AddAll(obj1, obj2, obj3, obj4, obj5);
        }

        [Test]
        public void DataCollector_ReturnsCorrectAverage1() 
        {
            double expected = 27d;
            Assert.AreEqual(expected, collector.CollectData<TestObj>(scene.SimulationObjects.Select(obj => (TestObj)obj))["Value"].Average);
        }

        [Test]
        public void DataCollector_ReturnsCorrectStandardDeviation1() 
        {
            double expected = 12.727922061358d;
            Assert.AreEqual(expected, collector.CollectData<TestObj>(scene.SimulationObjects.Select(obj => (TestObj)obj))["Value"].StandardDeviation, allowedError);
        }

        [Test]
        public void DataCollector_ReturnsCorrectAverage2() 
        {
            double expected = 3d;
            Assert.AreEqual(expected, collector.CollectData<TestObj>(scene.SimulationObjects.Select(obj => (TestObj)obj))["Number"].Average);
        }

        [Test]
        public void DataCollector_ReturnsCorrectStandardDeviation2() 
        {
            double expected = 1.4142135623731d;
            Assert.AreEqual(expected, collector.CollectData<TestObj>(scene.SimulationObjects.Select(obj => (TestObj)obj))["Number"].StandardDeviation, allowedError);
        }

        [Test]
        public void DataCollector_ReturnsCorrectAverage3() 
        {
            scene.Update();
            double expected = 4d;
            Assert.AreEqual(expected, collector.CollectData<TestObj>(scene.SimulationObjects.Select(obj => (TestObj)obj))["Number"].Average);
        }

        [Test]
        public void DataCollector_ReturnsCorrectStandardDeviation3() 
        {
            scene.Update();
            double expected = 1.4142135623731d;
            Assert.AreEqual(expected, collector.CollectData<TestObj>(scene.SimulationObjects.Select(obj => (TestObj)obj))["Number"].StandardDeviation, allowedError);
        }

        [Test]
        public void DataCollector_SingleValueCorrectValue() 
        {
            SingleTestObj obj = new SingleTestObj(19.007f);

            Assert.AreEqual(19.007f, collector.CollectSingleValueData<SingleTestObj>(obj)["SingleValue"].Value);
        }
    }
}