public class Seguimiento
{
	public int Id { get; set; }
	public int IdItem { get; set; }
	public DateTime FechaAsignacion { get; set; }
	public DateTime? FechaCompletado { get; set; }
	public bool Estado { get; set; }
}
