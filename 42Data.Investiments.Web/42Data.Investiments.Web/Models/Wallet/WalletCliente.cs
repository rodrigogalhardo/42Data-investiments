using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _42Data.Investiments.Web.Models.Wallet
{
    public class WalletCliente
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public string ClientId { get; set; }

        public DateTime Mes { get; set; }

        [Column(TypeName = "money")]
        public decimal ValorInicial { get; set; }

        [Column(TypeName = "money")]
        public decimal ValorEntrada { get; set; }

        [Column(TypeName = "money")]
        public decimal ValorRentabilidade { get; set; }

        [Column(TypeName = "money")]
        public decimal ValorFinal { get; set; }

        [Column(TypeName = "money")]
        public decimal ValorComissaoTotal { get; set; }
    }
}
