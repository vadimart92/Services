namespace Services.Runtime
{
	using System.Collections.Generic;

	public interface IServiceResponse
	{

		IReadOnlyDictionary<string, object> ParameterValues { get; }

	}
}