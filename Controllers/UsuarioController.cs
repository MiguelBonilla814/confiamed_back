using API_CONFIAMED_MB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace API_CONFIAMED_MB.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{
		private readonly ModelDbContext _context;

		// Inyección del DbContext
		public UsuarioController(ModelDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Usuario>>> AsignarItems()
		{
			// Obtenemos la lista de usuarios y items
			var usuarios = await _context.Usuarios.ToListAsync();
			var itemPendientes = await _context.Items.Where(i => i.Estado == false).ToListAsync(); // Items no asignados

			// Filtramos los items según los criterios de proximidad y relevancia
			var itemsProximosAVencer = itemPendientes.Where(i => (i.Fecha_Entrega - DateTime.Now).Days < 3)
				.OrderBy(i => i.Fecha_Entrega).ToList();

			var itemsRelevantes = itemPendientes.Where(i => i.Relevancia == "Alta")
				.OrderBy(i => i.Fecha_Entrega).ToList();

			// Asignamos los items relevantes a los usuarios disponibles
			foreach (var item in itemsRelevantes)
			{
				var usuarioDisponible = usuarios.Where(u => u.Cant_total_asig < 3)
					.OrderBy(u => u.Cant_total_asig).FirstOrDefault();

				if (usuarioDisponible != null)
				{
					// 1. Asignamos el item al usuario
					item.UsuarioId = usuarioDisponible.Id;
					item.Estado = true; // Marcamos el item como asignado

					// Actualizamos el item en la base de datos
					_context.Items.Update(item);

					// 2. Contamos los items asignados al usuario después de la asignación
					int itemsAsignados = await _context.Items.CountAsync(i => i.UsuarioId == usuarioDisponible.Id);

					// 3. Actualizamos la cantidad de items asignados en el usuario
					usuarioDisponible.Cant_total_asig = itemsAsignados;

					// Actualizamos el usuario en la base de datos
					_context.Usuarios.Update(usuarioDisponible);

					// 4. Guardamos el seguimiento de la asignación
					var seguimiento = new Seguimiento
					{
						IdItem = item.Id,
						FechaAsignacion = DateTime.Now,
						Estado = true
					};
					_context.Seguimientos.Add(seguimiento);  // Agregamos el seguimiento a la base de datos
				}
			}

			// Asignamos los items que están por vencer a los usuarios disponibles
			foreach (var item in itemsProximosAVencer)
			{
				var usuarioDisponible = usuarios.Where(u => u.Cant_total_asig < 3)
					.OrderBy(u => u.Cant_total_asig).FirstOrDefault();

				if (usuarioDisponible != null)
				{
					// 1. Asignamos el item al usuario
					item.UsuarioId = usuarioDisponible.Id;
					item.Estado = true; // Marcamos el item como asignado

					// Actualizamos el item en la base de datos
					_context.Items.Update(item);

					// 2. Contamos los items asignados al usuario después de la asignación
					int itemsAsignados = await _context.Items.CountAsync(i => i.UsuarioId == usuarioDisponible.Id);

					// 3. Actualizamos la cantidad de items asignados en el usuario
					usuarioDisponible.Cant_total_asig = itemsAsignados;

					// Actualizamos el usuario en la base de datos
					_context.Usuarios.Update(usuarioDisponible);

					// 4. Guardamos el seguimiento de la asignación
					var seguimiento = new Seguimiento
					{
						IdItem = item.Id,
						FechaAsignacion = DateTime.Now,
						Estado = true
					};
					_context.Seguimientos.Add(seguimiento);  // Agregamos el seguimiento a la base de datos
				}
			}

			// Guardamos los cambios en la base de datos
			await _context.SaveChangesAsync();
			//hola
			// Creamos un DTO para la respuesta
			var itemsConUsuarios = itemPendientes.Select(i => new
			{
				Item = i,
				UsuarioAsignado = usuarios.FirstOrDefault(u => u.Id == i.UsuarioId)
			}).ToList();

			// Ahora puedes retornar los datos combinados
			return Ok(itemsConUsuarios);
		}

	}
}
