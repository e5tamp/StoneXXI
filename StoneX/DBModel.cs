using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace StoneX
{
    public partial class DBModel : DbContext
    {
        public DBModel()
            : base("name=DBModel")
        {
        }

        public virtual DbSet<Item> Currency { get; set; }
        public virtual DbSet<CurrencyDaily> CurrencyDaily { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.ISO_Num_Code)
                .IsFixedLength();

            modelBuilder.Entity<Item>()
                .Property(e => e.ISO_Char_Code)
                .IsFixedLength();

            modelBuilder.Entity<Item>()
                .Property(e => e.ParentCode)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.EngName)
                .IsUnicode(false);

            modelBuilder.Entity<CurrencyDaily>()
                .Property(e => e.value)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CurrencyDaily>()
                .Property(e => e.currency_id)
                .IsUnicode(false);
        }
    }
}
