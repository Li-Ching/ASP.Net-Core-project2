using System.ComponentModel.DataAnnotations.Schema;

namespace CCP_HW2_Project_A9210256.Models
{
    [Table("SportTypes")]
    public class SportType
    {
        // 定義運動種類資料表紀錄之欄位名稱與資料型態
        public int id { get; set; } // 定義[運動種類id]欄位
        public string? sportType { get; set; } // 定義[運動種類]欄位
    }
}
