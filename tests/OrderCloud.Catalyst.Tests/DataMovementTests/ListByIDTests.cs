using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using OrderCloud.SDK;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests.DataMovementTests
{
	[TestFixture]
	public class ListByIDTests
	{
		private class ListByIDMock
		{
			public List<string> Reqeusts = new List<string>();

			public async Task<ListPage<Supplier>> Method(string filterValue)
			{
				Reqeusts.Add(filterValue);
				return await Task.FromResult(new ListPage<Supplier>()
				{
					Items = new List<Supplier>()
				});
			}

		}

		[Test]
		public async Task empty_list_returns_empty_list()
		{
			// Arrange 
			var ids = new List<string>();
			var mock = new ListByIDMock();
			// Act
			var result = await ListAllHelper.ListByIDAsync(ids, mock.Method);
			// Assert
			Assert.AreEqual(result.Count, 0);
			Assert.AreEqual(mock.Reqeusts.Count, 0);
		}

		[Test]
		public async Task one_id_creates_one_request()
		{
			// Arrange 
			var ids = new List<string>() { "my-cool-id" };
			var mock = new ListByIDMock();

			// Act
			await ListAllHelper.ListByIDAsync(ids, mock.Method);
			// Assert
			Assert.AreEqual(mock.Reqeusts.Count, 1);
			Assert.AreEqual(mock.Reqeusts[0], "my-cool-id");
		}

		[Test]
		public async Task multiple_ids_creates_one_request()
		{
			// Arrange 
			var ids = new List<string>() { "a", "b", "c", "d", "e" };
			var mock = new ListByIDMock();

			// Act
			await ListAllHelper.ListByIDAsync(ids, mock.Method);
			// Assert
			Assert.AreEqual(mock.Reqeusts.Count, 1);
			Assert.AreEqual(mock.Reqeusts[0], "a|b|c|d|e");
		}

		[Test]
		public async Task more_than_100_ids_creates_multiple_requests()
		{
			// Arrange 
			var ids = new List<string>();
			for (var i = 0; i < 350; i++)
			{
				ids.Add(i.ToString());
			}

			var mock = new ListByIDMock();

			// Act
			await ListAllHelper.ListByIDAsync(ids, mock.Method);

			// Assert
			Assert.AreEqual(mock.Reqeusts.Count, 4);
			Assert.AreEqual(mock.Reqeusts[0], string.Join("|", ids.Take(100)));
			Assert.AreEqual(mock.Reqeusts[3], string.Join("|", ids.Skip(300)));
		}

		[Test]
		public async Task duplicate_ids_are_ignored()
		{
			// Arrange 
			var ids = Enumerable.Repeat("repeated-id", 350).ToList();
			var mock = new ListByIDMock();

			// Act
			await ListAllHelper.ListByIDAsync(ids, mock.Method);

			// Assert
			Assert.AreEqual(mock.Reqeusts.Count, 1);
			Assert.AreEqual(mock.Reqeusts[0], "repeated-id");
		}

		[Test]
		public async Task huge_ids_become_separate_requests()
		{
			// Arrange. Create 10 strings of length 900. This should be 5 requests.
			var ids = new List<string>();
			for (var i = 0; i < 10; i++)
			{
				var id = string.Join("", Enumerable.Repeat(i.ToString(), 900));
				var a = id.Length;
				ids.Add(id);
			}

			var mock = new ListByIDMock();

			// Act
			await ListAllHelper.ListByIDAsync(ids, mock.Method);

			// Assert
			Assert.AreEqual(mock.Reqeusts.Count, 5);
			Assert.AreEqual(mock.Reqeusts[0], string.Join("|", ids.Take(2)));
			Assert.AreEqual(mock.Reqeusts[4], string.Join("|", ids.Skip(8)));
		}
	}
}
