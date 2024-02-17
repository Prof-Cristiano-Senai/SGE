namespace SGE.Models
{
    public class ReservaSala
    {
        public Guid ReservaSalaId { get; set; }
        public Guid SalaId { get; set; }
        public Sala? Sala { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public DateTime DataReserva { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFim { get; set; }
        public bool CadAtivo { get; set; }
        public DateTime? CadInativo { get; set; }
    }
}
