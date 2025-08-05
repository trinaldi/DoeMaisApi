using DoeMais.Domain.Interfaces;
using DoeMais.Services.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DoeMais.Data.Interceptors;

public class UserIdSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public UserIdSaveChangesInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context;
        if (context == null) return base.SavingChanges(eventData, result);

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified
                        && e.Entity is IUserOwned);

        foreach (var entry in entries)
        {
            var entity = (IUserOwned)entry.Entity;
            entity.UserId = _currentUserService.UserId;
        }

        return base.SavingChanges(eventData, result);
    }
}
