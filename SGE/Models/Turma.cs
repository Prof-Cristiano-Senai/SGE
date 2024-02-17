namespace SGE.Models
{
    public class Turma
    {
        public Guid TurmaId { get; set; }
        public string TurmaNome { get; set; }
        public string Turno { get; set; }
        public string Serie { get; set; }
        public bool CadAtivo { get; set; }
        public DateTime? CadInativo { get; set; }
    }
}
