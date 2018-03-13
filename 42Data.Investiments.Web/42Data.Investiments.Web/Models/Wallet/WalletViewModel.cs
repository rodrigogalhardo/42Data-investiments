using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _42Data.Investiments.Web.Models.Wallet
{
    public class WalletViewModel
    {
        public decimal SaldoTotal { get; set; }
        public decimal LucroTotal { get; set; }
        public decimal InvestimentoTotal { get; set; }
        public decimal ComissaoTotalPaga { get; set; }
        public decimal PlanoBold { get; set; }
        public decimal PlanoWise { get; set; }
        public string PlanoContratado { get; set; }
        public decimal ValorSaqueWise { get; set; }
        public decimal ValorSaqueBold { get; set; }
        public IEnumerable<WalletClienteViewModel> WalletClienteList { get; set; }
        public List<object> Graph { get; set; }
    }
}
