using BadProject.Helpers;
using NUnit.Framework;
using System;
using System.Threading;


namespace BadProject.Tests.Helpers
{
    class RetryHelperTests
    {
        RetryHelper _retryHelper;

        [SetUp]
        public void Setup()
        {
            _retryHelper = new RetryHelper(10,3);
        }

        [Test]
        public void ExcuteFirstRunTest()
        {
            string actualResult = string.Empty;

            //Action 
            _retryHelper.Execute(
                () =>   "First Run" , 
                (string actResult) =>  actualResult = actResult ,
                 (Exception ex) => { });

            //Assert
            Assert.AreEqual("First Run", actualResult);
        }

        [Test]
        public void ExcuteFirstThrowExceptionSecondRunProvideResultTest()
        {
            int runIndex= 0;
            string actualResult = string.Empty;

            //Action 
            _retryHelper.Execute(
                ()  => { if (runIndex == 1) { return "Second Run"; } throw new Exception("runtime exception"); },
                (string actResult) => actualResult = actResult,
                 (Exception ex) => { runIndex++; });

            //Assert
            Assert.AreEqual(1, runIndex);
            Assert.AreEqual("Second Run", actualResult);
        }
    }
}
