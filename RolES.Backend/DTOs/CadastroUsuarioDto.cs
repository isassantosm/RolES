namespace RolES.Backend.DTOs;

public class CadastroUsuarioDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly Nascimento { get; set; }
    public string Senha { get; set; } = string.Empty;
}