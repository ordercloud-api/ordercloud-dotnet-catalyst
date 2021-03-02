using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OrderCloud.Catalyst;

namespace OrderCloud.Catalyst.Tests
{
	/// <summary>
	/// You must run a local instance of Redis in order to run these tests.
	/// On Windows, install the latest msi from here: https://github.com/MSOpenTech/redis/releases
	/// </summary>
	[TestFixture]
	public class DifferTests
	{
		private static string CONN_STR = "localhost";
		private static string KEY_PREFIX = "product-sync-test";

		[SetUp, TearDown]
		public Task ClearRedisAsync() => new Differ(CONN_STR, KEY_PREFIX).ClearAllAsync();

		[Test]
		public async Task Differ_works() {
			var differ = new Differ(CONN_STR, KEY_PREFIX);

			// cache is empty. shouldn't be any diffs, but more importantly it shouldn't throw any errors
			(await differ.GetPreviousNotInCurrentAsync()).Should().BeEmpty();
			(await differ.GetCurrentNotInPreviousAsync()).Should().BeEmpty();

			// load day 1. all should be current not in previous, none vice-versa
			await differ.LoadCurrentAsync("day1", GetCsvDay1());
			(await differ.GetPreviousNotInCurrentAsync()).Should().BeEmpty();
			(await differ.GetCurrentNotInPreviousAsync()).Should().HaveCount(7);

			// load day 2. should return correct diffs compared to day 1
			await differ.LoadCurrentAsync("day2", GetCsvDay2());
			(await differ.GetPreviousNotInCurrentAsync())
				.Select(s => s.Substring(0, 4))
				.Should().BeEquivalentTo("0001", "0003", "0006");
			(await differ.GetCurrentNotInPreviousAsync())
				.Select(s => s.Substring(0, 4))
				.Should().BeEquivalentTo("0001", "0006", "0008");
		}

		[Test]
		public async Task Differ_T_works() {
			var differ = new Differ<Product>(CONN_STR, KEY_PREFIX);
			differ.ParseRow = s => new Product {
				Id = s.Split(',')[0],
				Name = s.Split(',')[1],
				Description = s.Split(',')[2],
				Price = decimal.Parse(s.Split(',')[3])
			};
			differ.GetId = p => p.Id;

			// cache is empty. shouldn't be any diffs, but more importantly it shouldn't throw any errors
			var diffs = (await differ.GetDiffsAsync()).ToList();
			diffs.Should().BeEmpty();

			// load day 1. should return all of them as Creates
			await differ.LoadCurrentAsync("day1", GetCsvDay1());
			diffs = (await differ.GetDiffsAsync()).ToList();
			diffs.Should().HaveCount(7).And.OnlyContain(d => d.ChangeType == ChangeType.Create);

			// load day 2. should return correct diffs compared to day 1
			await differ.LoadCurrentAsync("day2", GetCsvDay2());
			diffs = (await differ.GetDiffsAsync()).ToList();
			diffs.Should().HaveCount(4);
			diffs.Should().ContainSingle(d =>
				d.ChangeType == ChangeType.Delete &&
				d.Previous.Id == "0003" &&
				d.Current == null);
			diffs.Should().ContainSingle(d =>
				d.ChangeType == ChangeType.Update &&
				d.Previous.Id == "0001" &&
				d.Previous.Name == d.Current.Name &&
				d.Previous.Description == d.Current.Description &&
				d.Previous.Price == 10m &&
				d.Current.Price == 10.5m);
			diffs.Should().ContainSingle(d =>
				d.ChangeType == ChangeType.Update &&
				d.Previous.Id == "0006" &&
				d.Previous.Name == d.Current.Name &&
				d.Previous.Price == d.Current.Price &&
				d.Previous.Description != d.Current.Description);
			diffs.Should().ContainSingle(d =>
				d.ChangeType == ChangeType.Create &&
				d.Current.Id == "0008" &&
				d.Previous == null);
		}

		class Product
		{
			public string Id { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }
			public decimal Price { get; set; }
		}

		private IEnumerable<string> GetCsvDay1() {
			yield return "0001,Product 1,My price should get updated,10.00";
			yield return "0002,Product 2,rtgsty48gbm458rgnm458ergnr5y98r,11.00";
			yield return "0003,Product 3,I should get deleted,12.00";
			yield return "0004,Product 4,kebnei45y98gerne5y89tji94897dhn,13.00";
			yield return "0005,Product 5,elkfbknhtierergjhyoer59dbsmdrg9,14.00";
			yield return "0006,Product 6,My description should get updated,15.00";
			yield return "0007,Product 7,es;ksdfbioerineiogsdfgindrgsrmg,16.00";
		}

		private IEnumerable<string> GetCsvDay2() {
			yield return "0001,Product 1,My price should get updated,10.50";
			yield return "0002,Product 2,rtgsty48gbm458rgnm458ergnr5y98r,11.00";
			// yield return "0003,Product 3,I should get deleted,12.00";
			yield return "0004,Product 4,kebnei45y98gerne5y89tji94897dhn,13.00";
			yield return "0005,Product 5,elkfbknhtierergjhyoer59dbsmdrg9,14.00";
			yield return "0006,Product 6,My description got updated!,15.00";
			yield return "0007,Product 7,es;ksdfbioerineiogsdfgindrgsrmg,16.00";
			yield return "0008,Product 8,I got created!,16.50";
		}
	}
}
