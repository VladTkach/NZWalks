using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories;

public class SQLWalkRepository: IWalkRepository
{
    private readonly NZWalksDbContext _dbContext;

    public SQLWalkRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Walk> CreateAsync(Walk walk)
    {
        await _dbContext.Walks.AddAsync(walk);
        await _dbContext.SaveChangesAsync();

        return walk;
    }
    

    public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
        string? sortBy = null, bool asAscending = true, int pageNumber = 1, int pageSize = 1000)
    {

        var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
        
        //Filtering
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(x => x.Name.Contains(filterQuery));
            }
        }
        
        //Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = asAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
            }
            else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
            {
                walks = asAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
            }
        }
        
        // Pagination

        var skipResults = (pageNumber - 1) * pageSize;
        
        return await walks.Skip(skipResults).Take(pageSize).ToListAsync();

        // var walks = await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        //
        // return walks;
    }

    public async Task<Walk>? GetByIdAsync(Guid id)
    {
        var walk = await _dbContext.Walks.Include("Difficulty").Include("Region")
            .FirstOrDefaultAsync(x => x.Id == id);
        return walk;
    }

    public async Task<Walk>? UpdateAsync(Guid id, Walk walk)
    {
        var exist = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

        if (exist == null)
        {
            return null;
        }

        exist.Name = walk.Name;
        exist.Discription = walk.Discription;
        exist.LengthInKm = walk.LengthInKm;
        exist.WalkImageUrl = walk.WalkImageUrl;
        exist.DifficultyId = walk.DifficultyId;
        exist.RegionId = walk.RegionId;

        await _dbContext.SaveChangesAsync();
        return exist;
    }

    public async Task<Walk>? DeleteAsync(Guid id)
    {
        var exist = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

        if (exist == null)
        {
            return null;
        }

        _dbContext.Walks.Remove(exist);
        await _dbContext.SaveChangesAsync();

        return exist;
    }
}