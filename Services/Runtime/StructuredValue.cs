namespace Services.Runtime
{
	using System.Collections.Generic;

	public class StructuredValue : IStructuredValue
	{

		public StructuredValue(IDataExtractor dataExtractor, object value) {
			DataExtractor = dataExtractor;
			Value = value;
		}

		public IDataExtractor DataExtractor { get; }

		public object Value { get; }

	}
}