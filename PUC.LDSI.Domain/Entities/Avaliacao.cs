using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUC.LDSI.Domain.Entities
{
    public class Avaliacao : Entity
    {
        [ForeignKey("Professor")]
        public int ProfessorId { get; set; }
        
        [Display]
        [StringLength(100)]
        public string Disciplina { get; set; }
        [StringLength(100)]
        public string Materia { get; set; }
        [StringLength(100)]
        public string Descricao { get; set; }

        public Professor Professor { get; set; }

        public List<Questao> Questoes { get; set; }
        public List<Prova> Provas { get; set; }
        public List<Publicacao> Publicacoes { get; set; }
    }
}
