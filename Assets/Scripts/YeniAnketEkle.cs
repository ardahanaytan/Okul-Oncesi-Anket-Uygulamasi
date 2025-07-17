using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using TMPro;

public class YeniAnketEkle : MonoBehaviour
{
    public TMP_InputField anketAdiInput;
    public GameObject soruPanelPrefab;
    public Transform sorularContent;
    private string anketAdi;
    public GameObject ScrollView;
    public GameObject YeniSoruButton;
    public GameObject NextButton;
    public bool silindiMi = false;

    private void Start()
    {
        SoruNumarasiGuncelle();
    }

    public void AnketAdi()
    {
        ScrollView.SetActive(true);
        YeniSoruButton.SetActive(true);
        NextButton.SetActive(true);
        anketAdi = anketAdiInput.text;
        Debug.Log("Anket Adı: " + anketAdi);
    }

    public void YeniSoruEkle()
    {
        GameObject panel = Instantiate(soruPanelPrefab, sorularContent);
        panel.GetComponentInChildren<SoruSil>().yae = this;

        SoruNumarasiGuncelle();
    }

    public void SoruNumarasiGuncelle()
    {
        int soruNumarasi = 1;

        foreach (Transform soru in sorularContent)
        {
            TMP_Text soruBaslik = soru.Find("Text (TMP)")?.GetComponent<TMP_Text>();
            if (soruBaslik != null)
            {
                soruBaslik.text = soruNumarasi + ". Soru";
                soruNumarasi++;
            }
        }
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BirSonrakiSayfa()
    {
        Anket yeniAnket = new Anket();
        yeniAnket.anketAdi = anketAdi;
        yeniAnket.sorular = new List<Soru>();

        AnketYonetici kayitci = new AnketYonetici();
        
        CevapManager.Cevaplar.Clear();

        CevapManager.Cevaplar.Add(new SoruCevabi(yeniAnket.anketAdi));

        foreach (Transform soruPanel in sorularContent)
        {
            SoruPanel soru = soruPanel.GetComponent<SoruPanel>();
            if (soru != null)
            {
                string secilenSoruTipi = soru.GetSoruTipi();

                Soru yeniSoru = new Soru();
                yeniSoru.soruTipi = secilenSoruTipi;
                yeniSoru.soruMetni = "";
                yeniSoru.gorselYolu = "";
                yeniSoru.sesYolu = "";
                yeniSoru.secenekler = new List<Secenek>();

                yeniAnket.sorular.Add(yeniSoru);

                CevapManager.Cevaplar.Add(new SoruCevabi(secilenSoruTipi));
            }
        }
        
        kayitci.AnketiKaydet(yeniAnket);

        kayitci.AnketiYukle(yeniAnket.anketAdi); 

        CevapManager.index = 1;

        if (CevapManager.Cevaplar[1].cevap == "Metin Kutusu")
        {
            SceneManager.LoadScene("MetinselSoruOlusturScene");
        }
        else if (CevapManager.Cevaplar[1].cevap == "Çoktan Seçmeli")
        {
            SceneManager.LoadScene("SecmeliSoruOlusturScene");
        }
    }
}
