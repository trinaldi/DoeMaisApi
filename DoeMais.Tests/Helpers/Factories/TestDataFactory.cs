using DoeMais.Data;
using DoeMais.Domain.Entities;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;

namespace DoeMais.Tests.Helpers.Factories;

public static class TestDataFactory
{
    public static async Task<(User user, Address address)> CreateUserWithAddressAsync(AppDbContext context)
    {
        var fakeUser = FakeUser.Create().ToEntity();

        if (fakeUser.UserId == 0) fakeUser.UserId = 1;

        var fakeAddress = FakeAddress.Create().ToEntity();
        fakeAddress.UserId = fakeUser.UserId;

        context.Users.Add(fakeUser);
        context.Addresses.Add(fakeAddress);

        await context.SaveChangesAsync();

        return (fakeUser, fakeAddress);
    }

    public static async Task<List<Address>> CreateAddressesAsync(AppDbContext context)
    {
        const int userId = 1;
        var addresses = Enumerable.Range(1, 10)
            .Select(_ =>
            {
                var add = FakeAddress.Create().ToEntity();
                add.UserId = userId;
                return add;
            })
            .ToList();
        context.Addresses.AddRange(addresses);
        await context.SaveChangesAsync();

        return addresses;
    }

    public static async Task<Address> CreatePersistedAddressAsync(AppDbContext context)
    {
        var address = FakeAddress.Create().ToEntity();

        context.Addresses.Add(address);
        await context.SaveChangesAsync();

        return address;
    }

    // public static async Task<User> CreatePersistedUserAsync(AppDbContext context)
    // {
    //     var user = FakeUser.Create().ToEntity();

    //     context.Users.Add(user);
    //     await context.SaveChangesAsync();

    //     return user;
    // }

    // public static async Task<Donation> CreatePersistedDonationAsync(AppDbContext context)
    // {
    //     var donation = FakeDonation.Create().ToEntity();

    //     context.Donations.Add(donation);
    //     await context.SaveChangesAsync();

    //     return donation;
    // }
}
