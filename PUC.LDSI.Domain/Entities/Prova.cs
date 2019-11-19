using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUC.LDSI.Domain.Entities
{
    public class Prova : Entity
    {
        [ForeignKey("Aluno")]
        public int AlunoId { get; set; }
        [ForeignKey("Avaliacao")]
        public int AvaliacaoId { get; set; }
        public DateTime DataProva { get; set; }
        public decimal NotaObtida { get; set; }
        public Aluno Aluno { get; set; }
        public Avaliacao Avaliacao { get; set; }
        public List<QuestaoProva> QuestoesProva { get; set; }
    }
}
