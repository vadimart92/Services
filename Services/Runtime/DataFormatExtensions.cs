

namespace Services.Runtime
{
	using System.Collections.Generic;

	public static class DataFormatExtensions
	{

		public static IStructuredValue WithDataExtractor<T>(this T value, IDataExtractor dataExtractor) {
			return new StructuredValue(dataExtractor, value);
		}

	}

}
