using DoeMais.Domain.Entities;
using DoeMais.Domain.OwnedTypes;
using DoeMais.Infrastructure;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;

namespace DoeMais.Tests.Helpers.Factories;

public static class TestDataFactory
{
    public static async Task<User> CreatePersistedUserAsync(AppDbContext context)
    {
        var fakeUser = FakeUser.Create().ToEntity();
        if (fakeUser.UserId == 0) fakeUser.UserId = 1;

        context.Users.Add(fakeUser);
        await context.SaveChangesAsync();

        return fakeUser;
    }
    
    public static async Task<(User user, Donation donation)> CreateUserWithDonationAsync(AppDbContext context)
    {
        var fakeUser = FakeUser.Create().ToEntity();

        if (fakeUser.UserId == 0) fakeUser.UserId = 1;

        var fakeDonation = FakeDonation.Create().WithAddress().ToEntity();
        fakeDonation.UserId = fakeUser.UserId;

        context.Users.Add(fakeUser);
        context.Donations.Add(fakeDonation);

        await context.SaveChangesAsync();

        return (fakeUser, fakeDonation);
    }

    public static async Task<Donation> CreatePersistedDonationAsync(AppDbContext context)
    {
        var donation = FakeDonation.Create().WithAddress().ToEntity();

        context.Donations.Add(donation);
        await context.SaveChangesAsync();

        return donation;
    }

    public static async Task<List<Donation>> CreateDonationListAsync(AppDbContext context)
    {
        const int userId = 1;
        var donations = Enumerable.Range(1, 3)
            .Select(_ =>
            {
                var donation = FakeDonation.Create().WithAddress().ToEntity();
                donation.UserId = userId;
                donation.PickupAddress = FakeAddress.Create().ToEntity();
                donation.DeliveryAddress = FakeAddress.Create().ToEntity();
                return donation;
            })
            .ToList();
        
        context.Donations.AddRange(donations);
        await context.SaveChangesAsync();

        return donations;
    }

}
