namespace Services.Runtime
{

	public interface IServiceClient
	{

		IServiceResponse Execute(IServiceRequest request);

	}

}