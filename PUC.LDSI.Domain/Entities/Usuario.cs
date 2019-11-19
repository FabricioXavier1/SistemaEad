using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PUC.LDSI.Domain.Entities
{
    public class Usuario : IdentityUser
    {
        [PersonalData]
        public int Tipo { get; set; }
        [PersonalData]
        [StringLength(255)]
        public string Nome { get; set; }
    }
}
