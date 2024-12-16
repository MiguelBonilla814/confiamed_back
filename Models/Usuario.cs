public class Usuario
{
	public int Id { get; set; }
	public string Nombre { get; set; }
	public int Cant_total_asig { get; set; }
	public bool Estado { get; set; }

	public ICollection<Item> Items { get; set; }
}