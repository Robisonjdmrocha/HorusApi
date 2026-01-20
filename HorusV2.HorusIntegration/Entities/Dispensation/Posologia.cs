namespace HorusV2.HorusIntegration.Entities.Dispensation;

public class Posologia
{
    public Posologia()
    {
    }

    public Posologia(int dose, int frequencia, string periodo, string unidadeDose)
    {
        Dose = dose;
        Frequencia = frequencia;
        Periodo = periodo;
        UnidadeDose = unidadeDose;
    }

    public int Dose { get; set; }
    public int Frequencia { get; set; }
    public string Periodo { get; set; }
    public string UnidadeDose { get; set; }
}