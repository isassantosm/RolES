
using MySql.Data.MySqlClient;
using RolES.Backend.Dados;
using RolES.Backend.DTOs;
using RolES.Backend.Modelos;

namespace RolES.Backend.Servicos;

public class UsuarioServicos (BdConexao conexao)
{
    public async Task<int> AdicionarAsync(CadastroUsuarioDto modelo)
    {
        var sql =
            "INSERT INTO usuarios(nome, email, nascimento, senha, cargo) VALUE(@nome, @email, @nascimento, @senha, 'comum')";

        var mySqlComando = new MySqlCommand(sql, await conexao.CriarConexaoAsync());

        mySqlComando.Parameters.AddWithValue("@nome", modelo.Nome);
        mySqlComando.Parameters.AddWithValue("@email", modelo.Email);
        mySqlComando.Parameters.AddWithValue("@nascimento", modelo.Nascimento.ToString("yyyy-MM-dd"));
        mySqlComando.Parameters.AddWithValue("@senha", modelo.Senha);

        var LinhasAfetadas = await mySqlComando.ExecuteNonQueryAsync();

        return LinhasAfetadas;
    }

    public async Task<Usuario?> BuscarPorEmailAsync(string emailBuscado)
    {
        var sql = "SELECT id, nome, senha FROM usuarios WHERE email = @email";

        var mySqlComando = new MySqlCommand(sql, await conexao.CriarConexaoAsync());

        mySqlComando.Parameters.AddWithValue("@email", emailBuscado);

        var leitor = await mySqlComando.ExecuteReaderAsync();

        if (await leitor.ReadAsync())
        {
            var usuarioEncontrado = new Usuario
            {
                Id = leitor.GetInt32(leitor.GetOrdinal("id")),
                Nome = leitor["nome"].ToString() ?? string.Empty,
                Senha = leitor["senha"].ToString() ?? string.Empty
            };

            return usuarioEncontrado;
        }

        return null;
    }
}
        