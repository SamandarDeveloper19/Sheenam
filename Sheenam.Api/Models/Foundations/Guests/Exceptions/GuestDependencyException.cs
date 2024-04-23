using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestDependencyException : Xeption
    {
        public GuestDependencyException(Xeption innerException)
            : base(message: "Guest dependency error occurred, contact support",
                  innerException)
        { }
    }
}
