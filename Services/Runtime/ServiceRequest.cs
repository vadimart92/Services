namespace Services.Runtime
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Services.Metadata;
	using Services.Runtime.BodyStructure;

	public class ServiceRequest: IServiceRequest
	{

		private readonly ServiceMethod _method;

		public ServiceRequest(ServiceMethod method) {
			_method = method;
		}

		public IReadOnlyDictionary<string, object> ParameterValues { get; set; }

		public Dictionary<Type, IDataExtractor> TypeExtractorsMap { get; set; } = new Dictionary<Type, IDataExtractor>();

		internal IRequestData PrepareRequestData() {
			var headers = new HeaderValue[0];
			var rootNode = new BodyTree();
			foreach (ServiceParameter parameter in _method.Request.Parameters) {
				var parameterName = parameter.Name;
				IBodyTreeItem value;
				if (!ParameterValues.ContainsKey(parameterName)) {
					value = new BodyTreeNodeParameterValue {
						Parameter = parameter,
						Value = parameter.DefValue
					};
				} else {
					var customValue = ParameterValues[parameterName];
					value = GetParameterValue(parameter, customValue);
				}
				rootNode.Add(value);
			}
			return new RequestData(headers, rootNode);
		}

		private IBodyTreeItem GetParameterValue(ServiceParameter parameter, object value) {
			IBodyTreeItem result;
			if (parameter.IsArray) {
				if (value is IStructuredValue structuredValue) {
					object structuredValueCollection = structuredValue.Value;
					var dataExtractor = structuredValue.DataExtractor;
					result = GetStructuredArray(parameter, structuredValueCollection, dataExtractor);
				} else {
					if (value is IEnumerable collection) {
						var array = new BodyTreeArray { Parameter = parameter };
						result = array;
						foreach (object item in collection) {
							var treeItem = CreateSimpleTreeNode(parameter, item);
							array.Add(treeItem);
						}
					} else {
						result = CreateSimpleTreeNode(parameter, value);
					}
				}
			} else if (value is IStructuredValue structuredValue) {
				result = GetParameterValue<BodyTreeNode>(parameter, structuredValue.Value, structuredValue.DataExtractor);
			} else {
				result = CreateSimpleTreeNode(parameter, value);
			}
			return result;
		}

		private BodyTreeArray GetStructuredArray(ServiceParameter parameter, object structuredValueCollection,
				IDataExtractor dataExtractor) {
			var array = new BodyTreeArray {
				Parameter = parameter
			};
			foreach (object item in (IEnumerable)structuredValueCollection) {
				var treeValue = GetParameterValue<BodyTreeObject>(parameter, item, dataExtractor);
				array.Add(treeValue);
			}
			return array;
		}

		private bool TryGetDataExtractor(object value, out IDataExtractor dataExtractor) {
			dataExtractor = null;
			if (value == null || TypeExtractorsMap.Count == 0) {
				return false;
			}
			var type = value.GetType();
			if (TypeExtractorsMap.ContainsKey(type)) {
				dataExtractor = TypeExtractorsMap[type];
				return true;
			}
			return false;
		}

		private IBodyTreeObject CreateSimpleTreeNode(ServiceParameter parameter, object value) {
			if (TryGetDataExtractor(value, out IDataExtractor dataExtractor)) {
				return GetParameterValue<BodyTreeObject>(parameter, value, dataExtractor);
			}
			return new BodyTreeNodeParameterValue { Parameter = parameter, Value = value };
		}

		private T GetParameterValue<T>(ServiceParameter parameter, object value,
				IDataExtractor dataExtractor) where T : BodyTreeNode, new() {
			var innerParameters = parameter.InnerParameters;
			var container = innerParameters.Select(p => new ParameterValue { ParameterInfo = p}).ToList();
			dataExtractor.FillDataValues(container, value);
			var result = new T { Parameter = parameter };
			foreach (ParameterValue parameterValue in container) {
				var parameterInfo = innerParameters.Single(p => p.UId == parameterValue.ParameterInfo.UId);
				var nodeValue = GetParameterValue(parameterInfo, parameterValue.Value);
				result.Add(nodeValue);
			}
			return result;
		}

	}

}
