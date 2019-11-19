using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PUC.LDSI.Domain.Entities
{
    public class Professor : Entity
    {
        [StringLength(100)]
        public string Nome { get; set; }
        public string Login { get; set; }
        
        public List<Avaliacao> Avaliacoes { get; set; }
    }
}
