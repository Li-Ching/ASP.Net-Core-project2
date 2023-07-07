using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using CCP_HW2_Project_A9210256.Models;

namespace CCP_HW2_Project_A9210256.Controllers
{
    public class SportRecordController : Controller
    {
        private readonly SportDBContext db; // 宣告SportDBContext物件變數db

        #region  SportRecordController類別的建構式    
        public SportRecordController(SportDBContext context)
        {
            // 將SportDBContext物件傳入的參數值指定給db變數 (依賴注入DI)
            db = context;

            int count = db.SportTypes.Count(); // 取得運動種類資料表的紀錄數

            //===== 若運動種類資料表(SportTypes)沒有任何紀錄，則將10個預先設好的運動種類存入運動種類資料表中
            if (count == 0)
            {
                //初始運動種類陣列
                string[] initialSportTypes = { "跑步", "籃球", "羽球", "桌球", "瑜珈", "健身", "跳舞", "游泳", "跳繩", "其他" };
                int length = initialSportTypes.Length; // 取得初始運動種類陣列的長度

                // 建立運動種類紀錄陣列物件
                List<SportType> sportTypeList = new List<SportType>(length);
                //若沒有任何運動種類，則利用迴圈方式，將初始運動種類逐一存到SportTypes資料表中
                for (int i = 0; i < length; i++)
                {
                    // 將initialSportTypes陣列第i個元素存入sportTypeArray物件第i個元素的sportType屬性
                    SportType sportTypeObject = new SportType(); // 建立運動種類物件
                    sportTypeObject.sportType = initialSportTypes[i];
                    sportTypeList.Add(sportTypeObject);
                }
                // 將sportType物件新增到SportTypes資料表中
                db.SportTypes.AddRange(sportTypeList.ToArray());
                // 儲存變更到資料庫 
                db.SaveChanges();
            }
        }
        #endregion

        #region 顯示儲存運動紀錄頁面的動作方法
        // 沒有HTTP動詞標示，預設只接受GET請求
        // 路由：/LiteTallybook/SaveSport
        public IActionResult SaveSport()
        {
            // 建立一個SaveSportViewModel物件
            SaveSportViewModel saveSportViewModel = new SaveSportViewModel();
            // 利用LINQ語法取出所有的運動種類紀錄
            var result = from a in db.SportTypes
                         select a;
            // 建立一個字串清單物件sportTypeList
            List<SelectListItem> sportTypeList = new List<SelectListItem>();
            // 將回傳的運動種類一一取出，然後存入字串清單sportTypeList
            int length = result.Count(); // 取得回傳的運動種類紀錄數
            for (int i = 0; i < length; i++)
            {
                string? text = result.AsEnumerable().ElementAt(i).sportType;
                sportTypeList.Add(new SelectListItem { Text = text });
            }
            // 將sportTypeList存入saveSportViewModel物件的sportTypeList屬性中
            saveSportViewModel.sportTypeList = sportTypeList;

            // 回傳攜帶saveSportViewModel物件的SaveSport()的View Razor Page
            return View(saveSportViewModel);
        }
        #endregion

        #region 儲存運動紀錄的動作方法
        [HttpPost]  // 只接受POST請求
        [ValidateAntiForgeryToken] // 設定防止CSRF攻擊的標註
        // 路由：/LiteTallybook/SaveSport
        // 傳入參數：SaveSportViewModel物件
        // 只接受(繫結)price, sportType, comment, payDate屬性
        public async Task<IActionResult> SaveSport([Bind("time,sportType,heartbeat,remark,sportDate")] SaveSportViewModel sportData)
        {
            if (ModelState.IsValid) // 假如傳入的SaveSportViewModel物件的模型繫結狀態正確，則執行以下程式碼
            {
                // 建立運動紀錄物件，並存入從前端網頁傳過來的運動紀錄資料
                SportRecord sportRecord = new SportRecord();
                sportRecord.time = sportData.time;
                sportRecord.sportType = sportData.sportType;
                sportRecord.heartbeat = sportData.heartbeat;
                sportRecord.remark = sportData.remark;
                sportRecord.sportDate = sportData.sportDate;

                db.SportRecords.Add(sportRecord); // 將運動紀錄物件新增到SportRecords資料表中
                await db.SaveChangesAsync(); // 儲存變更到資料庫
                // 將成功儲存1比運動紀錄訊息存入result屬性中
                TempData["ResultOfSaveSport"] = "已成功儲存一筆運動紀錄!\n";// 儲存運動紀錄結果的訊息

                //重新導向儲存運動資料頁面
                return RedirectToAction("SaveSport");
            }
            else
            {
                // 若模型繫結狀態不正確，則回傳攜帶SaveSportViewModel物件的SaveSport()的View Razor Page
                return View(sportData);
            }
        }
        #endregion

        #region  顯示查詢運動紀錄頁面的動作方法
        // 沒有HTTP動詞標示，預設只接受GET請求
        // 路由：/LiteTallybook/QuerySport
        public IActionResult QuerySport()
        {
            // 回傳QuerySport()的View Razor Page
            return View();
        }
        #endregion

        #region 查詢運動紀錄的動作方法
        [HttpPost] // 設定只接受POST請求
        [ValidateAntiForgeryToken] // 設定防止CSRF攻擊的標註
        // 路由：/LiteTallybook/QuerySport
        // 傳入參數： QuerySportViewModel物件
        // 只接受(繫結)startDate,endDate,queryMode屬性
        public IActionResult QuerySport([Bind("startDate,endDate,queryMode")] QuerySportViewModel querySportData)
        {
            if (ModelState.IsValid) // 假如傳入的QuerySportViewModel物件的模型繫結狀態正確，則執行以下程式碼
            {
                // 取出從前端網頁傳過來的起、訖日期及查詢模式
                DateTime sDate = querySportData.startDate.Date;
                DateTime eDate = querySportData.endDate.Date;
                string str = "";      //用於紀錄查詢結果字串
                int totalTime = 0; // 用於紀錄運動總時數
                int rowCount;    // 用於儲存紀錄數量

                // 使用LINQ語法查詢起訖日期間的運動紀錄，並依照運動日期排序
                var result = from a in db.SportRecords
                             where ((a.sportDate >= sDate) && (a.sportDate <= eDate))
                             orderby a.sportDate
                             select a;
                rowCount = result.Count(); // 取得記錄數量
                str = "在" + sDate.Date.ToString("yyyy-MM-dd") + "到"
                        + eDate.Date.ToString("yyyy-MM-dd") + "共有" + rowCount + "筆運動紀錄:\n";

                foreach (var record in result) // 利用迴圈逐一讀取每一筆紀錄
                { //讀取time欄位(即運動金額time)，並加總到運動總時數中
                    totalTime += record.time;
                }
                // 將運動總時數串接到顯示字串(str)中
                str += "共計運動 " + totalTime + " 分鐘.\n";

                // 顯示運動紀錄之每一個欄位之抬頭，將每一個欄位的抬頭串接到顯示字串(str)中	
                string[] colNames = { "編號", "運動時間", "運動類別", "運動日期", "心跳", "備註" };
                foreach (var name in colNames)
                {
                    str += string.Format("{0}    ", name);
                }

                str += "\n";

                // 利用迴圈逐一讀取每一筆紀錄
                int i = 0;
                foreach (var record in result)
                {
                    // 串接記錄編號(索引值+1)
                    str += string.Format("{0:d4}  ", (i + 1));
                    // 串接price欄位值(運動金額)
                    str += string.Format("{0,7}   ", record.time);
                    // 串接sportType欄位值(運動種類)
                    str += string.Format("{0,8}     ", record.sportType);
                    // 串接payDate欄位值(運動日期)
                    str += string.Format("{0,-10}    ", record.sportDate.Date.ToString("d"));
                    // 串接heartbeat欄位值(心跳)
                    str += string.Format("{0,-2}", record.heartbeat);
                    // 串接remark欄位值(備註)
                    str += string.Format("{0,10}", record.remark);
                    str += "\n";
                    i++;
                }
                // 將查詢結果字串存入QuerySportViewModel物件的result欄位中
                querySportData.result = str;

                // 回傳攜帶QuerySportViewModel物件的QuerySport()的View Razor Page
                return View(querySportData);

            }
            else // 若模型繫結狀態不正確，則回傳攜帶QuerySportViewModel物件的QuerySport()的View Razor Page
            {
                // 將警示訊息存入QuerySportViewModel物件的result欄位中
                querySportData.result = "請輸入查詢日期";
                // 回傳攜帶QuerySportViewModel物件的QuerySport()的View Razor Page
                return View(querySportData);
            }
        }
        #endregion

        #region 顯示刪除運動紀錄頁面的動作方法
        // 沒有HTTP動詞標示，預設只接受GET請求
        // 路由：/LiteTallybook/DeleteSport
        public IActionResult DeleteSport()
        {
            // 回傳DeleteSport()的View Razor Page
            return View();
        }
        #endregion

        #region 刪除運動紀錄的動作方法
        [HttpPost, ActionName("DeleteSport")] // 設定只接受POST請求，並設定動作的名稱為DeleteSport
        [ValidateAntiForgeryToken] // 設定防止CSRF攻擊的標註
        // 路由：/LiteTallybook/DeleteSport
        // 傳入參數： DeleteSportViewModel物件
        // 只接受(繫結)startDate,endDate屬性
        public async Task<IActionResult> DeleteSportConfirmed([Bind("startDate,endDate")] DeleteSportViewModel deleteSportData)
        {
            if (ModelState.IsValid) // 假如傳入的DeleteSportViewModel物件的模型繫結狀態正確，則執行以下程式碼
            {
                // 取出從前端網頁傳過來的起、訖日期
                DateTime sDate = deleteSportData.startDate.Date;
                DateTime eDate = deleteSportData.endDate.Date;
                string str = "";
                // 使用 LINQ 取出起訖日期間的運動資料紀錄 (依照運動日期排序)
                var result = from a in db.SportRecords
                             where ((a.sportDate >= sDate) && (a.sportDate <= eDate))
                             orderby a.sportDate
                             select a;
                int count = result.Count(); // 取得紀錄數
                if (count == 0)  // 若記錄數為0，則回傳沒有運動紀錄之訊息
                {
                    str = "在" + sDate.Date.ToString("yyyy-MM-dd") + "到" +
                            eDate.Date.ToString("yyyy-MM-dd") + "沒有運動紀錄:!";

                    // 將沒有運動紀錄的訊息存入DeleteSportViewModel物件的result欄位中
                    deleteSportData.result = str;
                    // 回傳攜帶DeleteSportViewModel物件的DeleteSport()的View Razor Page
                    return View(deleteSportData);
                }

                db.SportRecords.RemoveRange(result); // 從db物件刪除result中的運動資料紀錄
                await db.SaveChangesAsync();       // 將更新存入資料庫中

                // 將刪除成功的訊息存入DeleteSportViewModel物件的result欄位中
                deleteSportData.result = "已經成功刪除" + count + "筆紀錄!";
                // 回傳攜帶DeleteSportViewModel物件的DeleteSport()的View Razor Page
                return View(deleteSportData);
            }
            else // 若模型繫結狀態不正確，則回傳攜帶DeleteSportViewModel物件的DeleteSport()的View Razor Page
            {
                // 將警示訊息存入DeleteSportViewModel物件的result欄位中
                deleteSportData.result = "請輸入查詢日期";
                // 回傳攜帶DeleteSportViewModel物件的DeleteSport()的View Razor Page
                return View(deleteSportData);
            }
        }
        #endregion

    }
}
