namespace Services.Runtime
{
	using System.Collections;
	using System.Collections.Generic;

	public interface IStructuredValue
	{

		IDataExtractor DataExtractor { get; }

		object Value { get; }

	}
}