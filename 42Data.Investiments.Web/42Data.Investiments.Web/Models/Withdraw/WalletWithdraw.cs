using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _42Data.Investiments.Web.Models.Withdraw
{
    public class WalletWithdraw
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public string ClientId { get; set; }

        public DateTime Mes { get; set; }

        public decimal ValorBold { get; set; }

        public decimal ValorWise { get; set; }
    }
}
