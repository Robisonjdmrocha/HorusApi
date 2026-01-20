using HorusV2.Domain.Entities;

namespace HorusV2.Domain.Data.Repositories;

public interface IStreamingMovementRepository
{
    Task Add(StreamingMovement streamingMovement);
    Task<IEnumerable<StreamingMovement>> GetByStreamingRequest(int streamingRequestId);
}