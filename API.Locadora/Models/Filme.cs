using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using API.Locadora.Models;

namespace API.Locadora.Models
    //classe da entidade filme
{    public class Filme
    {
        [Key()]
        public int Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string Nome { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
        [ForeignKey("Genero")]
        public int GeneroId { get; set; }
        [ForeignKey("Locacao")]
        public int? LocacaoId { get; set; }
        public virtual Genero Genero { get; set; }
        public virtual Locacao Locacao { get; set; }
    }
}
