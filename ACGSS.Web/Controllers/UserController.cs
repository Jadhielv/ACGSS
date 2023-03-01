using ACGSS.Domain.DTOs;
using ACGSS.Domain.Enums;
using ACGSS.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ACGSS.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsers();

            return users != null ?
                        View(users) :
                        Problem("Entity 'Users' is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userService.GetUser((int)id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Address,PhoneNumber,Email")] UserDto userDto)
        {
            userDto.CreatedDate = DateTime.Now;
            userDto.ModifiedDate = DateTime.Now;
            userDto.IsActive = UserStatus.Active;

            if (ModelState.IsValid)
            {
                await _userService.AddUser(userDto);
                return RedirectToAction(nameof(Index));
            }
            return View(userDto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userService.GetUser((int)id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,PhoneNumber,Email,CreatedDate,ModifiedDate,IsActive")] UserDto userDto)
        {
            userDto.ModifiedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateUser(userDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userDto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userService.GetUser((int)id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _userService.GetUsers();
            if (users == null)
            {
                return Problem("Entity 'Users' is null.");
            }

            var user = await _userService.GetUser(id);
            user.ModifiedDate = DateTime.Now;
            user.IsActive = UserStatus.Inactive;

            if (user != null)
            {
                await _userService.UpdateUser(user);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            var user = _userService.GetUser(id);
            return user != null ? true : false;
        }
    }
}
