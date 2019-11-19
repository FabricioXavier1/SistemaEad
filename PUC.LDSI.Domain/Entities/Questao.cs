using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUC.LDSI.Domain.Entities
{
    public class Questao : Entity
    {
        [ForeignKey("Avaliacao")]
        public int AvaliacaoId { get; set; }
        public int Tipo { get; set; }
        [StringLength(2000)]
        public string Enunciado { get; set; }

        public Avaliacao Avaliacao { get; set; }
        public List<OpcaoAvaliacao> Opcoes { get; set; }
        public List<QuestaoProva> QuestoesProva { get; set; }
    }
}
