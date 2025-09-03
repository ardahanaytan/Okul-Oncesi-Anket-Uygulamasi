using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using System.Linq;

public class AnketYonetici : MonoBehaviour
{
    public void AnketiKaydet(Anket anket)
    {
        string klasorYolu = Path.Combine(Application.persistentDataPath, "Anketler", anket.anketAdi);
        Directory.CreateDirectory(klasorYolu);

        string json = JsonUtility.ToJson(anket, true);
        string dosyaYolu = Path.Combine(klasorYolu, "anket.json");
        File.WriteAllText(dosyaYolu, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

        string csvYolu = Path.Combine(klasorYolu, "cevaplar.csv");
        File.WriteAllText(csvYolu, BuildQuestionsCsv(anket), new UTF8Encoding(true));

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
    
    public string CSVPath(string anketAdi)
    {
        string csvYolu = Path.Combine(Application.persistentDataPath, "Anketler", anketAdi, "cevaplar.csv");
        return csvYolu;
    }

    private string BuildQuestionsCsv(Anket anket)
    {
        var sb = new StringBuilder();

        var headers = new List<string> { "İsim" };

        if (anket.sorular != null && anket.sorular.Count > 0)
        {
            for (int i = 0; i < anket.sorular.Count; i++)
            {
                headers.Add(EscapeCsv(anket.sorular[i]?.soruMetni ?? $"Soru{i + 1}"));
            }
        }

        sb.AppendLine(string.Join(";", headers));


        return sb.ToString();
    }

    private string EscapeCsv(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        s = s.Replace("\"", "\"\"");
        if (s.Contains(";") || s.Contains("\n") || s.Contains("\r"))
            return $"\"{s}\"";
        return s;
    }

}
