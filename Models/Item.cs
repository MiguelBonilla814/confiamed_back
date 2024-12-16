using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Item
{
	[Column("id")]
	public int Id { get; set; }
	[Column("descripcion")]
	public string Descripcion { get; set; }
	[Column("fecha_entrega")]
	public DateTime Fecha_Entrega { get; set; }
	[Column("relevancia")]
	public string Relevancia { get; set; }
	[Column("estado")]
	public bool Estado { get; set; }
	[Column("Id_Usuario")]  // Especifica que la columna en la base de datos es Id_Usuario
	public int UsuarioId { get; set; }
	[JsonIgnore]
	public Usuario Usuario { get; set; }
}