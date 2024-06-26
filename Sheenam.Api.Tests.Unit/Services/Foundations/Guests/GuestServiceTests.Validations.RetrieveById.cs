﻿using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid guestId = Guid.Empty;
            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Id),
                values: "Id is required");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            // when
            ValueTask<Guest> retrieveGuestByIdTask =
                this.guestService.RetrieveGuestByIdAsync(guestId);

            // then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
                retrieveGuestByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByItIfGuestIsNotFoundAndLogItAsync()
        {
            // given
            Guid someGuestId = Guid.NewGuid();
            Guid inputGuestId = someGuestId;
            Guest noGuest = null;

            var notFoundGuestException =
                new NotFoundGuestException(inputGuestId);

            var expectedGuestValidationException =
                new GuestValidationException(notFoundGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(noGuest);

            // when
            ValueTask<Guest> retrieveGuestByIdTask =
                this.guestService.RetrieveGuestByIdAsync(inputGuestId);

            // then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
                retrieveGuestByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
