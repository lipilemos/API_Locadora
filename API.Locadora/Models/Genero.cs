using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Locadora.Models
{
    //classe da entidade genero
    public class Genero
    {
        [Key()]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Nome { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
    }
}
