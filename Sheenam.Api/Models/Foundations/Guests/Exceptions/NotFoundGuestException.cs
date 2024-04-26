using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class NotFoundGuestException : Xeption
    {
        public NotFoundGuestException(Guid guestId)
            : base(message: $"Couldn't find guest with id: {guestId}")
        { }
    }
}
