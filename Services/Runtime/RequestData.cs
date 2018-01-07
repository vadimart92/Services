namespace Services.Runtime
{
	using System.Collections.Generic;
	using Services.Runtime.BodyStructure;

	class RequestData: IRequestData
	{

		public RequestData(IEnumerable<HeaderValue> headerValues, BodyTree body) {
			HeaderValues = headerValues;
			Body = body;
		}

		public BodyTree Body { get; }

		public IEnumerable<HeaderValue> HeaderValues { get; }

	}
}