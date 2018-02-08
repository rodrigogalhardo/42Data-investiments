using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _42Data.Investiments.Web.Data;
using _42Data.Investiments.Web.Models;
using _42Data.Investiments.Web.Models.Wallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _42Data.Investiments.Web.Controllers
{
    /// <summary>
    /// Dashboard controller.
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _db;

        public DashboardController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var data = _db.WalletCliente.Where(c => c.ClientId == user.Id).ToList();
            var model = new WalletViewModel()
            {
                InvestimentoTotal = data != null ? data.Select(s => s.ValorFinal).Sum() : 0,
                LucroTotal = data != null ? data.Select(s => s.ValorRentabilidade).Sum() : 0,
                ComissaoTotalPaga = data != null ? data.Select(s => s.ValorComissaoTotal).Sum() : 0,
                SaldoTotal = data != null ? data.Select(s => s.ValorFinal).Sum() : 0,
                WalletClienteList = data ?? new List<WalletCliente>()
            };

            return View(model);
        }

        public async Task<IActionResult> MakeInvestiment(WalletCliente wallet)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            wallet.ClientId = user.Id;
            _db.Attach<WalletCliente>(wallet).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();

            return Ok(new { Id = wallet.ClientId });
        }
    }
}