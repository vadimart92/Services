namespace Services.Runtime
{
	using System;
	using System.Collections.Generic;

	public class ParameterValue: IParameterValue
	{

		public IParameterInfo ParameterInfo { get; set; }

		public object Value { get; set; }

		public bool UseDefValue { get; set; }
	}
}