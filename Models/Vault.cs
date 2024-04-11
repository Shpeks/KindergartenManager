using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom.Models
{
    public class Vault
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id   { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateStart { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateEnd { get; set; }
        
        public string IdUser { get; set; }
        public ApplicationUser ApplicationUsers { get; set; }
        public ICollection<VaultNote> VaultNotes { get; set; }

        
    }

    
}