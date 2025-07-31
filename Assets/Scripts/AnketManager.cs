using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private int soruSayisi;
    private int soruIndex = 0;
    public TMP_InputField metinSoruInput;
    public GridLayoutGroup optionsGrid;
    public GameObject optionPrefab;
    public RectTransform gridRectTransform;
    AnketYonetici kayitci = new AnketYonetici();

    void Awake()
    {
        anketAdi = UygulanacakAnketAdi.GetAnketAdi();
        anketAdiText.text = anketAdi;
    }

    void Start()
    {
        anket = kayitci.AnketiYukle(anketAdi);
        soruSayisi = anket.sorular.Count;
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("AnketListesiScene");
    }

    public void MetinSoruHazirla()
    {
        metinSoruInput.text = anket.sorular[soruIndex].soruMetni;
    }

    public void SecmeliSoruHazirla()
    {
        foreach (Transform child in optionsGrid.transform)
            Destroy(child.gameObject);

        int secenekSayisi = anket.sorular[soruIndex].secenekler.Count;

        int kolonSayisi = 1;
        int satirSayisi = secenekSayisi;
        Debug.Log("Seçenek Sayısı: " + secenekSayisi);

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

        Debug.Log("Kolon Sayısı: " + kolonSayisi);
        Debug.Log("Satır Sayısı: " + satirSayisi);

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

        for (int i = 0; i < secenekSayisi; i++)
        {
            GameObject option = Instantiate(optionPrefab, optionsGrid.transform);
            option.transform.localScale = Vector3.one;
        }
    }

    public void KatilimciIsmiKaydet()
    {
        katilimciIsmi = katilimciAdiInput.text;
        isimPanel.SetActive(false);

        if (anket.sorular[0].soruTipi == "Metin Kutusu")
        {
            metinPanel.SetActive(true);
            seceneklerPanel.SetActive(false);
            MetinSoruHazirla();
        }
        else if (anket.sorular[0].soruTipi == "Çoktan Seçmeli")
        {
            seceneklerPanel.SetActive(true);
            metinPanel.SetActive(false);
            SecmeliSoruHazirla();
        }
        else
        {
            Debug.LogError("Bilinmeyen soru tipi: " + anket.sorular[0].soruTipi);
        }

    }

    

}
