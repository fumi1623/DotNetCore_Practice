using Microsoft.AspNetCore.Mvc;

namespace QuickMaster.Controllers {
    //Controllerクラスの継承
    public class HelloController : Controller {
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
    }
}
