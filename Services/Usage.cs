namespace Services
{
	using System;
	using System.Collections.Generic;
	using Fasterflect;
	using Services.Metadata;
	using Services.Runtime;

	public class Usage
	{

		private Service SetupServiceMetadata() {
			return new Service {
				Name = "webinar",
				Request = new Metadata.ServiceRequest {
					Parameters = {
						new ServiceParameter { Name = "subject", ValueType = ParameterType.String},
						new ServiceParameter { Name = "clients", ValueType = ParameterType.Number},
						new ServiceParameter {
							Name = "participants",
							ValueType = ParameterType.Object,
							IsArray = true,
							InnerParameters = {
								new ServiceParameter { Name = "name", ValueType = ParameterType.String},
								new ServiceParameter { Name = "payedDates", ValueType = ParameterType.String },
							}
						},
						new ServiceParameter {
							Name = "dateRange",
							ValueType = ParameterType.Object,
							InnerParameters = {
								new ServiceParameter { Name = "start", ValueType = ParameterType.Date},
								new ServiceParameter { Name = "end", ValueType = ParameterType.Date},
							}
						}
					}
				}
			};
		}

		public void CallServiceMethod() {
			var service = SetupServiceMetadata();
			var client = service.CreateClient();
			var request = service.CreateRequest("createWebinar");
			request.ParameterValues = new Dictionary<string, object> {
				{ "subject", "someSubject"},
				{ "clients", 100 },
				{
					"participants", new [] {
						new {
							Name ="Participant1",
							PayedDates = new[] { "2018-01-05T00:00:00", "2018-01-06T00:00:00" }.WithReflectionExtractor()
						}
					}.WithReflectionExtractor()
				},
				{ "dateRange", new DateRange {Start= DateTime.Now, End=DateTime.Now.AddDays(10)}.WithReflectionExtractor()}
			};
			var response = client.Execute(request);
			/*
			 JSON:
			 {
				"subject": "someSubject",
				"clients": 100,
				"participants": [
					{
						"name": "Participant1",
						"payedDates": [ "2018-01-05", "2018-01-06" ]
					}
				],
				"dateRange": {
					"startDate": /Date(1234567)/,
					"endDate": /Date(1234567)/
				}
			}
			 */
			var resultValues = response.ParameterValues;
		}

	}
	class DateRange//custom data type
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

	}

	public static class ReflectionDataExtractorUtils
	{
		public static IStructuredValue WithReflectionExtractor(this object value) {
			return value.WithDataExtractor(new ReflectionDataExtractor());
		}

	}

	public class ReflectionDataExtractor : IDataExtractor {
		
		public void FillDataValues(IEnumerable<IParameterValue> values, object sourceValue) {
			foreach (IParameterValue dataPropertyValue in values) {
				string propertyName = dataPropertyValue.ParameterInfo.Name;
				var alignedPropertyName = propertyName[0].ToString().ToUpperInvariant() + propertyName.Substring(1);
				var value = sourceValue.GetPropertyValue(alignedPropertyName);
				dataPropertyValue.Value = value;
			}
		}

	}


}