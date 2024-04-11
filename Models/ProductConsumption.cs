using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Diplom.Models
{
    public class ProductConsumption
    {
        public int Id { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Не вверные входные данные")]
        public double? FoodCountChild { get; set; }
        [Required(ErrorMessage = "Не вверные входные данные")]
        public double? FoodCountKid { get; set; }
        public int IdFood { get; set; }
        public int IdVaultNote { get; set; }
        public VaultNote VaultNote { get; set; }
        
        public Food Food { get; set; }

    }
}