using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using OrderCloud.Catalyst;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class ConfigExtensionTests
	{
		[Test]
		public void can_register_services_by_convention_without_namespace() {
			var container = new ServiceCollection();

			container.AddServicesByConvention(this.GetType().Assembly);

			container.Should().HaveCount(3);
			container.Should().Contain(s => s.ServiceType == typeof(MyServices.ISrv1) && s.ImplementationType == typeof(MyServices.Srv1));
			container.Should().Contain(s => s.ServiceType == typeof(MyServices.ISrv2) && s.ImplementationType == typeof(MyServices.Srv2));
			container.Should().Contain(s => s.ServiceType == typeof(MyOtherServices.ISrv4) && s.ImplementationType == typeof(MyOtherServices.Srv4));
		}

		[Test]
		public void can_register_services_by_convention_with_namespace() {
			var container = new ServiceCollection();

			container.AddServicesByConvention(this.GetType().Assembly, "OrderCloud.Catalyst.Tests.MyServices");

			container.Should().HaveCount(2);
			container.Should().Contain(s => s.ServiceType == typeof(MyServices.ISrv1) && s.ImplementationType == typeof(MyServices.Srv1));
			container.Should().Contain(s => s.ServiceType == typeof(MyServices.ISrv2) && s.ImplementationType == typeof(MyServices.Srv2));
		}
	}

	namespace MyServices
	{
		public interface ISrv1 { }
		public interface ISrv2 { }
		public interface Srv1 : ISrv1 { }
		public interface Srv2 : ISrv1, ISrv2 { }
		public interface Srv3 : ISrv1 { }
	}

	namespace MyOtherServices
	{
		public interface ISrv4 { }
		public interface Srv4 : ISrv4 { }
	}
}
