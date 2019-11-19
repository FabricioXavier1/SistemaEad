using System;

namespace PUC.LDSI.Domain.QueryResult
{
    public class AvaliacaoPublicadaQueryResult
    {
        public int AlunoId { get; set; }

        public int AvaliacaoId { get; set; }
        public string Disciplina { get; set; }
        public string Materia { get; set; }
        public string Descricao { get; set; }

        public int PublicacaoId { get; set; }
        public DateTime DataPublicacao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int ValorProva { get; set; }

        public int? ProvaId { get; set; }
        public DateTime? DataRealizacao { get; set; }
        public decimal? NotaObtida { get; set; }
    }
}
