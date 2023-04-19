using Microsoft.AspNetCore.Http.HttpResults;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories;

public interface IRegionRepository
{
    Task<List<Region>> GetAllAsync();
    
    Task<Region?> GetByIdAsync(Guid id);

    Task<Region> CreatedAsync(Region region);
    
    Task<Region>? UpdateAsync(Guid id, Region region);
    
    Task<Region>? DeleteAsync(Guid id);
    
    


}