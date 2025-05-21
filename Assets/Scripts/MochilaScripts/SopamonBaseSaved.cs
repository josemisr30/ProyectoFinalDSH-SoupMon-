using System;

[Serializable]
public class SopamonBaseSaved
{
    public string nombre;
    public double vida;
    public double vidaMax;
    public int atck;
    public int def;
    public int speed;
    public int nvl;
    public int num;
    public int exp;
    public int expMax;
    public int tipo;
    public bool salvaje;
    public bool rival;

    public SopamonBaseSaved() { }

    public SopamonBaseSaved(SopamonData sopa)
    {
        nombre = sopa.nombreSopa();
        vida = sopa.vidaActual();
        vidaMax = sopa.vidaMaxSopa();
        atck = sopa.calculaDano(1, sopa.tipoSopa()) == 0 ? 1 : (int)(sopa.calculaDano(1, sopa.tipoSopa()) / 1); // estimación
        def = sopa.defSopa();
        speed = sopa.veloSopa();
        nvl = sopa.nvlSopa();
        num = sopa.imagen();
        exp = sopa.expSop();
        tipo = sopa.tipoSopa();
        salvaje = sopa.atrapable();
        rival = sopa.enemigo();

        // Calcular expMax según nivel
        expMax = 1000;
        for (int i = 1; i < nvl; i++) expMax += 500;
    }

    public SopamonData ToSopamonData()
    {
        return new SopamonData(nombre, nvl, vidaMax / nvl, atck / nvl, def / nvl, speed / nvl, num, salvaje, rival, tipo);
    }
}
