using System.Collections.Generic;

namespace PUC.LDSI.Domain.QueryResult
{
    public class QuestaoQueryResult
    {
        public int QuestaoId { get; set; }
        public int TipoId { get; set; }
        public string Tipo { get; set; }
        public string Enunciado { get; set; }

        public AvaliacaoQueryResult Avaliacao { get; set; }
        public List<OpcaoAvaliacaoQueryResult> Opcoes { get; set; }
    }
}
