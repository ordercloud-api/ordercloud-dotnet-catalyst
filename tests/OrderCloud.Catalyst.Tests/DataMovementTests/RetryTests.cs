using AutoFixture;
using AutoFixture.NUnit3;
using Flurl.Http;
using Flurl.Http.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class RetryTests
	{
		private HttpTest _httpTest;
		private Fixture _fixture;

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
			_httpTest = new HttpTest();
			_httpTest.RespondWith("server error", 500);
		}

		[TearDown]
		public void TearDown()
		{
			_httpTest.Dispose();
		}

		[Test]
		public async Task succeed_after_x_tries_works_as_expected()
		{
			var x = _fixture.Create<int>() % 10;
			var tries = _fixture.Create<int>() % 10;
			var action = new SucceedAfterXTries(x);
			for (int i = 0; i < tries; i++)
			{
				var didSucceed = false;
				try
				{
					didSucceed = await action.Run();
				}
				catch { }
				if (i >= x)
				{
					Assert.IsTrue(didSucceed);
				}
				else
				{
					Assert.IsFalse(didSucceed);
				}
			}
		}

		[Test]
		[TestCase(0)]
		[AutoData]
		public async Task succeeds_if_retry_schedule_is_not_exceeded(int retryCount)
		{
			retryCount = retryCount % 10;
			var action = new SucceedAfterXTries(retryCount);
			var policy = new RetryPolicy(Enumerable.Repeat(0, retryCount).ToList());
			var result = await policy.RunWithRetries(() => action.Run());
			Assert.IsTrue(result);
		}


		[Test]
		[TestCase(0)]
		[AutoData]
		public async Task fails_if_retry_schedule_is_exceeded(int retryCount)
		{
			retryCount = retryCount % 10;
			var action = new SucceedAfterXTries(retryCount + 1);
			var policy = new RetryPolicy(Enumerable.Repeat(0, retryCount).ToList());
			Assert.ThrowsAsync<FlurlHttpException>(async () => await policy.RunWithRetries(() => action.Run()));
		}


		[Test]
		[AutoData]
		public async Task cummulative_pause_before_success_is_correct(int retryCount)
		{
			retryCount = retryCount % 5;
			var action = new SucceedAfterXTries(retryCount);
			var policy = new RetryPolicy(Enumerable.Repeat(500, retryCount).ToList());
			var watch = new Stopwatch();
			watch.Start();
			var result = await policy.RunWithRetries(() => action.Run());
			watch.Stop();
			Assert.GreaterOrEqual(watch.ElapsedMilliseconds, retryCount * 500); ;
		}
	}

	public class SucceedAfterXTries
	{
		private readonly int _succeedAfter;
		private int _currentRequest = 0;
		public SucceedAfterXTries(int x)
		{
			_succeedAfter = x;
		}

		public async Task<bool> Run()
		{
			_currentRequest++;
			if (_currentRequest <= _succeedAfter)
			{
				await "http://always.fails.url.com/".GetJsonAsync();
			}
			return true;
		}
	}
}
