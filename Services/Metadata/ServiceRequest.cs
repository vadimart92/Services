using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Metadata
{
	public class ServiceRequest
	{

		private readonly Lazy<List<ServiceParameter>> _parameters = new Lazy<List<ServiceParameter>>();

		public IList<ServiceParameter> Parameters {
			get { return _parameters.Value; }
		}

	}
}
