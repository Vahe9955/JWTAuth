using JWTAuth.Data;
using JWTAuth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.Controllers
{
    [Route("api/userInfo")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly JWTAuthContext _context;

        public UserInfoController(JWTAuthContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserInfo>>> Get()
        {
            var query = await _context.UserInfos.ToListAsync();
            if (query != null)
            {
                return Ok(query);
            }
            return NotFound();
        }

        // GET api/employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> Get(int id)
        {
            var userinfo = await Task.FromResult(_context.UserInfos.Where(x => x.UserId == id));
            if (userinfo != null)
            {
                return Ok(userinfo);
            }
            return NotFound(); ;
        }

        // POST api/employee
        [HttpPost]
        public async Task<ActionResult<UserInfo>> Post(RequestModelUserInfo userinfo)
        {
            var UserInfo = new UserInfo()
            {
                UserName = userinfo.UserName,
                CreatedDate = DateTime.UtcNow,
                DisplayName = userinfo.DisplayName,
                Email = userinfo.Email,
                Password = userinfo.Password
            };
            await _context.UserInfos.AddAsync(UserInfo);
            await _context.SaveChangesAsync();

            return await Task.FromResult(UserInfo);
        }
    }
}
