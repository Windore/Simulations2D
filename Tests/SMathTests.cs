using System.Collections.Generic;
using NUnit.Framework;
using Simulations2D.Util.SMath;

namespace Tests
{
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
    }
}
