using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldRemoveGuestByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid guestId = randomId;
            Guest randomGuest = CreateRandomGuest();
            Guest storageGuest = randomGuest;
            Guest inputGuest = storageGuest;
            Guest deletedGuest = storageGuest;
            Guest expectedGuest = deletedGuest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(guestId))
                .ReturnsAsync(storageGuest);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteGuestAsync(inputGuest))
                .ReturnsAsync(deletedGuest);

            // when
            Guest actualGuest =
                await this.guestService.RemoveGuestByIdAsync(guestId);

            // then
            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(guestId),
                Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(inputGuest),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
