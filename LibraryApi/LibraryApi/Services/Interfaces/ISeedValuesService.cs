using LibraryApi.Helpers;

namespace LibraryApi.Services.Interfaces;

public interface ISeedValuesService
{
    Task<Result<bool>> AddSeedAdmin();
}