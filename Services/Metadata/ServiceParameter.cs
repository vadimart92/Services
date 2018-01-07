using System.Collections.Generic;

namespace Services.Metadata
{
	using System;
	using System.Diagnostics;
	using Services.Runtime;

	[DebuggerDisplay("Name: {" + nameof(Name) + ("}, Array: {" + nameof(IsArray) + "}"))]
	public class ServiceParameter:IParameterInfo
	{

		private IList<ServiceParameter> _innerParameters;

		public Guid UId { get; set; } = Guid.NewGuid();
		public string Name { get; set; }

		public string Format { get; set; }

		public string Path { get; set; }

		public string DefValue { get; set; }

		public ParameterType ValueType { get; set; }

		public bool IsArray { get; set; }

		public IList<ServiceParameter> InnerParameters {
			get { return _innerParameters ?? (_innerParameters = new List<ServiceParameter>()); }
			set { _innerParameters = value; }
		}

		IEnumerable<IParameterInfo> IParameterInfo.InnerParameters => InnerParameters;

	}

}
