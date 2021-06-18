using System.Net.Mime;
using System.Net;
using System;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<ApplicationUser> _userManager;
        public TodoController(ITodoItemService todoItemService,UserManager<ApplicationUser> userManager){
            _todoItemService=todoItemService;
            _userManager=userManager;
        } 
      
        public async Task<IActionResult> Index(){
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();
            // 从数据库获取 to-do 条目
            var todoItems= await _todoItemService.GetIncompleteItemsAsync(currentUser);
            // 把条目置于 model 中
            var model= new TodoViewModel{
                Items= todoItems
            };
            // 使用 model 渲染视图
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem, ApplicationUser user){
            if(!ModelState.IsValid){
                return RedirectToAction("Index");
            }

            var currentUser= await _userManager.GetUserAsync(User);  
            if(currentUser == null) return Challenge();
    
            var successful= await _todoItemService.AddItemAsync(newItem,currentUser);

            if(!successful){
                return BadRequest("Could not add item.");       
            }
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDown(Guid id, ApplicationUser user){
            if(id==Guid.Empty){
                return RedirectToAction("Index");
            }

            var currentUser= await _userManager.GetUserAsync(User);  
            if(currentUser == null) return Challenge();

            var successful = await _todoItemService.MarkDoneAsync(id,currentUser);

            if(!successful){
                return BadRequest("Could not mark item as done");
            }
            return RedirectToAction("Index");

        }
    }
}