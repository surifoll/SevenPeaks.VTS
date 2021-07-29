namespace SevenPeaks.VTS.Infrastructure.Interfaces
{
    public interface IAuthenticatedUser
    {
        string UserId { get; }
        string Username { get; }
    }
}
