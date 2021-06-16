using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Controllers
{
     //[ApiController]
     //[Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        public TodoController(ITodoItemService todoItemService){
            _todoItemService=todoItemService;
        } 

        public async Task<IActionResult> Index(){
            // 从数据库获取 to-do 条目
            var Items= await _todoItemService.GetIncompleteItemsAsync();
            // 把条目置于 model 中
            var model= new TodoViewModel{
                Items= Items
            };
            // 使用 model 渲染视图
            return View(model);
        }
    }
}