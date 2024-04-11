using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom.Models
{
    public class VaultNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }     
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Не вверные входные данные")]
        public int ChildCount { get; set; }
        [Required(ErrorMessage = "Не вверные входные данные")]
        public int KidCount { get; set; }
        public int IdVault { get; set; }
        public Vault Vault { get; set; }
        public ICollection<ProductConsumption> ProductConsumptions { get; set; }
        public ICollection<PreviousBalance> PreviousBalances { get; set; }
        public ICollection<Arrival> Arrivals { get; set; }
        
    }
}