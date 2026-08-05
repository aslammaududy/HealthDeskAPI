using HealthDeskAPI.Models;

namespace HealthDeskAPI.Interfaces;

public interface IMappable<TResponse, TModel, TRequest>
{
    TResponse ToResponse(TModel model);
    void UpdateModel(TRequest request, TModel model);
}