using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom.Models
{
    public class MenuFood
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не вверные входные данные или пустое поле")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не вверные входные данные")]
        public double CountPerUnit { get; set; }
        [Required(ErrorMessage = "Не вверные входные данные")]
        public double Supply { get; set; }
        [Required(ErrorMessage = "Не вверные входные данные")]
        public int Code { get; set; }
        public int MealId { get; set; }
        public int MealTimeId { get; set; }
        public int UnitId { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public Meal Meal { get; set; }
        public MealTime MealTime { get; set; }
        public Unit Unit { get; set; }
    }
}