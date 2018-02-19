using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        [HttpGet]
        public async Task<ActionResult> GraphData()
        {
            WalletViewModel model = await ReturnWallet();
            if (model != null)
                return Json(model.Graph.ToList());

            return Json(null);
        }

        public async Task<IActionResult> Index()
        {
            WalletViewModel model = await ReturnWallet();
            if (model != null)
                return View(model);

            return View(new WalletViewModel());
        }

        private async Task<WalletViewModel> ReturnWallet()
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var data = _db.WalletCliente.Where(c => c.ClientId == user.Id).OrderBy(o => o.Mes).ToList();

                IEnumerable<object> grafico = new List<object>();
                grafico = data.Select(s => new
                {
                    Mes = s.Mes.ToString("MMM"),
                    Valor = s.ValorFinal
                });


                var model = new WalletViewModel()
                {
                    InvestimentoTotal = data != null ? data.Select(s => s.ValorEntrada).Sum() : 0,
                    LucroTotal = data != null ? data.Select(s => s.ValorRentabilidade).Sum() : 0,
                    ComissaoTotalPaga = data != null ? data.Select(s => s.ValorComissaoTotal).Sum() : 0,
                    SaldoTotal = data != null ? data.Last().ValorFinal : 0,
                    WalletClienteList = data ?? new List<WalletCliente>(),
                    Graph = grafico.ToList()
                };
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> MakeInvestiment(string Id, string ValorDeposito)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            SendEmail(user, ValorDeposito);

            return Ok(new { mensagem = "E-mail enviado com sucesso. Em breve você receberá o boleto em seu e-mail cadastrado." });
        }

        public void SendEmail(ApplicationUser usuario, string ValorDeposito)
        {
            try
            {
                SmtpClient client = new SmtpClient("mx1.hostinger.com.br");
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("contato@42data.com.br", "abc@1234");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("contato@42data.com.br");
                mailMessage.To.Add("contato@42data.com.br");
                mailMessage.Body = $"Oi, eu {usuario.Name} quero depositar {ValorDeposito} na minha conta";
                mailMessage.Subject = "Novo deposito :)";
                client.Send(mailMessage);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}