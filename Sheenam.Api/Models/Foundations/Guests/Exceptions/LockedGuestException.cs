using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class LockedGuestException : Xeption
    {
        public LockedGuestException(Exception innerException)
            : base(message: "Guest is locked, please try again",
                  innerException)
        { }
    }
}
