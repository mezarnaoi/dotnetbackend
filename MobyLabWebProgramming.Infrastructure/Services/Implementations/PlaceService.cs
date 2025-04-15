using System.Net;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services;

public class PlaceService : IPlaceService
{
    private readonly WebAppDatabaseContext _dbContext;
    public PlaceService(WebAppDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResponse<PlaceDTO>> GetPlace(Guid id)
    {
        var place = await _dbContext.Places
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Include(p => p.PlaceTags)
                .ThenInclude(pt => pt.Tag)
            .Select(p => new PlaceDTO(p.Id, p.Name, p.Address, p.Description, p.CategoryId, p.PlaceTags.Select(pt => new TagMiniDTO(pt.Tag.Id, pt.Tag.Name)).ToList(), p.Reviews.Select(r => new ReviewDTO(
                r.Id,
                r.Content,
                r.Rating,
                r.UserId,
                r.PlaceId,
                r.CreatedAt
            )).ToList()))
            .FirstOrDefaultAsync();

        return (ServiceResponse<PlaceDTO>)(place != null 
            ? ServiceResponse<PlaceDTO>.ForSuccess(place)
            : ServiceResponse<PlaceDTO>.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Locația nu a fost găsită")));
    }

    public async Task<ServiceResponse<PagedResponse<PlaceDTO>>> GetPlaces(PaginationSearchQueryParams pagination)
    {
        var query = _dbContext.Places
            .AsNoTracking()
            .Include(p => p.PlaceTags)
                .ThenInclude(pt => pt.Tag)
            .Where(p => string.IsNullOrWhiteSpace(pagination.Search) || 
                        p.Name.ToLower().Contains(pagination.Search.ToLower()));

        var total = await query.CountAsync();

        var result = await query
            .OrderBy(p => p.Name)
            .Skip(pagination.Page * pagination.ElementsPerPage)
            .Take(pagination.ElementsPerPage)
            .Select(p => new PlaceDTO(p.Id, p.Name, p.Address, p.Description, p.CategoryId, p.PlaceTags.Select(pt => new TagMiniDTO(pt.Tag.Id,pt.Tag.Name)).ToList(), p.Reviews.Select(r => new ReviewDTO(
                r.Id,
                r.Content,
                r.Rating,
                r.UserId,
                r.PlaceId,
                r.CreatedAt
            )).ToList()))
            .ToListAsync();

        return ServiceResponse<PagedResponse<PlaceDTO>>.ForSuccess(
            new PagedResponse<PlaceDTO>(
                pagination.Page,
                pagination.ElementsPerPage,
                total,
                result
            )
        );
    }

    public async Task<ServiceResponse> AddPlace(PlaceAddDTO place, UserDTO? requestingUser = null)
    {
        if (requestingUser?.Role is not (UserRoleEnum.Admin or UserRoleEnum.Personnel))
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Nu ai permisiunea de a adăuga locații!"));
        }

        var entity = new Place
        {
            Id = Guid.NewGuid(),
            Name = place.Name,
            Address = place.Address,
            Description = place.Description,
            CategoryId = place.CategoryId,
            PlaceTags = place.TagIds.Select(tagId => new PlaceTag
            {
                TagId = tagId
            }).ToList()
        };

        _dbContext.Places.Add(entity);
        await _dbContext.SaveChangesAsync();

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdatePlace(PlaceUpdateDTO place, UserDTO? requestingUser = null)
    {
        if (requestingUser?.Role is not (UserRoleEnum.Admin or UserRoleEnum.Personnel))
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Nu ai permisiunea de a modifica locații!"));
        }

        var entity = await _dbContext.Places
            .Include(p => p.PlaceTags)
            .FirstOrDefaultAsync(p => p.Id == place.Id);

        if (entity == null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Locația nu a fost găsită"));
        }

        entity.Name = place.Name;
        entity.Address = place.Address;
        entity.Description = place.Description;
        entity.CategoryId = place.CategoryId;
        
        _dbContext.PlaceTags.RemoveRange(entity.PlaceTags);
        entity.PlaceTags = place.TagIds.Select(tagId => new PlaceTag
        {
            PlaceId = place.Id,
            TagId = tagId
        }).ToList();

        await _dbContext.SaveChangesAsync();

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeletePlace(Guid id, UserDTO? requestingUser = null)
    {
        if (requestingUser?.Role is not (UserRoleEnum.Admin or UserRoleEnum.Personnel))
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Nu ai permisiunea de a șterge locații!"));
        }

        var entity = await _dbContext.Places.FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Locația nu a fost găsită"));
        }

        _dbContext.Places.Remove(entity);
        await _dbContext.SaveChangesAsync();

        return ServiceResponse.ForSuccess();
    }
}
