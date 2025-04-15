using System.Net;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly WebAppDatabaseContext _dbContext;

    public CategoryService(WebAppDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResponse<CategoryDTO>> GetCategory(Guid id)
    {
        var category = await _dbContext.Categories
            .Include(c => c.Places)
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoryDTO(
                c.Id,
                c.Name,
                c.Places.Select(p => new PlaceMiniDTO(p.Id, p.Name)).ToList()
            ))
            .FirstOrDefaultAsync();

        return (ServiceResponse<CategoryDTO>)(category is not null
            ? ServiceResponse<CategoryDTO>.ForSuccess(category)
            : ServiceResponse<CategoryDTO>.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Categoria nu a fost găsită.")));
    }

    public async Task<ServiceResponse<PagedResponse<CategoryDTO>>> GetCategories(PaginationSearchQueryParams pagination)
    {
        var query = _dbContext.Categories
            .Include(c => c.Places)
            .AsNoTracking()
            .Where(c => string.IsNullOrWhiteSpace(pagination.Search) ||
                        c.Name.ToLower().Contains(pagination.Search.ToLower()));

        var total = await query.CountAsync();

        var result = await query
            .OrderBy(c => c.Name)
            .Skip(pagination.Page * pagination.ElementsPerPage)
            .Take(pagination.ElementsPerPage)
            .Select(c => new CategoryDTO(
                c.Id,
                c.Name,
                c.Places.Select(p => new PlaceMiniDTO(p.Id, p.Name)).ToList()
            ))
            .ToListAsync();

        return ServiceResponse<PagedResponse<CategoryDTO>>.ForSuccess(
            new PagedResponse<CategoryDTO>(pagination.Page, pagination.ElementsPerPage, total, result));
    }

    public async Task<ServiceResponse> AddCategory(CategoryAddDTO dto)
    {
        
        // Verifică dacă există deja o categorie cu același nume (ignorând capitalizarea)
        var existingCategory = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.Name.ToLower());

        // Dacă există, returnează eroare
        if (existingCategory != null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.BadRequest, "Categoria cu acest nume deja există."));
        }
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateCategory(CategoryUpdateDTO dto)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == dto.Id);

        if (category is null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Categoria nu a fost găsită."));
        }

        category.Name = dto.Name;
        await _dbContext.SaveChangesAsync();

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteCategory(Guid id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Categoria nu a fost găsită."));
        }

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();

        return ServiceResponse.ForSuccess();
    }
}
