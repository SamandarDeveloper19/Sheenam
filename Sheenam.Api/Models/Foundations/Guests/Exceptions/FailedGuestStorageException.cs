using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class FailedGuestStorageException : Xeption
    {
        public FailedGuestStorageException(Exception innerException)
            : base(message: "Failed guest storage error occurred, contact support",
                  innerException)
        { }
    }
}
