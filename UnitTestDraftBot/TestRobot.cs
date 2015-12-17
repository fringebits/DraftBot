using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestDraftBot
{
    using System.Diagnostics;
    using System.Threading;
    using DraftBot;
    using DraftBot.Protocol;

    [TestClass]
    public class TestRobot
    {
        [TestMethod]
        public void ConnectToRobot()
        {
            using (var robot = new Robot())
            {
                robot.Connect();
                robot.Beep(500);
                robot.Beep(150);

                Assert.IsTrue(robot.WaitForComplete());
                Assert.AreEqual(2, robot.JobsComplete);
            }

            Trace.TraceInformation("All done.");
        }

        [TestMethod]
        public void TestPenUpPenDown()
        {
            using (var robot = new Robot())
            {
                robot.Connect();
                robot.Beep(150);
                robot.PenUp();
                robot.PenDown();
                robot.PenUp();

                Assert.IsTrue(robot.WaitForComplete());
                Assert.AreEqual(4, robot.JobsComplete);
            }

            Trace.TraceInformation("All done.");
        }

        [TestMethod]
        public void TestMoveForwardBack()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                robot.Beep(150);
                robot.Move(100);
                robot.Beep(150);
                robot.Move(-100);

                Assert.IsTrue(robot.WaitForComplete());
                //Assert.AreEqual(4, robot.JobsComplete);
            }

            Trace.TraceInformation("All done.");
        }

        [TestMethod]
        public void TestTurnLeftRight()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                robot.Beep(150);
                robot.TurnLeft(45);
                robot.WaitForComplete();
                Assert.AreEqual(45, robot.Heading);

                robot.Beep(150);
                robot.TurnRight(45);
                robot.WaitForComplete();
                Assert.AreEqual(0, robot.Heading);
            }

            Trace.TraceInformation("All done.");
        }

    }
}
