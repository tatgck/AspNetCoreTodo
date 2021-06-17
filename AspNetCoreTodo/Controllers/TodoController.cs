using System;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        public TodoController(ITodoItemService todoItemService){
            _todoItemService=todoItemService;
        } 
      
        public async Task<IActionResult> Index(){
            // 从数据库获取 to-do 条目
            var todoItems= await _todoItemService.GetIncompleteItemsAsync();
            // 把条目置于 model 中
            var model= new TodoViewModel{
                Items= todoItems
            };
            // 使用 model 渲染视图
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem){
            if(!ModelState.IsValid){
                return RedirectToAction("Index");
            }
            var successful= await _todoItemService.AddItemAsync(newItem);
            if(!successful){
                return BadRequest("Could not add item.");       
            }
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDown(Guid id){
            if(id==Guid.Empty){
                return RedirectToAction("Index");
            }

            var successful = await _todoItemService.MarkDoneAsync(id);

            if(!successful){
                return BadRequest("Could not mark item as done");
            }
            return RedirectToAction("Index");

        }
    }
}