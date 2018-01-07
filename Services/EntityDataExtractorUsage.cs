namespace Services
{
	using System.Collections.Generic;
	using Services.Runtime;

	class EntityDataExtractorUsage
	{

		public void Samples() {
			var value = new Entity().WithDataExtractor(new EntityDataExtractor());
			var collectionValue = new EntityCollection().WithDataExtractor(new EntityDataExtractor());
			var columnMap = new Dictionary<string,string> {
				{ "Name", "FullName" }
			};
			var valueWithColumnRemap = new Entity().WithDataExtractor(new EntityDataExtractor(columnMap));
		}

	}

	public class EntityDataExtractor:IDataExtractor
	{

		private readonly Dictionary<string, string> _dataMap;

		public EntityDataExtractor(Dictionary<string, string> dataMap = null) {
			_dataMap = dataMap;
		}

		public void FillDataValues(IEnumerable<IParameterValue> values, object sourceValue) {
			var entity = (IEntity)sourceValue;
			foreach (IParameterValue propertyValue in values) {
				string dataKey = propertyValue.ParameterInfo.Name;
				var columnName = _dataMap?.ContainsKey(dataKey) ?? false ? _dataMap[dataKey] : dataKey;
				propertyValue.Value = entity.GetColumnValue(columnName);
			}
		}

	}

	public interface IEntity
	{

		object GetColumnValue(string column);

	}

	public class EntityCollection : LinkedList<IEntity>
	{ }

	public class Entity: IEntity
	{

		public object GetColumnValue(string column) {
			throw new System.NotImplementedException();
		}

	}
}