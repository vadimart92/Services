namespace Services.Runtime
{
	using System;
	using System.Collections.Generic;

	public interface IParameterInfo
	{

		Guid UId { get; set; }
		string Name { get; }
		string Format { get; }
		string DefValue { get; }
		IEnumerable<IParameterInfo> InnerParameters { get; }
	}
}