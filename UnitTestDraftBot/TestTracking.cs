using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestDraftBot
{
    using System.Diagnostics;
    using System.Windows;
    using DraftBot;

    [TestClass]
    public class TestTracking
    {

        [TestMethod]
        public void Caliabrate()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                robot.Beep(150);
                robot.PenDown();
                robot.TurnLeft(180);
                robot.TurnLeft(180);
                robot.PenUp();

                Assert.IsTrue(robot.WaitForComplete());
            }

            Trace.TraceInformation("All done.");
        }

        [TestMethod]
        public void TestGotoHeading()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                var h = 0;
                const int step = 35;
                
                for (var ii = 0; ii < 6; ii++)
                {
                    robot.Beep(150);
                    robot.GotoHeading(h += step);
                    Assert.AreEqual(h, robot.Heading);
                }

                robot.GotoHeading(0);
                Assert.AreEqual(0, robot.Heading);

                Assert.IsTrue(robot.WaitForComplete(TimeSpan.FromSeconds(5)));
            }

            Trace.TraceInformation("All done.");
        }



        [TestMethod]
        public void TestGotoPoint()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                var d = 30;

                robot.Beep(150);
                robot.GotoPoint(new Point(d, d));
                Assert.AreEqual(new Point(d, d), robot.Position);

                robot.GotoPoint(new Point(2 * d, d));
                Assert.AreEqual(new Point(2 * d, d), robot.Position);

                robot.GotoPoint(new Point(0, 0));
                Assert.AreEqual(new Point(0, 0), robot.Position);

                Assert.IsTrue(robot.WaitForComplete(TimeSpan.FromSeconds(5)));
            }

            Trace.TraceInformation("All done.");
        }


        [TestMethod]
        public void DriveLineX()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                robot.PenDown();
                robot.Beep(150);
                robot.Move(100);
                robot.PenUp();

                Assert.IsTrue(robot.WaitForComplete(TimeSpan.FromSeconds(5)));
                Assert.AreEqual(new Point(100, 0), robot.Position);
                Assert.AreEqual(0, robot.Heading);
            }

            Trace.TraceInformation("All done.");
        }

        [TestMethod]
        public void DriveLineY()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                robot.Beep(150);
                robot.Turn(90);
                robot.Move(100);

                Assert.IsTrue(robot.WaitForComplete(TimeSpan.FromSeconds(5)));
                Assert.AreEqual(new Point(0, 100), robot.Position);
                Assert.AreEqual(90, robot.Heading);
            }

            Trace.TraceInformation("All done.");
        }

        [TestMethod]
        public void DriveLineXY()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                robot.PenDown();
                robot.Move(100);
                robot.Turn(90);
                robot.Move(100);
                robot.PenUp();

                Assert.IsTrue(robot.WaitForComplete(TimeSpan.FromSeconds(15)));
                Assert.AreEqual(new Point(100, 100), robot.Position);
                Assert.AreEqual(90, robot.Heading);
            }

            Trace.TraceInformation("All done.");
        }


        [TestMethod]
        public void DriveSquare()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                robot.Beep(150);
                for (var ii = 0; ii < 4; ii++)
                {
                    robot.PenDown();
                    robot.Move(100);
                    robot.PenUp();
                    robot.Turn(90);
                    robot.Beep(10);
                }

                Assert.IsTrue(robot.WaitForComplete());
                Assert.AreEqual(21, robot.JobsComplete);
                Assert.AreEqual(new Point(0,0), robot.Position);
            }

            Trace.TraceInformation("All done.");
        }

        [TestMethod]
        public void DriveTriangle()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                robot.Beep(150);
                for (var ii = 0; ii < 3; ii++)
                {
                    robot.Move(100);
                    robot.Turn(360 / 3);
                    robot.Beep(10);
                }

                Assert.IsTrue(robot.WaitForComplete());
                Assert.AreEqual(10, robot.JobsComplete);
            }

            Trace.TraceInformation("All done.");
        }
    }
}
