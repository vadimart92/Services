using System;

namespace Services.Runtime.BodyStructure
{
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using Services.Metadata;

	public class JsonBodyTreeRenderer:IBodyTreeRenderer
	{

		public string Render(BodyTree body) {
			var jObject  = new JObject();
			foreach (var nodeProperty in body) {
				var propValue = GetValue(nodeProperty);
				jObject.Add(propValue);
			}
			return jObject.ToString(Formatting.Indented);
		}

		private JToken GetValue(IBodyTreeItem treeItem) {
			if (treeItem is BodyTreeNodeParameterValue nodeValue) {
				return new JProperty(treeItem.Parameter.Path, GetFormattedObject(nodeValue.Value, nodeValue.Parameter));
			}
			if (treeItem is BodyTreeNode node) {
				return new JProperty(treeItem.Parameter.Path, new JObject(node.Select(GetValue)));
			}
			if (treeItem is BodyTreeArray arrayProp) {
				var items = new JArray(arrayProp.Select(i => {
					if (i.Value is IEnumerable<IBodyTreeItem> treeNode) {
						return new JObject(treeNode.Select(GetValue));
					}
					return (JToken)new JValue(GetFormattedObject(i.Value, i.Parameter));
				}));
				return new JProperty(treeItem.Parameter.Path, items);
			}
			throw new NotSupportedException();
		}

		private object GetFormattedObject(object source, ServiceParameter parameterInfo) {
			// todo: move?
			if (parameterInfo.Format == null) {
				return source;
			}
			string format = "{0:" + parameterInfo.Format + "}";
			return string.Format(format, source);
		}

	}
}
