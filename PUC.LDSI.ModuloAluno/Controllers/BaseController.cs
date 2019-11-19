using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PUC.LDSI.Domain.Entities;
using System;

namespace PUC.LDSI.ModuloAluno.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly UserManager<Usuario> _userManager;

        protected string LoginUsuario => _userManager.GetUserAsync(User).Result.Email;

        public BaseController(UserManager<Usuario> user)
        {
            _userManager = user;
        }
    }
}
