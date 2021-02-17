using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OrderCloud.Catalyst.Tests
{
    public class ThrottlerTests
    {
        [Test]
        public async Task TestThrollter()
        {
            //arrange. 
            var minWaitTime = 100; //min wait time parameter
            var maxConcurrent = 5;
            var numberOfIterations = 20; 
            var functionWaitTimes = CreateRandomList(100, 200, numberOfIterations); //create a random list of times our test function will take to run.
            //act 
            var elapsedTime = await RunAndTimeThrottler(functionWaitTimes, minWaitTime, maxConcurrent);

            //assert
            Assert.IsTrue(elapsedTime >= minWaitTime * (numberOfIterations - 1)); //ensures it waited the minWaitTime for each function call except the first
            Assert.IsTrue(elapsedTime < functionWaitTimes.Sum()); //assert the total time the throttler took to run is less than the sum of each individual function
        }

        private async Task<long> RunAndTimeThrottler(List<int> waitTimes, int minWaitTime, int maxConcurrent)
        {
            var watch = new Stopwatch();
            watch.Start();
            await Throttler.RunAsync(waitTimes, minWaitTime, maxConcurrent, t => TestAsyncFunction(t));
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public async Task TestAsyncFunction(int number)
        {
            await Task.Delay(number);
        }

        private List<int> CreateRandomList(int min, int max, int length)
        {
            int[] test = new int[length];
            Random randNum = new Random();
            for (int i = 0; i < test.Length; i++)
            {
                test[i] = randNum.Next(min, max);
            }
            return test.ToList();
        }
    }
}
