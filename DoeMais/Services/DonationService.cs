using DoeMais.Domain.Entities;
using DoeMais.DTO.Donation;
using DoeMais.Exceptions;
using DoeMais.Extensions;
using DoeMais.Repositories.Interfaces;
using DoeMais.Services.Interfaces;

namespace DoeMais.Services;

public class DonationService : IDonationService
{
    private readonly IDonationRepository _donationRepository;

    public DonationService(IDonationRepository donationRepository)
    {
        _donationRepository = donationRepository;
    }
    
    public async Task<DonationDto?> CreateDonationAsync(CreateDonationDto dto, long userId)
    {
        var donation = dto.ToEntity(userId);
        var result = await _donationRepository.CreateDonationAsync(donation);

        return result?.ToDto();
    }

    public async Task<List<DonationDto>> GetDonationListAsync(long userId)
    {
        var donations = await _donationRepository.GetDonationListAsync(userId);
        return donations.Select(d => d.ToDto()).ToList();
    }

    public async Task<DonationDto?> GetDonationByIdAsync(long id, long userId)
    {
        var donation = await _donationRepository.GetDonationByIdAsync(id, userId);
        
        return donation?.ToDto();
    }

    public async Task<DonationDto?> UpdateDonationAsync(UpdateDonationDto donation, long userId)
    {
        var foundDonation = await _donationRepository.GetDonationByIdAsync(donation.DonationId, userId);
        if (foundDonation == null) throw new NotFoundException<Donation>();

        foundDonation.UpdateFromDto(donation);
        var updatedDonation = await _donationRepository.UpdateDonationAsync(foundDonation, userId);
        
        return updatedDonation?.ToDto();
    }

    public async Task<bool> DeleteDonationAsync(long id, long userId)
    {
        var success = await _donationRepository.DeleteDonationAsync(id, userId);
        
        return success;
    }
}