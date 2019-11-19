using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PUC.LDSI.Domain.Entities
{
    public class OpcaoProva : Entity
    {
        [ForeignKey("OpcaoAvaliacao")]
        public int OpcaoAvaliacaoId { get; set; }
        [ForeignKey("QuestaoProva")]
        public int QuestaoProvaId { get; set; }
        public bool Resposta { get; set; }

        public OpcaoAvaliacao OpcaoAvaliacao { get; set; }
        public QuestaoProva QuestaoProva { get; set; }
    }
}
