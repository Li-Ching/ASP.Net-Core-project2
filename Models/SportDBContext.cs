using Microsoft.EntityFrameworkCore;

namespace CCP_HW2_Project_A9210256.Models
{
    public class SportDBContext : DbContext
    {
        // 資料庫內容類別TallybookDBContext之建構式
        public SportDBContext(DbContextOptions<SportDBContext> options)
        : base(options)
        {
        }
        // 定義SportRecords資料表實體集，資料表中每一筆紀錄對應到SportRecord類別建立的物件
        public DbSet<SportRecord> SportRecords { get; set; }
        // 定義SportTypes資料表實體集，資料表中每一筆紀錄對應到SportType類別建立的物件
        public DbSet<SportType> SportTypes { get; set; }
    }
}
