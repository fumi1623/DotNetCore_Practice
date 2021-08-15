using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuickMaster.Models;

namespace QuickMaster.Controllers
{
    public class BooksController : Controller
    {
        private readonly MyContext _context;

        //コンテキストを準備
        public BooksController(MyContext context)
        {
            _context = context;
        }

        // GET: Books(非同期でデータベースにアクセス)
        // async:非同期通信
        public async Task<IActionResult> Index()
        {
            //データベースにアクセス＆取得した結果をリスト化
            //await:一度別スレッドに制御を移したうえで、完了後に再度実行
            return View(await _context.Book.ToListAsync());
        }

        // GET: Books/Details/5
        //リクエストデータidを取得
        //int?:null許容型
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //引数idをキーにデータベースを検索
            //FirstOrDefault:検索して最初に見つかったものを返す
            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            //データがない場合、404エラー
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }


        //HTTP POSTで実行されるアクション（つけないとGET時に実行される）
        [HttpPost]
        [ValidateAntiForgeryToken]
        //ポストデータを引数にバインド（引数としてBook型を指定）
        public async Task<IActionResult> Create([Bind("Id,Title,Price,Publisher,Sample")] Book book)
        {
            if (ModelState.IsValid)
            {
                //モデルをデータベースに反映
                _context.Add(book);
                //反映をセーブする
                await _context.SaveChangesAsync();
                //処理後は一覧画面にリダイレクト
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // [Edit] リンクから呼び出され、編集フォームを生成
        public async Task<IActionResult> Edit(int? id)
        {
            //引数が空の場合は404Not Found
            if (id == null)
            {
                return NotFound();
            }

            //引数idをキーに書籍情報を取得
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            //編集フォームを表示
            return View(book);
        }

        //[Save]ボタンで編集内容をデータベースに反映
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,Publisher,Sample")] Book book)
        {
            //隠しフィールドのid値と、URLパラメーターのidが等しいかをチェック
            if (id != book.Id)
            {
                return NotFound();
            }

            //入力値に問題がなければ更新処理
            if (ModelState.IsValid)
            {
                try
                {
                    //モデルの更新をデータベースに反映
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                //競合が発生した場合の処理
                catch (DbUpdateConcurrencyException)
                {
                    //書籍が存在しなければ404
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        //書籍が存在する場合は例外
                        throw;
                    }
                }
                //データベース更新に成功した場合はリダイレクト
                return RedirectToAction(nameof(Index));
            }
            //入力値に問題がある場合は編集フォームを再描画
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //指定された書籍が存在するかを判定
        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
