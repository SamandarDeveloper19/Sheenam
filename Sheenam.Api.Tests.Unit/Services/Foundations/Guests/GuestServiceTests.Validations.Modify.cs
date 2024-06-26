﻿using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuestIsNullAndLogItAsync()
        {
            // given
            Guest nullGuest = null;
            var nullGuestException = new NullGuestException();

            var expectedGuestValidationException =
                new GuestValidationException(nullGuestException);

            // when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(nullGuest);

            // then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
                modifyGuestTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(It.IsAny<Guest>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuestIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            Guest invalidGuest = new Guest
            {
                FirstName = invalidText
            };

            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Id),
                values: "Id is required");

            invalidGuestException.AddData(
                key: nameof(Guest.FirstName),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.LastName),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.DateOfBirth),
                values: "Date is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Email),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Address),
                values: "Text is required");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            // when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(invalidGuest);

            // then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
                modifyGuestTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(It.IsAny<Guest>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuestDoesNotExistAndLogItAsync()
        {
            // given
            Guest randomGuest = CreateRandomGuest();
            Guest noExistGuest = randomGuest;
            Guest nullGuest = null;

            var notFoundGuestException =
                new NotFoundGuestException(noExistGuest.Id);

            var expectedGuestValidationException =
                new GuestValidationException(notFoundGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(noExistGuest))
                .ThrowsAsync(notFoundGuestException);

            // when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(noExistGuest);

            // then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
                modifyGuestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(noExistGuest),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
