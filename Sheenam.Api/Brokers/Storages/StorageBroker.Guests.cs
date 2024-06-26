﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Guest> guestEntityEntry =
                await broker.Guests.AddAsync(guest);

            await broker.SaveChangesAsync();

            return guestEntityEntry.Entity;
        }

        public IQueryable<Guest> SelectAllGuests()
        {
            using var broker = new StorageBroker(this.configuration);

            return broker.Set<Guest>();
        }

        public async ValueTask<Guest> SelectGuestByIdAsync(Guid guestId)
        {
            using var broker = new StorageBroker(this.configuration);

            return await broker.Guests.FindAsync(guestId);
        }

        public async ValueTask<Guest> UpdateGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.Entry(guest).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return guest;
        }

        public async ValueTask<Guest> DeleteGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.Entry(guest).State = EntityState.Deleted;
            await broker.SaveChangesAsync();

            return guest;
        }
    }
}
