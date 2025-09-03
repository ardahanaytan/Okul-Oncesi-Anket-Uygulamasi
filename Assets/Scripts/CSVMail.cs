using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // TMP kullanıyorsan: using TMPro;

public class CsvMail : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInput; // TMP_InputField kullanıyorsan türünü değiştir
    [SerializeField] private string subFolder = "Anketler";
    [SerializeField] private string fileName  = "cevaplar.csv";
    [SerializeField] private string subject   = "Anket CSV";
    [SerializeField] private string body      = "Merhaba,\nCSV dosyası ektedir.";
    public TMP_Text anketAdiText;

    void Start()
    {
        anketAdiText.text = UygulanacakAnketAdi.GetAnketAdi();
    }

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _ComposeMail(string toCsv, string subject, string body, string filePath, string attachmentName);
#endif

    public void OnSendButton()
    {
        var to = (emailInput?.text ?? "").Trim();
        if (!IsLikelyEmail(to))
        {
            Debug.LogWarning("Geçerli bir e-posta adresi giriniz.");
            return;
        }

        // CSV dosya yolu
        string dir = Path.Combine(Application.persistentDataPath, subFolder);
        string dir2 = Path.Combine(dir, anketAdiText.text);
        string csvPath = Path.Combine(dir2, fileName);
        Debug.Log(fileName);
        Debug.Log($"CSV dosya yolu: {csvPath}");

        // Dosya yoksa BOM'lu boş CSV oluştur (Excel/Türkçe karakter uyumu)
        if (!File.Exists(csvPath))
            File.WriteAllText(csvPath, "", new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

#if UNITY_IOS && !UNITY_EDITOR
        _ComposeMail(to, subject, body, csvPath, fileName);
#else
        Debug.Log($"[Editor Sim] iOS mail composer çağrısı: TO={to} PATH={csvPath}");
#endif
    }

    private bool IsLikelyEmail(string s) =>
        !string.IsNullOrEmpty(s) && s.Contains("@") && s.Contains(".") && s.Length >= 6;
}
