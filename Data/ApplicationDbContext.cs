
using HokkaidoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HokkaidoBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        }
        public DbSet<User> Users { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<User>().ToTable("HokkaidoUsers"); // Create table in SQL Server, name "HokkaidoUsers"
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Nguyễn Văn A", Password = "password123", PhoneNumber = "0905123456", Email = "nguyenvana@example.com", Address = "Hà Nội, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new User { Id = 2, Name = "Trần Thị B", Password = "password123", PhoneNumber = "0905234567", Email = "tranthib@example.com", Address = "TP. Hồ Chí Minh, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new User { Id = 3, Name = "Lê Văn C", Password = "password123", PhoneNumber = "0905345678", Email = "levanc@example.com", Address = "Đà Nẵng, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new User { Id = 4, Name = "Phạm Thị D", Password = "password123", PhoneNumber = "0905456789", Email = "phamthid@example.com", Address = "Cần Thơ, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new User { Id = 5, Name = "Hoàng Văn E", Password = "password123", PhoneNumber = "0905567890", Email = "hoangvane@example.com", Address = "Hải Phòng, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new User { Id = 6, Name = "Đặng Thị F", Password = "password123", PhoneNumber = "0905678901", Email = "dangthif@example.com", Address = "Nha Trang, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new User { Id = 7, Name = "Võ Văn G", Password = "password123", PhoneNumber = "0905789012", Email = "vovang@example.com", Address = "Huế, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new User { Id = 8, Name = "Bùi Thị H", Password = "password123", PhoneNumber = "0905890123", Email = "buithih@example.com", Address = "Quảng Ninh, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new User { Id = 9, Name = "Ngô Văn I", Password = "password123", PhoneNumber = "0905901234", Email = "ngovani@example.com", Address = "Vũng Tàu, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new User { Id = 10, Name = "Đỗ Thị K", Password = "password123", PhoneNumber = "0906012345", Email = "dothik@example.com", Address = "Biên Hòa, Việt Nam", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow }
            );
        }
    }
}
