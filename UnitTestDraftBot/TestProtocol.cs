using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestDraftBot
{
    using DraftBot;
    using DraftBot.Protocol;
        
    [TestClass]
    public class TestProtocol
    {
        [TestMethod]
        public void TestCommandBeep()
        {
            var cmd = new Beep() { Duration = 500 };
            Assert.AreEqual("{\"cmd\":\"beep\", \"arg\":\"500\"}", cmd.ToString());
        }
    }
}
