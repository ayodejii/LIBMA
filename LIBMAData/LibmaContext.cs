using LIBMAData.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace LIBMAData
{
    public class LibmaContext: DbContext
    {
        public LibmaContext(DbContextOptions options): base(options) { }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Checkout> Checkouts { get; set; }
        public virtual DbSet<CheckoutHistory> CheckoutHistories { get; set; }
        public virtual DbSet<LibraryBranch> LibraryBranches { get; set; }
        public virtual DbSet<BranchHours> BranchHours { get; set; }
        public virtual DbSet<LibraryCard> LibraryCards { get; set; }
        public virtual DbSet<Patron> Patrons { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<LibraryAsset> LibraryAssets { get; set; }
        public virtual DbSet<Hold> Holds { get; set; }
    }
}
