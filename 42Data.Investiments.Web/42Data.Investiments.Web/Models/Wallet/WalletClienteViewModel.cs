using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _42Data.Investiments.Web.Models.Wallet
{
    public class WalletClienteViewModel
    {
        public long Id { get; set; }

        public string ClientId { get; set; }

        public DateTime Mes { get; set; }

        public string PlanoContratado { get; set; }

        public decimal ValorInicial { get; set; }

        public decimal ValorEntradaWise { get; set; }

        public decimal ValorEntradaBold { get; set; }

        public decimal ValorRentabilidadeWise { get; set; }

        public decimal ValorRentabilidadeBold { get; set; }

        public decimal ValorSaqueWise { get; set; }

        public decimal ValorSaqueBold { get; set; }

        public decimal ValorFinal { get; set; }

        public decimal ValorComissaoTotal { get; set; }

        public DateTime DataContratacao { get; set; }
    }
}
