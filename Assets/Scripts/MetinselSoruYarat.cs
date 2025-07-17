using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using NativeFilePickerNamespace;
using System.Collections;

public class MetinselSoruYarat : MonoBehaviour
{
    string anketAdi;
    public TMP_Text kacinciSoruText;
    public Button sesKaydi;
    public Button secenekler;
    public Button yeniSecenek;
    private AudioSource audioSource;
    public GameObject sesKaydiPanel;
    public GameObject scrollView;
    public GameObject sesKaydiYuklendiText;
    public TMP_InputField soruInput;
    public Image resimAlani;
    private string hedefDosyaYolu;
    private string hedefYol;
    public Transform secenekContent;

    void Awake()
    {
        anketAdi = CevapManager.Cevaplar[0].cevap;
    }

    void Start()
    {
        kacinciSoruText.text = CevapManager.index + ". Soru";

        audioSource = gameObject.AddComponent<AudioSource>();

        if (SceneManager.GetActiveScene().name == "MetinselSoruOlusturScene")
        {
            GameObject siradakiSoru = GameObject.Find("siradakiSoruButton");
            GameObject anketiTamamla = GameObject.Find("anketiTamamlaButton");

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
    }

    public void SesDosyasiSec()
    {
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
                sesKaydi.GetComponent<Image>().color = Color.green;
                secenekler.interactable = true;
                sesKaydiYuklendiText.SetActive(true);
            }
        }
    }

    public void SesKaydiniDinle()
    {
        audioSource.Play();
    }

    public void SesKaydıPanel()
    {
        sesKaydiPanel.SetActive(true);
        scrollView.SetActive(false);
        yeniSecenek.interactable = false;
    }

    public void ScrollView()
    {
        AnketYonetici kayitci = new AnketYonetici();
        Anket anket = kayitci.AnketiYukle(anketAdi);

        anket.sorular[CevapManager.index - 1].soruMetni = soruInput.text;
        anket.sorular[CevapManager.index - 1].gorselYolu = hedefDosyaYolu;
        anket.sorular[CevapManager.index - 1].sesYolu = hedefYol;

        kayitci.AnketiKaydet(anket);

        sesKaydiPanel.SetActive(false);
        scrollView.SetActive(true);
        yeniSecenek.interactable = true;
    }

    public void ResimYukleme()
    {
        string[] resimTurleri = new string[] { "image/" };

        NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("Kullanıcı seçim yapmadı.");
                return;
            }

            Debug.Log("Seçilen dosya: " + path);

            string dosyaAdi = Path.GetFileName(path);

            string anketKlasoru = Path.Combine(Application.persistentDataPath, "Anketler", anketAdi);
            string imagesDosyaYolu = Path.Combine(anketKlasoru, "Images");
            if (!Directory.Exists(imagesDosyaYolu))
            {
                Directory.CreateDirectory(imagesDosyaYolu);
            }

            string hedefDosyaYoluAmaYanlis = Path.Combine(imagesDosyaYolu, dosyaAdi);
            hedefDosyaYolu = hedefDosyaYoluAmaYanlis.Replace("\\", "/");
            

            try
            {
                File.Copy(path, hedefDosyaYolu, true);
                Debug.Log("Dosya Kopyalandı: " + hedefDosyaYolu);

                StartCoroutine(ResimGoster(hedefDosyaYolu));
            }
            catch (IOException ex)
            {
                Debug.LogError("Kopyalama Hatası: " + ex.Message);
            }
        }, resimTurleri);
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

        yield return null;
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SiradakiSoru(GameObject button)
    {
        if (SceneManager.GetActiveScene().name == "MetinselSoruOlusturScene")
        {
            AnketYonetici kayitci = new AnketYonetici();
            Anket anket = kayitci.AnketiYukle(anketAdi);

            anket.sorular[CevapManager.index - 1].soruMetni = soruInput.text;

            kayitci.AnketiKaydet(anket);
        }
        else if (SceneManager.GetActiveScene().name == "SecmeliSoruOlusturScene")
        {
            AnketYonetici kayitci = new AnketYonetici();
            Anket anket = kayitci.AnketiYukle(anketAdi);

            foreach (Transform secenek in secenekContent)
            {
                var panelScript = secenek.GetComponent<ResimIslemleri>();
                if (panelScript == null) continue;

                Secenek yeniSecenek = new Secenek
                {
                    secenekMetni = panelScript.GetSecenekMetni(),
                    gorselYolu = panelScript.GetGorselYolu(),
                    sesYolu = panelScript.GetSesYolu()
                };

                anket.sorular[CevapManager.index - 1].secenekler.Add(yeniSecenek);
            }

            kayitci.AnketiKaydet(anket);
        }

        CevapManager.index++;

        if (button.name == "siradakiSoruButton")
        {
            if (CevapManager.Cevaplar[CevapManager.index].cevap == "Metin Kutusu")
            {
                SceneManager.LoadScene("MetinselSoruOlusturScene");
            }
            else if (CevapManager.Cevaplar[CevapManager.index].cevap == "Çoktan Seçmeli")
            {
                SceneManager.LoadScene("SecmeliSoruOlusturScene");
            }
        }
        else if (button.name == "anketiTamamlaButton")
        {
            SceneManager.LoadScene("MainMenu");
        }

        
    }
}
