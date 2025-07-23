using System;
using Unity.VisualScripting;

public static class UygulanacakAnketAdi
{
    public static string anketAdi;

    public static void SetAnketAdi(string ad)
    {
        anketAdi = ad;
    }

    public static string GetAnketAdi()
    {
        return anketAdi;
    }

}