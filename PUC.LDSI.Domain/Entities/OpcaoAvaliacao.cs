using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUC.LDSI.Domain.Entities
{
    public class OpcaoAvaliacao : Entity
    {
        [ForeignKey("Questao")]
        public int QuestaoId { get; set; }
        [StringLength(1000)]
        public string Descricao { get; set; }
        [StringLength(100)]
        public bool Verdadeira { get; set; }

        public Questao Questao { get; set; }
        public List<OpcaoProva> OpcoesProva { get; set; }
    }
}
