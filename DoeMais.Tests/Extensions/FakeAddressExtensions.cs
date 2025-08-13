using DoeMais.Domain.Entities;
using DoeMais.DTOs.Address;
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
        };
        
        return address;
    }

    public static AddressDto ToDto(this FakeAddress fakeAddress)
    {
        
        var dto = new AddressDto
        {
            AddressId = fakeAddress.AddressId,
            Street = fakeAddress.Street ?? "",
            Complement = fakeAddress.Complement,
            Neighborhood = fakeAddress.Neighborhood ?? "",
            City = fakeAddress.City ?? "",
            State = fakeAddress.State ?? "",
            ZipCode = fakeAddress.ZipCode ?? "",
            IsPrimary = fakeAddress.IsPrimary,
        };
        
        return dto;
    }

    public static AddressSnapshot ToAddressSnapshot(this FakeAddress fakeAddress)
    {
        return new AddressSnapshot()
        {
            City = fakeAddress.City,
            Complement = fakeAddress.Complement,
            Neighborhood = fakeAddress.Neighborhood,
            State = fakeAddress.State,
            Street = fakeAddress.Street,
            ZipCode = fakeAddress.ZipCode
        };
    }
}