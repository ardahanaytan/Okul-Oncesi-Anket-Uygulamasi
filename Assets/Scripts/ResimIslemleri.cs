using System.IO;
using UnityEngine;
using UnityEngine.UI;
using NativeFilePickerNamespace;
using System.Collections;
using TMPro;

public class ResimIslemleri : MonoBehaviour
{
    public Image resimAlani;
    public GameObject sesText;
    public GameObject resimText;
    public bool aktifMi;
    private AudioSource audioSource;
    public Button yeniSecenek;
    public GameObject resimYukleButton;
    public GameObject resimPanel;
    public GameObject sesYukleButton;
    public GameObject sesDinleButton;
    private string anketAdi;
    public GameObject siradakiSoru;
    public GameObject anketiTamamla;
    private string hedefYol;
    private string hedefDosyaYolu;
    public TMP_InputField secenekMetni;


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void BuPaneliAktifEt(Button disButon)
    {
        yeniSecenek = disButon;
        aktifMi = true;
        yeniSecenek.interactable = false;
    }

    void Update()
    {
        if (!aktifMi) return; // sadece son panel Update çalıştırır

        if (resimText.activeSelf)
        {
            yeniSecenek.interactable = true;
        }
    }

    public void SecenekMetniOnay()
    {
        resimYukleButton.SetActive(true);
        resimPanel.SetActive(true);
        sesYukleButton.SetActive(true);
        sesDinleButton.SetActive(true);
    }

    public void ResimYukleme()
    {
        // anket adını güvenle al
        if (CevapManager.Cevaplar == null || CevapManager.Cevaplar.Count == 0 || CevapManager.Cevaplar[0] == null)
        {
            Debug.LogError("Anket adı alınamadı (CevapManager).");
            ButonDurumlariniGuncelle(); // akışı bozmamak için
            return;
        }

        anketAdi = CevapManager.Cevaplar[0].cevap;

        if (!NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image))
        {
            Debug.LogWarning("Galeri izni reddedildi.");
            return;
        }

        // Photos/Galeri için NativeGallery kullan
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("Kullanıcı seçim yapmadı.");
                ButonDurumlariniGuncelle();
                return;
            }

            Debug.Log("Seçilen dosya: " + path);

            try
            {
                string dosyaAdi = Path.GetFileName(path);

                string anketKlasoru    = Path.Combine(Application.persistentDataPath, "Anketler", anketAdi);
                string imagesDosyaYolu = Path.Combine(anketKlasoru, "Images");

                if (!Directory.Exists(imagesDosyaYolu))
                    Directory.CreateDirectory(imagesDosyaYolu);

                string hedefDosyaYoluAmaYanlis = Path.Combine(imagesDosyaYolu, dosyaAdi);
                hedefDosyaYolu = hedefDosyaYoluAmaYanlis.Replace("\\", "/");

                // Seçilen resmi kalıcı klasöre kopyala (varsa üzerine yaz)
                File.Copy(path, hedefDosyaYolu, true);

                Debug.Log("Dosya Kopyalandı: " + hedefDosyaYolu);

                StartCoroutine(ResimGoster(hedefDosyaYolu));
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Kopyalama Hatası: " + ex.Message);
            }
            finally
            {
                ButonDurumlariniGuncelle();
            }

        }, "Bir fotoğraf seçin", "image/*");
    }

    // Mevcut buton mantığını ayırıp çağırıyoruz, akış değişmesin
    private void ButonDurumlariniGuncelle()
    {
        if (CevapManager.Cevaplar.Count == CevapManager.index + 1)
        {
            siradakiSoru.SetActive(false);
            anketiTamamla.SetActive(true);
        }
        else
        {
            siradakiSoru.SetActive(true);
            anketiTamamla.SetActive(false);
        }
    }

    IEnumerator ResimGoster(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        if (tex.LoadImage(fileData))
        {
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            resimAlani.sprite = sprite;
        }

        resimText.SetActive(true);
        yield return null;
    }

    public void SesDosyasiSec()
    {
        anketAdi = CevapManager.Cevaplar[0].cevap;

        string[] dosyaTuru = { "audio/wav" };

        NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.LogError("Dosya Seçimi İptal Edildi.");
                return;
            }

            Debug.Log("Seçilen Dosya: " + path);

            string dosyaAdi = Path.GetFileName(path);
            string hedefKlasor = Path.Combine(Application.persistentDataPath, "Anketler", anketAdi, "Voices");

            if (!Directory.Exists(hedefKlasor))
            {
                Directory.CreateDirectory(hedefKlasor);
            }

            string hedefYolAmaYanlis = Path.Combine(hedefKlasor, dosyaAdi);
            hedefYol = hedefYolAmaYanlis.Replace("\\", "/");

            File.Copy(path, hedefYol, true);

            StartCoroutine(SesDosyasiYukle(hedefYol));

        }, dosyaTuru);

        
    }

    IEnumerator SesDosyasiYukle(string path)
    {
        using (var www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Ses Yüklenemedi: " + www.error);
                }
                else
                {
                    AudioClip clip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);
                    audioSource.clip = clip;
                    Debug.Log("Ses Çalıyor");
                    sesText.SetActive(true);
                }
            }

        
    }

    public string GetSecenekMetni()
    {
        return secenekMetni.text;
    }

    public string GetGorselYolu()
    {
        return hedefDosyaYolu;
    }

    public string GetSesYolu()
    {
        return hedefYol;
    }

    public void SesKaydiDinle()
    {
        audioSource.Play();
    }

}
