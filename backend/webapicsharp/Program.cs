
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Servicios;
using webapicsharp.Servicios.Abstracciones;
using webapicsharp.Servicios.Proxy;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "tablasprohibidas.json",
    optional: true,
    reloadOnChange: true
);

//JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });


builder.Services.AddControllers();

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("PermitirTodo", politica => politica
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opciones =>
{

    opciones.IdleTimeout = TimeSpan.FromMinutes(30);


    opciones.Cookie.HttpOnly = true;

    opciones.Cookie.IsEssential = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Web API EcoMedellinSAS", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Por favor, ingrese 'Bearer' seguido del token JWT.",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddScoped<webapicsharp.Servicios.Abstracciones.IServicioCrud,
                           webapicsharp.Servicios.ServicioCrud>();
builder.Services.AddScoped<webapicsharp.Servicios.Abstracciones.IServicioCliente,
                           webapicsharp.Servicios.ServicioCliente>();

//Patron proxy funcionando junto con el servicio de JWT
builder.Services.AddScoped<webapicsharp.Servicios.ServicioJwt>();
builder.Services.AddScoped<IServicioJwt>(provider =>
    {
        var servicioReal = provider.GetRequiredService<ServicioJwt>();
        var cache = provider.GetRequiredService<IMemoryCache>();
        var logger = provider.GetRequiredService<ILogger<ServicioJwtProxy>>();
        var config = provider.GetRequiredService<IConfiguration>();

        return new ServicioJwtProxy(servicioReal, cache, logger);
    });


builder.Services.AddScoped<webapicsharp.Interface.Servicios.Abstracciones.IServicioTrabajador,
                           webapicsharp.Servicios.ServicioTrabajador>();
builder.Services.AddScoped<webapicsharp.Interface.Servicios.Abstracciones.IServicioAdministrador,
                           webapicsharp.Servicios.ServicioAdministrador>();
builder.Services.AddScoped<webapicsharp.Interface.Servicios.Abstracciones.IServicioMaterial,
                           webapicsharp.Servicios.ServicioMaterial>();
builder.Services.AddScoped<webapicsharp.Interface.Servicios.Abstracciones.IServicioEcoPunto,
                           webapicsharp.Servicios.ServicioEcoPunto>();
builder.Services.AddScoped<webapicsharp.Interface.Servicios.Abstracciones.IServicioEntrega,
                           webapicsharp.Servicios.ServicioEntrega>();
builder.Services.AddScoped<webapicsharp.Interface.Servicios.Abstracciones.IServicioReporte,
                           webapicsharp.Servicios.ServicioReporte>();

builder.Services.AddSingleton<webapicsharp.Servicios.Abstracciones.IProveedorConexion,
                              webapicsharp.Servicios.Conexion.ProveedorConexion>();

var proveedorBD = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";

switch (proveedorBD.ToLower())
{
    case "sqlserver":
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioBusquedaPorCampoTabla,
        webapicsharp.Repositorios.RepositorioBuscarPorCampoSqlServer>();
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioLecturaTabla,
        webapicsharp.Repositorios.RepositorioLecturaSqlServer>();
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioEscrituraTabla,
        webapicsharp.Repositorios.RepositorioEscrituraSqlServer>();
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioActualizarTabla,
        webapicsharp.Repositorios.RepositorioActualizarSqlServer>();
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioEliminarTabla,
        webapicsharp.Repositorios.RepositorioEliminarSqlServer>();
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioBuscarUltimoTabla,
        webapicsharp.Repositorios.RepositorioBuscarUltimoSqlServer>();
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioSubconsulta,
        webapicsharp.Repositorios.RepositorioSubconsultaSqlServer>();
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioJoin,
        webapicsharp.Repositorios.RepositorioJoinSqlServer>();
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioConsultaPersonalizada,
        webapicsharp.Repositorios.RepositorioConsultaPersonalizadaSqlServer>();
        break;
    case "sqlserverexpress": 
    case "localdb":
    default:
        break;
}

builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "webapicsharp v1");

    c.RoutePrefix = "swagger";
});



app.UseHttpsRedirection();

app.UseCors("PermitirTodo");

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

