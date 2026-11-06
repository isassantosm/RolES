using MySql.Data.MySqlClient;

namespace RolES.Backend.Dados;

public class BdConexao
{
    private readonly string StringConexao = Environment.GetEnvironmentVariable("MYSQL_URI") ?? 
                                            throw new ArgumentNullException
                                                ("MYSQL_URI", "A variável de ambiente MYSQL_URI não foi informada");

    public async Task<MySqlConnection> CriarConexao()
    {
        var mySqlConexao = new MySqlConnection(StringConexao);
        await mySqlConexao.OpenAsync();
        return mySqlConexao;
    } 
}