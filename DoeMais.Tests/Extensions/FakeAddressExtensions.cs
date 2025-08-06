using DoeMais.Domain.Entities;
using DoeMais.Tests.Domain;

namespace DoeMais.Tests.Extensions;

public static class FakeAddressExtensions
{
    public static Address ToEntity(this FakeAddress fakeAddress)
    {
        var address = new Address
        {
            AddressId = fakeAddress.AddressId,
            Street = fakeAddress.Street ?? "",
            Complement = fakeAddress.Complement,
            Neighborhood = fakeAddress.Neighborhood ?? "",
            City = fakeAddress.City ?? "",
            State = fakeAddress.State ?? "",
            ZipCode = fakeAddress.ZipCode ?? "",
            IsPrimary = fakeAddress.IsPrimary,
            Donations = new List<Donation>()
        };
        
        address.Donations = 
            fakeAddress
            .FakeDonations
            .Select(d => new Donation
            {
                DonationId = d.DonationId,
                AddressId = d.AddressId,
                Title = d.Title,
                Description = d.Description,
                Quantity = d.Quantity,
                Status = d.Status,
                Images = d.Images,
            }).ToList();
        
        return address;
    }
    
}