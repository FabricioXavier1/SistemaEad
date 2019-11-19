using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUC.LDSI.Domain.Entities
{
    public class QuestaoProva : Entity
    {
        [ForeignKey("Prova")]
        public int ProvaId { get; set; }
        [ForeignKey("Questao")]
        public int QuestaoId { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public decimal Nota { get; set; }

        public Prova Prova { get; set; }
        public Questao Questao { get; set; }
        public List<OpcaoProva> OpcoesProva { get; set; }
    }
}
