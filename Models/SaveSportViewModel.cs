using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CCP_HW2_Project_A9210256.Models
{
    public class SaveSportViewModel
    {
        [DisplayName("運動時間(分鐘)：")]
        [Required(ErrorMessage = "請輸入運動時間!")]
        public int time { get; set; } // 運動時間屬性 

        [DisplayName("運動種類：")]
        [Required(ErrorMessage = "請選擇運動種類!")]
        public string? sportType { get; set; } // 運動種類屬性 
        public List<SelectListItem>? sportTypeList { get; set; } // 運動種類清單屬性 

        [DisplayName("心跳：")]
        [Required(ErrorMessage = "請輸入心跳!")]
        public int? heartbeat { get; set; } // 心跳屬性 

        [DisplayName("備註：")]
        public string? remark { get; set; } // 備註屬性 

        [DisplayName("運動日期：")]
        [Required(ErrorMessage = "請選擇運動日期!")]
        [DataType(DataType.Date)]
        public DateTime sportDate { get; set; } // 運動日期屬性 
        public string? result { get; set; } // 執行結果屬性 
    }
}
