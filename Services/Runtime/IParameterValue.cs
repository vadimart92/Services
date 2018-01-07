namespace Services.Runtime
{
	using System.Collections.Generic;

	public interface IParameterValue
	{

		IParameterInfo ParameterInfo { get; set; }

		object Value { get; set; }

	}

}