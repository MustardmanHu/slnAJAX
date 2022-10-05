using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using slnAJAX.Models;

namespace slnAJAX.Controllers
{
    public class APIController : Controller
    {
        private readonly IWebHostEnvironment _host;
        public readonly DemoContext _context;
        public APIController(IWebHostEnvironment host)
        {
            _host = host;
            _context = new DemoContext();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult wait()
        {
            return Content("Hello");
        }
        public IActionResult Hello(string txt)
        {
            if (String.IsNullOrEmpty(txt))
            {
                txt = "ajax";
            }
            return Content($"{txt}，歡迎", "text/html", System.Text.Encoding.UTF8);
        }
        public IActionResult Register(Member? member, IFormFile File1/*從HomeController傳回的檔案*/)
        {

            //todo 將收到會員資料寫進資料庫中
            //_host.ContentRootPath會得到專案資料夾(在本地是C:/......../slnAJAX
            var Q=from i in _context.Members where i.Name==member.Name select i;
            if (File1 != null|| Q.Any())
            {
                string filePath = Path.Combine(_host.WebRootPath/*應用程式執行所在伺服器的webroot目錄*/, "uploads"/*根目錄下的資料夾*/, File1.FileName/*檔案名稱*/);

                //將檔案存到資料夾中
                using (var fileStream = new FileStream(filePath, FileMode.Create))//使用using結束後自動關閉串流
                {
                    File1.CopyTo(fileStream);
                }
                #region
                //將檔案轉成二進位
                byte[] imgByte = null;
                using (var memoryStream = new MemoryStream())
                {
                    File1.CopyTo(memoryStream);
                    imgByte = memoryStream.ToArray();
                }
                member.FileData = imgByte;
                #endregion
                member.FileName = File1.FileName;
                Member NewMember = new()
                {
                    Name = member.Name,
                    FileName = member.FileName,
                    Email = member.Email,
                    Age = member.Age,
                    FileData = member.FileData,
                };
                //todo 將收到會員資料寫進資料庫中
                _context.Members.Add(NewMember);
                _context.SaveChanges();
                return Content("已經寫入資料庫", "text/plain", System.Text.Encoding.UTF8);
            }
            else
            {
                return Content("圖片為必要", "text/plain", System.Text.Encoding.UTF8);
            }
        }
        public IActionResult Check(string? name)
        {
            DemoContext context = new();
            var Q = from u in context.Members
                    where u.Name == name
                    select u.Name;
            if (Q.Count() > 0)
            {
                return Content("帳號已存在", "text/plain", System.Text.Encoding.UTF8);
            }
            return Content("帳號可以使用", "text/plain", System.Text.Encoding.UTF8);

        }

        public IActionResult City()
        {
            var cities = from i in _context.Addresses select i.City;
            cities = cities.Distinct();
            return Json(cities);
        }
        public IActionResult Site(string? city)
        {
            var sites = from i in _context.Addresses
                        where i.City == city
                        select i;
            sites = sites.Distinct();
            return Json(sites);
        }
        public IActionResult Road(string? siteid)
        {
            var roads = from i in _context.Addresses
                        where i.SiteId == siteid
                        select i;
            roads = roads.Distinct();
            return Json(roads);
        }
    }
}
