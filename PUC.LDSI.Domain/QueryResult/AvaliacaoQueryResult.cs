using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PUC.LDSI.Domain.QueryResult
{
    public class AvaliacaoQueryResult
    {
        public int AvaliacaoId { get; set; }
        public string Professor { get; set; }
        public string Disciplina { get; set; }
        [Display(Name = "Matéria")]
        public string Materia { get; set; }
        [Display(Name = "Descrição")] 
        public string Descricao { get; set; }

        public List<QuestaoQueryResult> Questoes { get; set; }
    }
}
