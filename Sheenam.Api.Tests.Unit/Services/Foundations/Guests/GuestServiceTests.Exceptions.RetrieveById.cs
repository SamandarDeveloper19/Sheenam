using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someGuestId = Guid.NewGuid();
            Guid inputGuestId = someGuestId;
            var sqlException = new Exception();

            var failedGuestStorageException =
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(inputGuestId))
                .ThrowsAsync(sqlException);

            // when
            ValueTask<Guest> retrieveGuestByIdTask =
                this.guestService.RetrieveGuestByIdAsync(inputGuestId);

            // then
            await Assert.ThrowsAsync<GuestDependencyException>(() =>
                retrieveGuestByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(inputGuestId),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
