using MySql.Data.MySqlClient;
using RolES.Backend.Dados;
using RolES.Backend.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<BdConexao>();
    
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

app.MapPost("/usuario", async (CadastroUsuarioDto requestBody, BdConexao conexao) =>
{
    var sql =
        "INSERT INTO usuarios(nome, email, nascimento, senha, cargo) VALUE (@nome, @email, @nascimento, @senha, 'comum')";
    var mySqlComando = new MySqlCommand(sql, await conexao.CriarConexao());

    mySqlComando.Parameters.AddWithValue("@nome", requestBody.Nome);
    mySqlComando.Parameters.AddWithValue("@email", requestBody.Email);
    mySqlComando.Parameters.AddWithValue("@nascimento", requestBody.Nascimento);
    mySqlComando.Parameters.AddWithValue("@senha", requestBody.Senha);

    var LinhasAfetadas = await mySqlComando.ExecuteNonQueryAsync();

    if (LinhasAfetadas > 0) return Results.Created();

    return Results.Conflict(new
        { Mensagem = "Houve um erro ao tentar criar o seu usuário. Tente novamente mais tarde." });
});

app.Run();