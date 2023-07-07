using System.ComponentModel.DataAnnotations.Schema;

namespace CCP_HW2_Project_A9210256.Models
{
    [Table("SportRecords")] // 設定資料表名稱為sport_record
    public class SportRecord
    {        
        // 定義運動紀錄資料表紀錄之欄位名稱與資料型態
        public int id { get; set; }             // 定義[運動紀錄id]欄位
        public int time { get; set; }          // 定義[運動時間]欄位
        public string? sportType { get; set; } //  定義[運動種類]欄位
        public int? heartbeat { get; set; }     // 定義[心跳]欄位
        public string? remark { get; set; }     // 定義[備註]欄位
        public DateTime sportDate { get; set; }   // 定義[運動日期]欄位
        
    }
}
