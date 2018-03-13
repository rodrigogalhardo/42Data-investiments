using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using _42Data.Investiments.Web.Data;
using _42Data.Investiments.Web.Models;
using _42Data.Investiments.Web.Models.Wallet;
using _42Data.Investiments.Web.Models.Withdraw;
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
                var wallet = _db.WalletCliente.Where(c => c.ClientId == user.Id).OrderBy(o => o.Mes).ToList();

                var data = wallet.Select(s => new WalletClienteViewModel()
                {
                    ValorSaqueBold = _db.Walletwithdraw.Where(e => e.Mes >= new DateTime(s.Mes.Year, s.Mes.Month, 1)
                    && e.Mes <= new DateTime(s.Mes.Year, s.Mes.Month, 1).AddMonths(1).AddMilliseconds(-1)).Sum(p => p.ValorBold),

                    ValorSaqueWise = _db.Walletwithdraw.Where(e => e.Mes >= new DateTime(s.Mes.Year, s.Mes.Month, 1)
                    && e.Mes <= new DateTime(s.Mes.Year, s.Mes.Month, 1).AddMonths(1).AddMilliseconds(-1)).Sum(p => p.ValorWise),

                    Mes = s.Mes,
                    PlanoContratado = s.PlanoContratado,
                    DataContratacao = s.DataContratacao,
                    ValorComissaoTotal = s.ValorComissaoTotal,
                    ValorEntradaBold = s.ValorEntradaBold,
                    ValorEntradaWise = s.ValorEntradaWise,
                    ValorFinal = s.ValorFinal,
                    ValorInicial = s.ValorInicial,
                    ValorRentabilidadeBold = s.ValorRentabilidadeBold,
                    ValorRentabilidadeWise = s.ValorRentabilidadeWise

                }).ToList();

                IEnumerable<object> grafico = new List<object>();
                grafico = data.Select(s => new
                {
                    Mes = s.Mes.ToString("MMM"),
                    Valor = s.ValorFinal
                });

                var model = new WalletViewModel()
                {
                    InvestimentoTotal = data != null ? (data.Select(s => s.ValorEntradaWise).Sum() + data.Select(s => s.ValorEntradaBold).Sum()) : 0,
                    ComissaoTotalPaga = data != null ? data.Select(s => s.ValorComissaoTotal).Sum() : 0,
                    SaldoTotal = data != null ? data.Last().ValorFinal : 0,
                    WalletClienteList = data ?? new List<WalletClienteViewModel>(),
                    PlanoContratado = data.Select(s => s.PlanoContratado).ToString().ToUpper(),
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
        public async Task<IActionResult> MakeInvestiment(string Id, string ValorDeposito, string PlanoContratado)
        {
            var usuario = await _userManager.FindByEmailAsync(User.Identity.Name);

            var mensagem = $"Oi, eu {usuario.Name} contratei o plano {PlanoContratado.ToUpper()}, e quero depositar {ValorDeposito} na minha conta";

            SendEmail(mensagem);

            return Ok(new { mensagem = "E-mail enviado com sucesso. Em breve você receberá o boleto em seu e-mail cadastrado." });
        }

        [HttpPost]
        public async Task<IActionResult> MakeWithDraw(string Id, string ValorSaque, string PlanoContratado)
        {

            var usuario = await _userManager.FindByEmailAsync(User.Identity.Name);
            var walletCurrent = _db.WalletCliente.LastOrDefault();
            var limitWise = _db.WalletCliente.Sum(e => e.ValorEntradaWise) + _db.WalletCliente.Sum(e => e.ValorRentabilidadeWise) - _db.Walletwithdraw.Sum(e => e.ValorWise);
            var limitBold = _db.WalletCliente.Sum(e => e.ValorEntradaBold) + _db.WalletCliente.Sum(e => e.ValorRentabilidadeBold) - _db.Walletwithdraw.Sum(e => e.ValorBold);
            var valor = Convert.ToDecimal(ValorSaque);

            var draw = new WalletWithdraw();
            draw.ClientId = usuario.Id;
            draw.Mes = DateTime.Now;


            if (PlanoContratado.ToLower().Contains("bold"))
            {
                if (valor > limitBold)
                {
                    return Ok(new { mensagem = $"O valor informado para saque é maior que o limite disponível de: {limitBold.ToString("C2")} " });
                }
                else
                {
                    draw.ValorBold = valor;
                }

            }
            else if (PlanoContratado.ToLower().Contains("wise"))
            {
                if (valor > limitWise)
                {
                    return Ok(new { mensagem = $"O valor informado para saque é maior que o limite disponível de: {limitWise.ToString("C2")} " });
                }
                else
                {
                    draw.ValorWise = valor;
                }
            }

            //Salvar Saque

            walletCurrent.ValorFinal -= Convert.ToDecimal(ValorSaque);


            _db.Set<WalletWithdraw>().Add(draw);

            _db.Entry(walletCurrent).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _db.SaveChanges();


            var mensagem = $"Oi, eu {usuario.Name} gostaria de fazer um saque no valor de: {ValorSaque}, do plano: {PlanoContratado}";

            SendEmail(mensagem);

            return Ok(new { mensagem = "E-mail enviado com sucesso. Em breve você receberá o boleto em seu e-mail cadastrado." });
        }

        public void SendEmail(string mensagem)
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
                mailMessage.Body = mensagem;

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