using System.Collections.Generic;
using NUnit.Framework;
using Windore.Simulations2D.Util.SMath;
using System;

namespace Windore.Simulations2D.Tests
{
    [TestFixture]
    public class SMathTests
    {
        [Test]
        public void ClampTestMethod1()
        {
            double value = 10;
            double result = SMath.Clamp(value, 0, 5);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void ClampTestMethod2()
        {
            double value = -10;
            double result = SMath.Clamp(value, -5, 10);
            Assert.AreEqual(-5, result);
        }

        [Test]
        public void ClampTestMethod3()
        {
            double value = 10;
            double result = SMath.Clamp(value, 0, 40);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void StandardDeviationTestMethod1()
        {
            double[] numbers = { 10, 12, 23, 23, 16, 23, 21, 16 };
            double sd = SMath.StandardDeviation(numbers);
            Assert.AreEqual(4.8989794855664, sd, 0.001);
        }

        [Test]
        public void StandardDeviationTestMethod2()
        {
            List<double> numbers = new List<double>() { 10, 12, 23, 23, 16, 23, 21, 16 };
            double sd = numbers.StandardDeviation();
            Assert.AreEqual(4.8989794855664, sd, 0.001);
        }

        [Test]
        public void CircleIntersectRectangleTestMethod1()
        {
            Point circleCenterPoint = new Point(3, 4);
            double circleRadius = 2;

            Point rectangleCenter = new Point(3, 4);
            double rectangleWidth = 5;
            double rectangleHeight = 8;

            Assert.AreEqual(true, SMath.CircleOverlapsRectangle(circleCenterPoint, circleRadius,
                rectangleCenter, rectangleWidth, rectangleHeight));
        }

        [Test]
        public void CircleIntersectRectangleTestMethod2()
        {
            Point circleCenterPoint = new Point(3, 4);
            double circleRadius = 2;

            Point rectangleCenter = new Point(30, 40);
            double rectangleWidth = 5;
            double rectangleHeight = 8;

            Assert.AreEqual(false, SMath.CircleOverlapsRectangle(circleCenterPoint, circleRadius,
                rectangleCenter, rectangleWidth, rectangleHeight));
        }

        [Test]
        public void CircleIntersectRectangleTestMethod3()
        {
            Point circleCenterPoint = new Point(3, 4);
            double circleRadius = 2;

            Point rectangleCenter = new Point(6, 4);
            double rectangleWidth = 2;
            double rectangleHeight = 1.5;

            Assert.AreEqual(true, SMath.CircleOverlapsRectangle(circleCenterPoint, circleRadius,
                rectangleCenter, rectangleWidth, rectangleHeight));
        }

        [Test]
        public void CircleIntersectRectangleTestMethod4()
        {
            Point circleCenterPoint = new Point(3, 4);
            double circleRadius = 2;

            Point rectangleCenter = new Point(6, 4);
            double rectangleWidth = 14;
            double rectangleHeight = 1.5;

            Assert.AreEqual(true, SMath.CircleOverlapsRectangle(circleCenterPoint, circleRadius,
                rectangleCenter, rectangleWidth, rectangleHeight));
        }

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

        [Test]
        public void DistanceTestMethod3()
        {
            Point point = new Point(-8, 12);
            Point point1 = new Point(-4, 15);
            Assert.AreEqual(5d, point.DistanceTo(point1));
        }

        [Test]
        public void DistanceTestMethod4()
        {
            Point point = new Point(-2, -2);
            Point point1 = new Point(1, 2);
            Assert.AreEqual(5d, point.DistanceTo(point1));
        }

        [Test]
        public void Percentage_FromDoubleTest() 
        {
            Assert.AreEqual(new Percentage(20), Percentage.FromDouble(0.2));
        }

        [Test]
        public void Shape_EllipseOverlapsEllipseReturnsTrue() 
        {
            Shape a = new Shape(new Point(2, 2), 3, 2, true);
            Shape b = new Shape(new Point(5, 2), 4, 2, true);

            Assert.IsTrue(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsEllipseReturnsTrue1()
        {
            Shape a = new Shape(new Point(2, 2), 3, 2, true);
            Shape b = new Shape(new Point(2, 4), 2.5, 2.5, true);

            Assert.IsTrue(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsEllipseReturnsTrue2()
        {
            Shape a = new Shape(new Point(-3, 1), 4.5, 4, true);
            Shape b = new Shape(new Point(-1.5, -2), 3, 3, true);

            Assert.IsTrue(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsEllipseReturnsFalse()
        {
            Shape a = new Shape(new Point(2, 2), 3, 2, true);
            Shape b = new Shape(new Point(5, 2), 2, 2, true);

            Assert.IsFalse(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsEllipseReturnsFalse1()
        {
            Shape a = new Shape(new Point(2, 2), 3, 2, true);
            Shape b = new Shape(new Point(-1.5, -2), 3, 3, true);

            Assert.IsFalse(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsEllipseReturnsFalse2()
        {
            Shape a = new Shape(new Point(-3, 1), 4.5, 4, true);
            Shape b = new Shape(new Point(2, 4), 2.5, 2.5, true);

            Assert.IsFalse(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsRectangleReturnsTrue()
        {
            Shape a = new Shape(new Point(5, 2), 2, 2, true);
            Shape b = new Shape(new Point(7, 2), 2, 2, false);

            Assert.IsTrue(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsRectangleReturnsTrue1()
        {
            Shape a = new Shape(new Point(2, 2), 3, 2, true);
            Shape b = new Shape(new Point(1.5, 3), 1, 2, false);

            Assert.IsTrue(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsRectangleReturnsTrue2()
        {
            Shape a = new Shape(new Point(-1.5, -2), 3, 3, true);
            Shape b = new Shape(new Point(-0.5, -1.5), 1, 1, false);

            Assert.IsTrue(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsRectangleReturnsFalse()
        {
            Shape a = new Shape(new Point(5, 2), 2, 2, true);
            Shape b = new Shape(new Point(-0.5, -1.5), 1, 1, false);

            Assert.IsFalse(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsRectangleReturnsFalse1()
        {
            Shape a = new Shape(new Point(-3, 1), 4.5, 4, true);
            Shape b = new Shape(new Point(7, 2), 2, 2, false);

            Assert.IsFalse(a.Overlaps(b));
        }

        [Test]
        public void Shape_EllipseOverlapsRectangleReturnsFalse2()
        {
            Shape a = new Shape(new Point(5, 2), 2, 2, true);
            Shape b = new Shape(new Point(-1.5, 4.5), 1, 1, false);

            Assert.IsFalse(a.Overlaps(b));
        }

        [Test]
        public void Shape_RectangleOverlapsRectangleReturnsTrue()
        {
            Shape a = new Shape(new Point(-4, 4), 2, 2, false);
            Shape b = new Shape(new Point(-1, 5), 6, 6, false);

            Assert.IsTrue(a.Overlaps(b));
        }

        [Test]
        public void Shape_RectangleOverlapsRectangleReturnsTrue1()
        {
            Shape a = new Shape(new Point(-1, 0), 2, 8, false);
            Shape b = new Shape(new Point(-1, 5), 6, 6, false);

            Assert.IsTrue(a.Overlaps(b));
        }

        [Test]
        public void Shape_RectangleOverlapsRectangleReturnsTrue2()
        {
            Shape a = new Shape(new Point(-4, 4), 2, 2, false);
            Shape b = new Shape(new Point(-6, 4.5), 2.1, 1, false);

            Assert.IsTrue(a.Overlaps(b));
        }

        [Test]
        public void Shape_RectangleOverlapsRectangleReturnsFalse()
        {
            Shape a = new Shape(new Point(-4, 4), 2, 2, false);
            Shape b = new Shape(new Point(-1, 0), 2, 8, false);

            Assert.IsFalse(a.Overlaps(b));
        }

        [Test]
        public void Shape_RectangleOverlapsRectangleReturnsFalse1()
        {
            Shape a = new Shape(new Point(-3, -3), 2, 2, false);
            Shape b = new Shape(new Point(-1, 0), 2, 8, false);

            Assert.IsFalse(a.Overlaps(b));
        }

        [Test]
        public void Shape_RectangleOverlapsRectangleReturnsFalse2()
        {
            Shape a = new Shape(new Point(-6, 4.5), 2.1, 1, false);
            Shape b = new Shape(new Point(-1, 0), 2, 8, false);

            Assert.IsFalse(a.Overlaps(b));
        }
    }
}
