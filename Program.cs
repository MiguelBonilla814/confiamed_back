using API_CONFIAMED_MB.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext
builder.Services.AddDbContext<ModelDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar servicios al contenedor
builder.Services.AddControllers();

// Agregar Swagger
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "Mi API",
		Version = "v1"
	});
});

var app = builder.Build();

// Configurar el pipeline de la aplicaci�n
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

// Habilitar Swagger
app.UseSwagger();

// Habilitar Swagger UI en la ra�z de la aplicaci�n
app.UseSwaggerUI(c =>
{
	// Esto har� que Swagger UI se cargue en la ra�z
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
	c.RoutePrefix = string.Empty; // Swagger UI estar� disponible en la ra�z
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers(); // Mapea los controladores

app.Run();
