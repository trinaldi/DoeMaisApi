using DoeMais.Common;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Donation;
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
    
    public async Task<Result<DonationDto?>> CreateDonationAsync(CreateDonationDto dto)
    {
        if (string.IsNullOrEmpty(dto.Title))
            throw new ArgumentNullException();
        
        var donation = dto.ToEntity();
        var result = await _donationRepository.CreateDonationAsync(donation);
        
        return result == null 
            ? new Result<DonationDto?>(ResultType.Error, null, "Donation could not be created.")
            : new Result<DonationDto?>(ResultType.Success, result.ToDto());
    }

    public async Task<Result<List<DonationDto>>?> GetDonationListAsync()
    {
        var donations = await _donationRepository.GetDonationListAsync();
        
        return new Result<List<DonationDto>>(ResultType.Success, donations.Select(d => d.ToDto()).ToList());
    }

    public async Task<Result<DonationDto?>> GetDonationByIdAsync(long donationId)
    {
        var donation = await _donationRepository.GetDonationByIdAsync(donationId);
        
        return donation == null 
            ? new Result<DonationDto?>(ResultType.NotFound, null, "No donation found.") 
            : new Result<DonationDto?>(ResultType.Success, donation.ToDto());
    }

    public async Task<Result<DonationDto?>> UpdateDonationAsync(long donationId, UpdateDonationDto donation)
    {
        if (donationId != donation.DonationId)
        {
            return new Result<DonationDto?>(ResultType.Mismatch, null, "Donation Ids mismatch.");
        }
        var foundDonation = await _donationRepository.GetDonationByIdAsync(donation.DonationId);
        if (foundDonation == null) return new Result<DonationDto?>(ResultType.NotFound, null, "Donation could not be found.");
        
        foundDonation.UpdateFromDto(donation);
        var updatedDonation = await _donationRepository.UpdateDonationAsync(donationId, foundDonation);
        
        return updatedDonation == null 
            ? new Result<DonationDto?>(ResultType.Error, null, "Error updating donation.") 
            : new Result<DonationDto?>(ResultType.Success, updatedDonation.ToDto());
    }

    public async Task<Result<bool>> DeleteDonationAsync(long donationId)
    {
        var success = await _donationRepository.DeleteDonationAsync(donationId);
        
        return success 
            ? new Result<bool>(ResultType.Success, success)
            : new Result<bool>(ResultType.Error, success);
    }
}