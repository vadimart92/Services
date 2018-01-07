namespace Services.Runtime
{
	using System.Collections.Generic;

	public interface IDataExtractor
	{

		void FillDataValues(IEnumerable<IParameterValue> values, object sourceValue);

	}
	
}