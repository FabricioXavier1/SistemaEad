using System.Collections.Generic;

namespace PUC.LDSI.Domain.QueryResult
{
    public class ProvaQueryResult
    {
        public int AvaliacaoId { get; set; }
        public int PublicacaoId { get; set; }
        public List<QuestaoProvaQueryResult> Questoes { get; set; }
    }
}
