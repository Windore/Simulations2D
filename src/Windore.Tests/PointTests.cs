using NUnit.Framework;
using Windore.Simulations2D.Util.SMath;

namespace Tests
{
    [TestFixture]
    public class PointTests
    {
        [Test]
        public void DistanceTestMethod1()
        {
            Point point = new Point(5, 3);
            Point point1 = new Point(9, 6);
            Assert.AreEqual(5d, point.DistanceTo(point1));
        }

        [Test]
        public void DistanceTestMethod2()
        {
            Point point = new Point(5, 3);
            Point point1 = new Point(1, 0);
            Assert.AreEqual(5d, point.DistanceTo(point1));
        }
    }
}