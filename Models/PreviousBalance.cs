using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom.Models
{
    public class PreviousBalance
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не вверные входные данные")]
        public double? StartBalance { get; set; }
        public double? EndBalance { get; set; }
        public int IdVaultNote { get; set; }
        public VaultNote VaultNote { get; set; }
        public int IdFood { get; set; }
        public Food Food { get; set; }

    }
}