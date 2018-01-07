namespace Tests
{
	using System;
	using System.Collections.Generic;
	using NUnit.Framework;
	using Services;
	using Services.Metadata;
	using Services.Runtime.BodyStructure;
	using Runtime = Services.Runtime;
	using ServiceRequest = Services.Metadata.ServiceRequest;

	[TestFixture]
	public class RequestTest
	{

		[Test]
		public void GetRequestData() {
			var method = new ServiceMethod {
				 Request = new ServiceRequest {
					 Parameters = {
						 new ServiceParameter{Name = "subject", Path = "subject"},
						 new ServiceParameter{Name = "simpleDefValue", Path = "defValueTest", DefValue = "defValue"},
						 new ServiceParameter{
							 Name = "simpleObj",
							 Path = "simpleObj",
							 InnerParameters = {
								 new ServiceParameter{Name = "name", Path = "simleObjName"},
								 new ServiceParameter{Name = "age", Path = "simleObjAge"}
							}
						 },
						 new ServiceParameter{
							 Name = "simpleArray",
							 Path = "simpleArray",
							 IsArray = true
						 },
						 new ServiceParameter{
							 Name = "simpleObjectArray",
							 Path = "simpleObjectArray",
							 IsArray = true,
							 InnerParameters = {
								 new ServiceParameter{Name = "start", Path = "start", Format = "yyyy-MM-dd", ValueType = ParameterType.Date},
								 new ServiceParameter{Name = "end", Path = "end", Format = "yyyy-MM", ValueType = ParameterType.Date}
							}
						 },
						 new ServiceParameter{
							 Name = "nestedObjArray",
							 Path = "nestedObjArray",
							 IsArray = true,
							 InnerParameters = {
								 new ServiceParameter{Name = "name", Path = "name"},
								 new ServiceParameter {
									 Name = "items",
									 Path = "items",
									 IsArray = true,
									 InnerParameters = {
										 new ServiceParameter{Name = "name", Path = "itemName"}
									 }
								 },
							 }
						 },
						 new ServiceParameter{
							 Name = "wellKnownTypes",
							 Path = "wellKnownTypes",
							 IsArray = true,
							 InnerParameters = {
								 new ServiceParameter{Name = "name", Path = "name"},
								 new ServiceParameter {
									 Name = "items",
									 Path = "items",
									 IsArray = true,
									 InnerParameters = {
										 new ServiceParameter{Name = "name", Path = "itemName"}
									 }
								 },
							 }
						 }
					}
				}
			};
			var request = new Runtime.ServiceRequest(method) {
				ParameterValues = new Dictionary<string, object> {
					{ "subject", "hello world" },
					{ "simpleObj", new {Name="testSimpleObjName", Age= 12}.WithReflectionExtractor() },
					{ "simpleArray", new [] {1,2,3} },
					{ "simpleObjectArray", new [] {
						new {Start=DateTime.Now, End=DateTime.Now.AddDays(1)},
						new {Start=DateTime.Now.AddDays(2), End=DateTime.Now.AddDays(3)},
					}.WithReflectionExtractor() },
					{ "nestedObjArray", new [] {
						new {Name="ArrayObject1", Items=new[]{new {Name="item1"}, new {Name="item2"} }.WithReflectionExtractor()},
						new {Name="ArrayObject2", Items=new[]{new {Name="item3"}, new {Name="item4"}, new {Name="item5"} }.WithReflectionExtractor()},
					}.WithReflectionExtractor()},
					{ "wellKnownTypes", new[] {
						new EntityCollection{Name="ArrayObject1", Items= {
							new Entity("item1"),
							new Entity("item2")
						}},
						new EntityCollection{Name="ArrayObject2", Items= {
							new Entity("item3"),
							new Entity("item4"),
							new Entity("item5")
						}}
					}}
				}
			};
			request.TypeExtractorsMap[typeof(EntityCollection)] = new ReflectionDataExtractor();
			request.TypeExtractorsMap[typeof(Entity)] = new ReflectionDataExtractor();
			var requestData = request.PrepareRequestData();
			var renderer = new JsonBodyTreeRenderer();
			var body = renderer.Render(requestData.Body);
		}


		public class Entity
		{

			public Entity(string name) {
				Name = name;
			}
			public string Name { get; }

		}

		class EntityCollection
		{
			
			public string Name { get; set; }

			public List<Entity> Items { get; set; } = new List<Entity>();

		}
	}
}
