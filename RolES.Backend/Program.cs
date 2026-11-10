using MySql.Data.MySqlClient;
using RolES.Backend.Dados;
using RolES.Backend.DTOs;
using RolES.Backend.Servicos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<BdConexao>();
builder.Services.AddScoped<UsuarioServicos>();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.MapGet("/", () => // Metodo sem nome
{
    var dataResposta = DateTime.UtcNow;
    return Results.Ok(new
    {
        Mensagem = "Servidor está online.",
        Data = dataResposta
    });
});

app.MapPost("/usuario", async (CadastroUsuarioDto requestBody, UsuarioServicos uServicos) =>
{

    try
    {
        var linhasAfetadas = await uServicos.AdicionarAsync(requestBody);
            
        if (linhasAfetadas > 0) return Results.Created();

        return Results.Conflict(new
            { Mensagem = "Houve um erro ao tentar criar o seu usuario. Tente novamente mais tarde" });
    }
    catch (MySqlException exception)
    {
        if (exception.Number == 1062)
            return Results.BadRequest(new
            {
                Mensagem = "O e-mail do usuário ja foi cadastro."
            });
        throw;
    }
    catch
    {
        return Results.InternalServerError(500);
    }
});

app.Run();