using System;
using System.Collections.Generic;
using System.Text;

namespace PUC.LDSI.Domain.QueryResult
{
    public class QuestaoProvaQueryResult
    {
        public int ProvaId { get; set; }
        public int QuestaoId { get; set; }
        public string Enunciado { get; set; }
        public int Tipo { get; set; }
        public bool Completa { get; set; }
        public List<OpcaoProvaQueryResult> Opcoes { get; set; }
    }
}
