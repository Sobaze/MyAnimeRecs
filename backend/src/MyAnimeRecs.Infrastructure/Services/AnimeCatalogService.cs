using Microsoft.EntityFrameworkCore;
using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Application.Models.Catalog; // maybe other models in the future
using MyAnimeRecs.Domain.Entities;

namespace MyAnimeRecs.Infrastructure.Services;

public class AnimeCatalogService(IApplicationDbContext dbContext) : IAnimeCatalogService
{
    
}