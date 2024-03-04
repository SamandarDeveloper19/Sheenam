﻿using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests.Exceptions
{
    public class GuestValidationException : Xeption
    {
        public GuestValidationException(Xeption innerException)
            :base(message: "Guest validation errors occurred, fix the errors and try again",
                 innerException)
        { }
    }
}
