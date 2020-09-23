using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        public async Task<ActionResult<List<User>>> Get(
                [FromBody] User model,
                [FromServices] DataContext context
        )
        {
            var users = await context.Users.AsNoTracking().ToListAsync();
            return users;
        }



        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
           [FromBody] User model,
           [FromServices] DataContext context
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                model.Role = "employee";
                context.Users.Add(model);
                await context.SaveChangesAsync();
                model.Password = "";
                return model;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível criar uma usuário!" });
            }
        }

        [HttpPost]
        [Route("login")]

        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model, [FromServices] DataContext context)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.UserName == model.UserName && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return BadRequest(new { message = "Usuário ou senha inválidos!" });


            var token = TokenService.GenerateToken(user);

            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put([FromBody] User model, [FromServices] DataContext context, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Não foi possível atualizar usuário" });

            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrado!" });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "ão foi possível atualizar usuário" });
            }
        }
    }
}