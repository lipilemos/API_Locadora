using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Locadora.Models;

namespace API.Locadora.Models
{
    //classe da entidade Locação
    public class Locacao
    {
        [Key()]
        public int Id { get; set; }
        public virtual List<Filme> Filmes { get; set; }
        [MaxLength(14)]
        [Required]
        public string CpfCliente { get; set; }
        public DateTime DataLocacao { get; set; }
    }
}
