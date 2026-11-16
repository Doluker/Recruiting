using Recruiting.Shared.DTOs;

namespace Recruiting.Client.Interfaces
{
    public interface IDictionaryServiceClient
    {
        Task<ReferenceDataDTO?> GetBootstrapAsync();
    }
}
