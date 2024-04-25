using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllGuests()
        {
            // given
            IQueryable<Guest> randomGuests = CreateRandomGuests();
            IQueryable<Guest> retrievedGuests = randomGuests;
            IQueryable<Guest> expectedGuests = retrievedGuests.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests())
                .Returns(retrievedGuests);

            // when
            IQueryable<Guest> actualGuests =
                this.guestService.RetrieveAllGuests();

            // then
            expectedGuests.Should().BeEquivalentTo(actualGuests);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
