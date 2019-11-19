using System.Collections.Generic;

namespace PUC.LDSI.Domain.InputData
{
    public class QuestaoProvaInputData
    {
        public int QuestaoId { get; set; }
        public List<OpcaoProvaInputData> Opcoes { get; set; }
    }
}
