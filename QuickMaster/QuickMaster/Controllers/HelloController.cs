using Microsoft.AspNetCore.Mvc;
using QuickMaster.Models;

namespace QuickMaster.Controllers {
    //Controllerクラスの継承
    public class HelloController : Controller {
        private readonly MyContext _context;

        //コンテキストクラスを取得
        public HelloController(MyContext context) {
            this._context = context;
        }

        //アクションメソッド（Indexアクション）
        public IActionResult Index() {
            //結果処理の方法を宣言
            return Content("こんにちは、世界・。・");
        }

        public IActionResult Greet() {
            //View変数の準備
            ViewBag.Message = "こんにちは世界！・。・！";
            //テンプレートを呼び出す
            return View();
        }

        public IActionResult List() {
            //書籍情報をビューに渡す
            return View(this._context.Book);
        }
    }
}
