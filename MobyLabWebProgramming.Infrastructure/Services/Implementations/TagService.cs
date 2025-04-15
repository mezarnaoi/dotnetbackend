using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Infrastructure.Services;

public class TagService : ITagService
{
    private readonly WebAppDatabaseContext _dbContext;

    public TagService(WebAppDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResponse<TagDTO>> GetTag(Guid id)
    {
        var tag = await _dbContext.Tags
            .AsNoTracking()
            .Include(t => t.PlaceTags)
            .ThenInclude(pt => pt.Place)
            .Where(t => t.Id == id)
            .Select(t => new TagDTO(
                t.Id,
                t.Name,
                t.PlaceTags.Select(pt => new PlaceMiniDTO(pt.Place.Id, pt.Place.Name)).ToList()
            ))
            .FirstOrDefaultAsync();

        return (ServiceResponse<TagDTO>)(tag is not null
            ? ServiceResponse<TagDTO>.ForSuccess(tag)
            : ServiceResponse<TagDTO>.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Tag-ul nu a fost găsit.")));
    }

    public async Task<ServiceResponse<PagedResponse<TagDTO>>> GetTags(PaginationSearchQueryParams pagination)
    {
        var query = _dbContext.Tags
            .Include(t => t.PlaceTags)
            .ThenInclude(pt => pt.Place)
            .AsNoTracking();

        var total = await query.CountAsync();

        var results = await query
            .OrderBy(t => t.Name)
            .Skip(pagination.Page * pagination.ElementsPerPage)
            .Take(pagination.ElementsPerPage)
            .Select(t => new TagDTO(
                t.Id,
                t.Name,
                t.PlaceTags.Select(pt => new PlaceMiniDTO(pt.Place.Id, pt.Place.Name)).ToList()
            ))
            .ToListAsync();

        return ServiceResponse<PagedResponse<TagDTO>>.ForSuccess(
            new PagedResponse<TagDTO>(pagination.Page, pagination.ElementsPerPage, total, results));
    }

    public async Task<ServiceResponse> AddTag(TagAddDTO dto)
    {
        var exists = await _dbContext.Tags.AnyAsync(t => t.Name == dto.Name);
        if (exists)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Conflict, "Tag-ul există deja."));
        }

        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Tags.Add(tag);
        await _dbContext.SaveChangesAsync();

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateTag(TagUpdateDTO dto)
    {
        var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == dto.Id);
        if (tag is null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Tag-ul nu a fost găsit."));
        }

        tag.Name = dto.Name;
        tag.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteTag(Guid id)
    {
        var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        if (tag is null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Tag-ul nu a fost găsit."));
        }

        _dbContext.Tags.Remove(tag);
        await _dbContext.SaveChangesAsync();
        return ServiceResponse.ForSuccess();
    }
}
