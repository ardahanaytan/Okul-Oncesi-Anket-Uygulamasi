using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnketSonuclariManager : MonoBehaviour
{
    public TMP_Text anketAdiText;

    /*public void MaileGec()
    {
        UygulanacakAnketAdi.SetAnketAdi(anketAdiText.text);
        SceneManager.LoadScene("MailScene");
    }*/

    public void CSVyiFilesaKaydet()
    {
        Debug.Log(anketAdiText.text);
        string path = AnketYonetici.CSVPath(anketAdiText.text);
        IOSFileExporter.ExportToFiles(path);
        Debug.Log("CSV dosyası iOS Files uygulamasına kaydedilecek");
    }


}
