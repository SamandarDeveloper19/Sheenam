using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            var sqlException = GetSqlError();

            var failedGuestStorageException =
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(someGuest))
                .ThrowsAsync(sqlException);

            // when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(someGuest);

            // then
            await Assert.ThrowsAsync<GuestDependencyException>(() =>
                modifyGuestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(someGuest),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            var dbUpdateException = new DbUpdateException();

            var failedGuestStorageException =
                new FailedGuestStorageException(dbUpdateException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(someGuest))
                .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(someGuest);

            // then
            await Assert.ThrowsAsync<GuestDependencyException>(() =>
                modifyGuestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(someGuest),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedGuestException =
                new LockedGuestException(dbUpdateConcurrencyException);

            var expectedGuestDependencyValidationException =
                new GuestDependencyValidationException(lockedGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(someGuest))
                .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(someGuest);

            // then
            await Assert.ThrowsAsync<GuestDependencyValidationException>(() =>
                modifyGuestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(someGuest),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            var serviceException = new Exception();

            var failedGuestServiceException =
                new FailedGuestServiceException(serviceException);

            var expectedGuestServiceException =
                new GuestServiceException(failedGuestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(someGuest))
                .ThrowsAsync(serviceException);

            // when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(someGuest);

            // then
            await Assert.ThrowsAsync<GuestServiceException>(() =>
                modifyGuestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(someGuest),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
