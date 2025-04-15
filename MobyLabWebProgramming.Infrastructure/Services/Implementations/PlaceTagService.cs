using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Infrastructure.Services;

public class PlaceTagService : IPlaceTagService
{
    private readonly WebAppDatabaseContext _dbContext;

    public PlaceTagService(WebAppDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResponse> AddTagToPlace(PlaceTagDTO dto)
    {
        var exists = await _dbContext.PlaceTags
            .AnyAsync(pt => pt.PlaceId == dto.PlaceId && pt.TagId == dto.TagId);

        if (exists)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Conflict, "Tag-ul este deja asociat acestui loc."));
        }

        var placeExists = await _dbContext.Places.AnyAsync(p => p.Id == dto.PlaceId);
        var tagExists = await _dbContext.Tags.AnyAsync(t => t.Id == dto.TagId);

        if (!placeExists || !tagExists)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Locul sau tag-ul nu există."));
        }

        _dbContext.PlaceTags.Add(new PlaceTag
        {
            PlaceId = dto.PlaceId,
            TagId = dto.TagId
        });

        await _dbContext.SaveChangesAsync();
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> RemoveTagFromPlace(PlaceTagDTO dto)
    {
        var entry = await _dbContext.PlaceTags
            .FirstOrDefaultAsync(pt => pt.PlaceId == dto.PlaceId && pt.TagId == dto.TagId);

        if (entry is null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Asocierea nu a fost găsită."));
        }

        _dbContext.PlaceTags.Remove(entry);
        await _dbContext.SaveChangesAsync();
        return ServiceResponse.ForSuccess();
    }
}
