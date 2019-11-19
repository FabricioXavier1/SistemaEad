using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUC.LDSI.Domain.Entities
{
    public class Aluno : Entity
    {
        [ForeignKey("Turma")]
        public int TurmaId { get; set; }
        [StringLength(100)]
        public string Nome { get; set; }
        [StringLength(50)]
        public string Login { get; set; }
        public Turma Turma { get; set; }
        public List<Prova> Provas { get; set; }
    }
}
