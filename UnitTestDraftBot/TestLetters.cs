using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestDraftBot
{
    using DraftBot;

    [TestClass]
    public class TestLetters
    {
        private static readonly int block = 12;
        private static readonly int blockDiagnol = (int)(block * Math.Sqrt(2));

        public void DrawA(Robot robot)
        {
            robot.TurnLeft(90);
            robot.PenDown();
            robot.Move(block * 3);
            robot.Turn(-45);
            robot.Move(blockDiagnol);
            robot.Turn(-90);
            robot.Move(blockDiagnol);
            robot.Turn(-45);

            robot.Move(block * 3);
            robot.PenUp();
            robot.Move(-block * 3);
            robot.Turn(-90);
            robot.PenDown();
            robot.Move(block * 2);
            robot.PenUp();
            robot.Move(-block * 2);
            robot.Turn(90);
            robot.Move(block * 3);
            robot.Turn(90);
            robot.Move(block);

            robot.WaitForComplete();
        }

        public void DrawS(Robot robot)
        {
            robot.PenDown();
            robot.Move(block * 2);
            robot.Turn(45);
            robot.Move(blockDiagnol); // diagnal
            robot.Turn(90);
            robot.Move(blockDiagnol);
            robot.Turn(45);
            robot.Move(block);
            robot.Turn(-45);
            robot.Move(blockDiagnol);
            robot.Turn(-90);
            robot.Move(blockDiagnol);
            robot.Turn(-45);
            robot.Move(block * 2);
            robot.PenUp();
            robot.TurnRight(90);
            robot.Move(block * 4);
            robot.TurnLeft(90);
            robot.Move(block);

            robot.WaitForComplete();
        }

        public void DrawE(Robot robot)
        {
            robot.PenDown();
            robot.Move(block * 3);
            robot.PenUp();
            robot.Move(-block * 3);
            robot.PenDown();
            robot.Turn(90);
            robot.Move(block * 2);
            robot.Turn(-90);
            robot.Move(block * 2);
            robot.PenUp();
            robot.Move(-block * 2);
            robot.Turn(90);
            robot.PenDown();
            robot.Move(block * 2);
            robot.Turn(-90);
            robot.Move(block * 3);
            robot.PenUp();
            robot.Turn(-90);
            robot.Move(block * 4);
            robot.Turn(90);
            robot.Move(block);

            robot.WaitForComplete();
        }

        public void DrawN(Robot robot)
        {
            robot.TurnLeft(90);
            robot.PenDown();
            robot.Move(block * 4);
            robot.Turn(-135);
            robot.Move(blockDiagnol * 4);
            robot.Turn(135);
            robot.Move(block * 4);
            robot.PenUp();
            robot.Move(-block * 4);

            robot.Turn(-90);
            robot.Move(block);

            robot.WaitForComplete();
        }

        public void DrawR(Robot robot)
        {
            // This is letter R
            robot.TurnLeft(90);
            robot.PenDown();
            robot.Move(block * 4);
            robot.TurnRight(90);
            robot.MoveForward(block * 2);
            robot.TurnRight(45);
            robot.MoveForward(blockDiagnol);
            robot.TurnRight(90);
            robot.MoveForward(blockDiagnol);
            robot.TurnRight(45);
            robot.MoveForward(block * 2);
            robot.PenUp();
            robot.MoveBack(block * 2);
            robot.TurnLeft(135);
            robot.PenDown();
            robot.MoveForward(blockDiagnol * 2);
            robot.PenUp();
            robot.TurnLeft(45);
            robot.MoveForward(block);

            robot.WaitForComplete();
        }

        public void DrawH(Robot robot)
        {
            // This is letter H
            robot.TurnLeft(90);
            robot.PenDown();
            robot.MoveForward(block * 4);
            robot.PenUp();
            robot.MoveBack(block * 2);
            robot.TurnRight(90);
            robot.PenDown();
            robot.MoveForward(block * 3);
            robot.TurnRight(90);
            robot.MoveForward(block * 2);
            robot.MoveBack(block * 4);
            robot.PenUp();
            robot.MoveForward(block * 4);
            robot.TurnLeft(90);
            robot.MoveForward(block);

            robot.WaitForComplete();
        }

        [TestMethod]
        public void TestDrawSean()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                this.DrawS(robot);
                this.DrawE(robot);
                this.DrawA(robot);
                this.DrawN(robot);
            }
        }

        [TestMethod]
        public void TestDrawSarah()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                this.DrawS(robot);
                this.DrawA(robot);
                this.DrawR(robot);
                this.DrawA(robot);
                this.DrawH(robot);
            }
        }

        [TestMethod]
        public void TestDrawHaha()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                this.DrawH(robot);
                this.DrawA(robot);
                this.DrawH(robot);
                this.DrawA(robot);
            }
        }

        [TestMethod]
        public void TestDrawLetters()
        {
            using (var robot = new Robot())
            {
                robot.Connect();

                Assert.AreEqual(0, robot.Heading);

                this.DrawA(robot);
                Assert.AreEqual(0, robot.Heading);

                this.DrawE(robot);
                Assert.AreEqual(0, robot.Heading);

                this.DrawH(robot);
                Assert.AreEqual(0, robot.Heading);

                this.DrawN(robot);
                Assert.AreEqual(0, robot.Heading);

                this.DrawR(robot);
                Assert.AreEqual(0, robot.Heading);
            }
        }

        [TestMethod]
        public void TestMultiBoxLoop()
        {
            using (var robot = new Robot())
            {
                robot.Connect();
                robot.Scale = 30.0;
                robot.PenDown();
                robot.PenUp();

                for (var ii = 0; ii < 5; ii++)
                {
                    var d = this.MoveBox(robot, 10, 1/150.0);
                    robot.TurnRight(90 + d);
                    robot.Beep(150);
                    robot.PenDown();
                    robot.PenUp();
                }
                

                robot.WaitForComplete();
            }
        }

        [TestMethod]
        public void TestForwardBackLine()
        {
            using (var robot = new Robot())
            {
                robot.Connect();
                robot.Scale = 15.0;
                robot.PenDown();
                robot.PenUp();

                for (int ii = 0; ii < 5; ii++)
                {
                    // Head Box
                    robot.MoveForward(10);
                    robot.MoveBack(10);
                    robot.Beep(150);
                    robot.PenDown();
                    robot.PenUp();
                }


                robot.WaitForComplete();
            }
        }

        public int MoveBox(Robot robot, int len, double correction)
        {
            var d = (int)(correction * len * robot.Scale);
            robot.MoveForward(len);
            robot.TurnRight(90);
            robot.MoveForward(len);
            robot.TurnRight(90 + d);
            robot.MoveForward(len);
            robot.TurnRight(90);
            robot.MoveForward(len);
            return d;
        }

        [TestMethod]
        public void TestDrawCreeper()
        {
            var correction = 1 / 150.0;

            using (var robot = new Robot())
            {
                robot.Connect();
                //robot.Scale = 15.0; // 15 = sheet of paper
                robot.Scale = 30.0; // large post it note

                // Head Box
                robot.PenDown();
                var ofs = this.MoveBox(robot, 10, correction);
                robot.PenUp();

                robot.MoveBack(2);
                robot.TurnRight(90);
                robot.MoveForward(6);

                // Left Eye
                robot.PenDown();
                ofs = this.MoveBox(robot, 2, correction);
                robot.PenUp();

                robot.MoveBack(4);
                robot.TurnRight(90+ofs);

                // Right Eye
                robot.PenDown();
                ofs = this.MoveBox(robot, 2, correction);
                robot.PenUp();

                // Mouth
                robot.PenDown();
                robot.MoveForward(2);
                robot.TurnLeft(90);
                robot.MoveForward(1);
                robot.TurnRight(90);
                robot.MoveForward(1);
                robot.TurnLeft(90);
                robot.MoveForward(3);
                robot.TurnLeft(90);
                robot.MoveForward(1);
                robot.TurnLeft(90);
                robot.MoveForward(1);
                robot.TurnRight(90);
                robot.MoveForward(2);
                robot.TurnRight(90);
                robot.MoveForward(1);
                robot.TurnLeft(90);
                robot.MoveForward(1);
                robot.TurnLeft(90);
                robot.MoveForward(3);
                robot.TurnLeft(90);
                robot.MoveForward(1);
                robot.TurnRight(90);
                robot.MoveForward(1);
                robot.PenUp();

                robot.TurnRight(180);
                robot.MoveForward(7);
                robot.TurnRight(90);
                robot.MoveForward(7);
                robot.TurnRight(90);

                robot.WaitForComplete();
            }
        }
    }
}

