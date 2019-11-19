using System.Collections.Generic;

namespace PUC.LDSI.Domain.InputData
{
    public class ProvaInputData
    {
        public int AvaliacaoId { get; set; }
        public int PublicacaoId { get; set; }
        public List<QuestaoProvaInputData> Questoes { get; set; }
    }
}
