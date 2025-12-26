using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries
{
    /// <summary>
    /// Query to list available vehicles.
    /// </summary>
    public class ListAvailableVehiclesQuery : IRequest<ListAvailableVehiclesQueryResponse>
    {
    }
}
