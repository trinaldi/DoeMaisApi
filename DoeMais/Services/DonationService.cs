using DoeMais.Common;
using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;
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
    
    public async Task<Result<DonationDto?>> CreateDonationAsync(CreateDonationDto dto, long userId)
    {
        var donation = dto.ToEntity(userId);
        var result = await _donationRepository.CreateDonationAsync(donation);
        
        return result == null 
            ? new Result<DonationDto?>(ResultType.Error, null, "Donation could not be created.")
            : new Result<DonationDto?>(ResultType.Success, result.ToDto());
    }

    public async Task<Result<List<DonationDto>>?> GetDonationListAsync(long userId)
    {
        var donations = await _donationRepository.GetDonationListAsync(userId);
        return new Result<List<DonationDto>>(ResultType.Success, donations.Select(d => d.ToDto()).ToList());
    }

    public async Task<Result<DonationDto?>> GetDonationByIdAsync(long id, long userId)
    {
        var donation = await _donationRepository.GetDonationByIdAsync(id, userId);
        return donation == null 
            ? new Result<DonationDto?>(ResultType.NotFound, null, "No donation found.") 
            : new Result<DonationDto?>(ResultType.Success, donation?.ToDto());
    }

    public async Task<Result<DonationDto?>> UpdateDonationAsync(UpdateDonationDto donation, long userId)
    {
        var foundDonation = await _donationRepository.GetDonationByIdAsync(donation.DonationId, userId);
        if (foundDonation == null) return new Result<DonationDto?>(ResultType.NotFound, null, "Donation could not be found.");

        foundDonation.UpdateFromDto(donation);
        var updatedDonation = await _donationRepository.UpdateDonationAsync(foundDonation, userId);
        return updatedDonation == null 
            ? new Result<DonationDto?>(ResultType.Error, null, "Error updating donation.") 
            : new Result<DonationDto?>(ResultType.Success, updatedDonation.ToDto());
    }

    public async Task<Result<bool>> DeleteDonationAsync(long id, long userId)
    {
        var success = await _donationRepository.DeleteDonationAsync(id, userId);
        return success 
            ? new Result<bool>(ResultType.Success, success)
            : new Result<bool>(ResultType.Error, success);
    }
}