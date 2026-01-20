namespace HorusV2.HorusIntegration.Entities.Dispensation;

public class UsuarioSus
{
    public UsuarioSus()
    {
    }

    public UsuarioSus(string documento)
    {
        if (documento.Length == 11)
            Cpf = documento;
        else
            Cns = documento;
    }

    public UsuarioSus(int altura, string cns, string cpf, int peso)
    {
        Altura = altura;
        Cns = cns;
        Cpf = cpf;
        Peso = peso;
    }

    public int Altura { get; set; }
    public string Cns { get; set; }
    public string Cpf { get; set; }
    public int Peso { get; set; }
}