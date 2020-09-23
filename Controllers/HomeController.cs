using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{

    [Route("v1")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var employee = new User { Id = 1, UserName = "Tavinho_Fun", Password = "tavis123", Role = "employee" };
            var manager = new User { Id = 2, UserName = "Tavinho_Man", Password = "tavis123", Role = "manager" };
            var category = new Category { Id = 1, Title = "categoria 1", };
            var product = new Product { Id = 1, Category = category, Title = "Produto 1", Price = 39 };
            context.Users.Add(employee);
            context.Users.Add(manager);
            context.Categories.Add(category);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            return Ok(new { message = "Dados configurados" });
        }

    }
}