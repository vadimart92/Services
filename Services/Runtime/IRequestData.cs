namespace Services.Runtime
{
	using System.Collections.Generic;
	using Services.Runtime.BodyStructure;

	public interface IRequestData
	{

		BodyTree Body { get; }

		IEnumerable<HeaderValue> HeaderValues { get; }

	}

}