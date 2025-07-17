using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnketYonetici : MonoBehaviour
{
    public void AnketiKaydet(Anket anket)
    {
        string klasorYolu = Path.Combine(Application.persistentDataPath, "Anketler", anket.anketAdi);
        Directory.CreateDirectory(klasorYolu);

        string json = JsonUtility.ToJson(anket, true);
        string dosyaYolu = Path.Combine(klasorYolu, "anket.json");

        File.WriteAllText(dosyaYolu, json);

        Debug.Log("Anket Kaydedildi: " + dosyaYolu);
    }

    public Anket AnketiYukle(string anketAdi)
    {
        string dosyaYolu = Path.Combine(Application.persistentDataPath, "Anketler", anketAdi, "anket.json");

        if (!File.Exists(dosyaYolu))
        {
            Debug.LogError("Anket dosyası bulunamadı: " + dosyaYolu);
            return null;
        }

        string json = File.ReadAllText(dosyaYolu);
        Anket anket = JsonUtility.FromJson<Anket>(json);

        Debug.Log("Anket Yüklendi: " + anket.anketAdi);
        return anket;
    }
}
