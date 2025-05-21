using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "SopamonData", menuName = "Sopamon Data Base")]
public class SopamonData : ScriptableObject
{
    [SerializeField] private string nombre;
    [SerializeField] private double vida = 1, vidaMax = 1;
    [SerializeField] private int atck = 1, def = 1, speed = 1, nvl = 1, num, exp = 0, expMax = 1000, tipo;//Tipo = 0 piedra, = 1 papel, = 2 tijeras
    [SerializeField] private bool salvaje = true, rival = true;

    public SopamonData(string nombre_, int nivel_, double vidaMax_, int atck_, int def_, int speed_, int num_, bool atr_, bool ene_, int tipo_)
    {
        nombre = nombre_;
        nvl = nivel_;
        vida = vidaMax_ * nivel_;
        vidaMax = vidaMax_ * nivel_;
        atck = atck_ * nivel_;
        def = def_ * nivel_;
        speed = speed_ * nivel_;
        num = num_;
        salvaje = atr_;
        rival = ene_;
        tipo = tipo_;
        for (int i = 1; i < nvl; i++) expMax += 500;
    }

    public void load(SopamonData sopa, bool atr, bool ene, int nivel)
    {
        nombre = sopa.nombre;
        nvl = nivel;
        vida = sopa.vidaMax * nivel;
        vidaMax = sopa.vidaMax * nivel;
        atck = sopa.atck * nivel;
        def = sopa.def * nivel;
        speed = sopa.speed * nivel;
        num = sopa.num;
        salvaje = atr;
        rival = ene;
    }
    public void aplicaDano(double dano)
    {
        if (dano > 0)
        {
            dano -= def;
            if (dano <= 0) dano = 1;
            vida -= dano;
        }
    }
    public void subirNivel(int nvlR)
    {
        nvlR -= nvl;
        if (nvlR < 0) nvlR = 0;
        exp += 500 + 1000 * nvlR;
        while (exp > expMax)
        {
            exp -= expMax;
            nvl += 1;
            expMax += 500;
        }
        if (nvl == 12 && num == 0)
        {
            num = 1;
            nombre = "Oscurroedor";
        }
        if (nvl == 10 && num == 10)
        {
            num = 11;
            nombre = "Hueniquilador";
        }
    }
    public void curar(double cura)
    {
        vida += cura;
        if (vida > vidaMax) vida = vidaMax;
    }
    public void forzarVida(double v)
    {
        vida += v;
    }
    public bool atrapable() { return salvaje; }
    public bool enemigo() { return rival; }
    public int nvlSopa() { return nvl; }
    public int defSopa() { return def; }
    public int veloSopa() { return speed; }
    public int tipoSopa() { return tipo; }
    public int imagen() { return num; }
    public double vidaActual() { return vida; }
    public double vidaVisual() { return vida / vidaMax; }
    public double vidaMaxSopa() { return vidaMax; }
    public double calculaDano(int combo, int tipoRival)
    {
        if (tipo == tipoRival) return atck * combo;
        if (tipo < tipoRival || (tipo == 0 && tipoRival == 2)) return atck * combo / 2;
        if (tipo > tipoRival || (tipo == 2 && tipoRival == 0)) return atck * combo * 2;
        return 0;
    }
    public string nombreSopa() { return nombre; }

    public int expSop() { return exp; }
    public float expBar()
    {
        float aux = exp, aux2 = expMax;
        return aux/aux2;
    }

}