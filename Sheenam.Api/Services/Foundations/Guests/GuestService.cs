using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService : IGuestService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public GuestService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Guest> AddGuestAsync(Guest guest) =>
            TryCatch(async () =>
            {
                ValidateGuestOnAdd(guest);

                return await this.storageBroker.InsertGuestAsync(guest);
            });

        public IQueryable<Guest> RetrieveAllGuests() =>
            TryCatch(() =>
            {
                return this.storageBroker.SelectAllGuests();
            });

        public ValueTask<Guest> RetrieveGuestByIdAsync(Guid guestId) =>
            TryCatch(() =>
            {
                ValidateGuestIdOnRetrieveById(guestId);

                return this.storageBroker.SelectGuestByIdAsync(guestId);
            });

        public async ValueTask<Guest> ModifyGuestAsync(Guest guest)
        {
            return await this.storageBroker.UpdateGuestAsync(guest);
        }
    }
}
