using System.Collections.Generic;

public static class CevapManager
{
    public static int index;
    public static List<SoruCevabi> Cevaplar = new List<SoruCevabi>();
}

public class SoruCevabi
{
    public string cevap;

    public SoruCevabi(string cevap)
    {
        this.cevap = cevap;
    }
}