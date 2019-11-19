namespace PUC.LDSI.Domain.QueryResult
{
    public class OpcaoAvaliacaoQueryResult
    {
        public int OpcaoAvaliacaoId { get; set; }
        public string Descricao { get; set; }
        public bool Verdadeira { get; set; }

        public int AvaliacaoId { get; set; }
        public int QuestaoId { get; set; }

        public AvaliacaoQueryResult Avaliacao { get; set; }
        public QuestaoQueryResult Questao { get; set; }
    }
}
