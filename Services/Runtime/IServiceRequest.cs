namespace Services.Runtime
{
	using System.Collections.Generic;

	public interface IServiceRequest
	{

		IReadOnlyDictionary<string, object> ParameterValues { get; set; }

	}

}