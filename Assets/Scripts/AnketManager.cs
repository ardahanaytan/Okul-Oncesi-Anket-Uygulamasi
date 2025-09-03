using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEditor;


public class AnketManager : MonoBehaviour
{
    public TextMeshProUGUI anketAdiText;
    public TMP_InputField katilimciAdiInput;
    public GameObject isimPanel;
    public GameObject seceneklerPanel;
    public GameObject metinPanel;
    private string katilimciIsmi;
    private string anketAdi;
    private Anket anket;
    private string csvYolu;
    private int soruSayisi;
    private int soruIndex = 0;
    public TMP_InputField metinSoruInput;
    public TMP_InputField cevapInput;
    public GridLayoutGroup optionsGrid;
    public GameObject optionPrefab;
    public RectTransform gridRectTransform;
    AnketYonetici kayitci = new AnketYonetici();
    public AudioSource audioSource;
    private readonly List<Button> optionButtons = new List<Button>();
    private bool isPlaying = false;
    private List<string> cevaplar = new List<string>();


    void Awake()
    {
        anketAdi = UygulanacakAnketAdi.GetAnketAdi();
        anketAdiText.text = anketAdi;
    }

    void Start()
    {
        anket = kayitci.AnketiYukle(anketAdi);
        csvYolu = kayitci.CSVPath(anketAdi);
        soruSayisi = anket.sorular.Count;
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("AnketListesiScene");
    }

    public void MetinSoruHazirla()
    {
        metinPanel.SetActive(true);
        seceneklerPanel.SetActive(false);
        cevapInput.text = "";
        metinSoruInput.text = anket.sorular[soruIndex].soruMetni;
    }

    public void SecmeliSoruHazirla()
    {
        seceneklerPanel.SetActive(true);
        metinPanel.SetActive(false);

        foreach (Transform child in optionsGrid.transform)
            Destroy(child.gameObject);

        int secenekSayisi = anket.sorular[soruIndex].secenekler.Count;

        int kolonSayisi = 1;
        int satirSayisi = secenekSayisi;

        // Sabit grid eşleşme tablosu
        if (secenekSayisi == 2)
        {
            kolonSayisi = 2; satirSayisi = 1;
        }
        else if (secenekSayisi == 3)
        {
            kolonSayisi = 3; satirSayisi = 1;
        }
        else if (secenekSayisi == 4)
        {
            kolonSayisi = 2; satirSayisi = 2;
        }
        else if (secenekSayisi <= 6)
        {
            kolonSayisi = 3; satirSayisi = 2;
        }
        else if (secenekSayisi <= 8)
        {
            kolonSayisi = 4; satirSayisi = 2;
        }
        else if (secenekSayisi <= 9)
        {
            kolonSayisi = 3; satirSayisi = 3;
        }
        else if (secenekSayisi <= 12)
        {
            kolonSayisi = 4; satirSayisi = 3;
        }
        else if (secenekSayisi <= 16)
        {
            kolonSayisi = 4; satirSayisi = 4;
        }
        else
        {
            kolonSayisi = Mathf.CeilToInt(Mathf.Sqrt(secenekSayisi));
            satirSayisi = Mathf.CeilToInt((float)secenekSayisi / kolonSayisi);
        }

        // Uygula
        optionsGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        optionsGrid.constraintCount = kolonSayisi;

        // Hücre boyutu
        float panelWidth = gridRectTransform.rect.width;
        float panelHeight = gridRectTransform.rect.height;

        float spacingX = optionsGrid.spacing.x;
        float spacingY = optionsGrid.spacing.y;

        float toplamSpacingX = spacingX * (kolonSayisi - 1);
        float toplamSpacingY = spacingY * (satirSayisi - 1);

        float hucreGenislik = (panelWidth - toplamSpacingX) / kolonSayisi;
        float hucreYukseklik = (panelHeight - toplamSpacingY) / satirSayisi;

        float kareBoyut = Mathf.Min(hucreGenislik, hucreYukseklik);
        optionsGrid.cellSize = new Vector2(kareBoyut, kareBoyut);

        StartCoroutine(SecenekleriOlustur_Co());
    }

    public void KatilimciIsmiKaydet()
    {
        cevaplar.Clear();
        katilimciIsmi = katilimciAdiInput.text;
        cevaplar.Add(katilimciIsmi);
        isimPanel.SetActive(false);

        if (anket.sorular[0].soruTipi == "Metin Kutusu")
        {
            MetinSoruHazirla();
        }
        else if (anket.sorular[0].soruTipi == "Çoktan Seçmeli")
        {
            SecmeliSoruHazirla();
        }
        else
        {
            Debug.LogError("Bilinmeyen soru tipi: " + anket.sorular[0].soruTipi);
        }

    }

    private IEnumerator SecenekleriOlustur_Co()
    {
        var secenekler = anket.sorular[soruIndex].secenekler;

        for (int i = 0; i < secenekler.Count; i++)
        {
            // Seçeneği oluştur
            GameObject option = Instantiate(optionPrefab, optionsGrid.transform);
            option.transform.localScale = Vector3.one;

            // Görseli ata
            Image img = option.transform.Find("AnketFoto").GetComponent<Image>();

            var fotoButton = img.GetComponent<Button>();
            fotoButton.onClick.RemoveAllListeners();
            fotoButton.onClick.AddListener(SonrakiSoru);
            int buttonIndex = i + 1;
            fotoButton.onClick.AddListener(() => cevaplar.Add(buttonIndex.ToString()));

            string gorselYolu = secenekler[i].gorselYolu;
            byte[] imageData = System.IO.File.ReadAllBytes(gorselYolu);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageData);
            img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            Button btn = option.transform.Find("SesKaydiButton").GetComponent<Button>();
            optionButtons.Add(btn);

            string sesYoluLocal = secenekler[i].sesYolu;             
            RectTransform imgRT = img.rectTransform;                

            btn.onClick.AddListener(() =>
            {
                if (isPlaying) return;                               
                StartCoroutine(PlayOptionAudioCo(imgRT, sesYoluLocal));
            });

            // --- ÖNEMLİ KISIM: Ses başlamadan önce büyüt ---
            RectTransform rt = img.rectTransform;
            Vector3 original = rt.localScale;
            Vector3 enlarged = original * 1.75f;      // büyüklük oranı (istersen 1.2f yap)

            // yumuşak büyütme
            yield return StartCoroutine(ScaleCo(rt, original, enlarged, 0.12f));

            // Ses çal ve bitene kadar bekle (bu sırada büyük kalsın)
            yield return StartCoroutine(SesCalVeBekle(secenekler[i].sesYolu));

            // Ses bitti → eski boyuta dön
            yield return StartCoroutine(ScaleCo(rt, enlarged, original, 0.12f));
        }
    }
    public void SonrakiSoru()
    {
        if (anket.sorular[soruIndex].soruTipi == "Metin Kutusu")
        {
            string cevap = cevapInput.text;
            cevaplar.Add(cevap);
        }
        else if(anket.sorular[soruIndex].soruTipi == "Çoktan Seçmeli")
        {
            // Cevap zaten buton tıklama ile eklendi
        }
        else
        {
            Debug.LogError("Bilinmeyen soru tipi: " + anket.sorular[soruIndex].soruTipi);
        }

        soruIndex++;

        if (soruIndex >= soruSayisi)
        {
            Debug.Log("Anket tamamlandı. Cevaplar: " + string.Join(", ", cevaplar));
            
            using (var writer = new StreamWriter(csvYolu, append: true, new System.Text.UTF8Encoding(true)))
            {
                string line = string.Join(";", cevaplar);
                writer.WriteLine(line);
            }

            SceneManager.LoadScene("AnketListesiScene");
        }
        else if (anket.sorular[soruIndex].soruTipi == "Metin Kutusu")
        {
            MetinSoruHazirla();
        }
        else if (anket.sorular[soruIndex].soruTipi == "Çoktan Seçmeli")
        {
            SecmeliSoruHazirla();
        }
        else
        {
            Debug.LogError("Bilinmeyen soru tipi: " + anket.sorular[soruIndex].soruTipi);
        }
    }

    private IEnumerator ScaleCo(RectTransform t, Vector3 from, Vector3 to, float duration)
    {
        float t0 = 0f;
        while (t0 < duration)
        {
            t0 += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t0 / duration);
            t.localScale = Vector3.Lerp(from, to, k);
            yield return null;
        }
        t.localScale = to;
    }

    public IEnumerator SesCalVeBekle(string dosyaYolu)
    {
        if (!File.Exists(dosyaYolu))
        {
            Debug.LogError("Ses dosyası bulunamadı: " + dosyaYolu);
            yield break;
        }

        AudioType audioType = dosyaYolu.EndsWith(".mp3") ? AudioType.MPEG : AudioType.WAV;

        using (var www = UnityWebRequestMultimedia.GetAudioClip("file://" + dosyaYolu, audioType))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ses yüklenemedi: " + www.error);
                yield break;
            }

            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);

            audioSource.clip = clip;
            audioSource.Play();

            // Ses çaldığı sürece bekle
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
    }

    private IEnumerator PlayOptionAudioCo(RectTransform rt, string sesYolu)
    {
        isPlaying = true;

        // tüm butonları kilitle
        foreach (var b in optionButtons) b.interactable = false;

        // büyüt
        Vector3 original = rt.localScale;
        Vector3 enlarged = original * 1.75f;
        yield return StartCoroutine(ScaleCo(rt, original, enlarged, 0.12f));

        // ses çal ve bitene kadar bekle (mevcut fonksiyonun)
        yield return StartCoroutine(SesCalVeBekle(sesYolu));

        // eski boyuta dön
        yield return StartCoroutine(ScaleCo(rt, enlarged, original, 0.12f));

        // kilidi aç
        foreach (var b in optionButtons) b.interactable = true;
        isPlaying = false;
    }

}
