namespace Services.Metadata
{
	using System.Collections.Generic;
	using System.Linq;
	using Services.Runtime;

	public class Service
	{

		public string Name { get; set; }

		public ServiceRequest Request { get; set; }

		public IList<ServiceMethod> Methods { get; set; }

		public IServiceClient CreateClient() {
			throw new System.NotImplementedException();
		}

		public IServiceRequest CreateRequest(string methodName) {
			var method = Methods.Single(m => m.Name == methodName);
			return new Runtime.ServiceRequest(method);
		}

	}

}