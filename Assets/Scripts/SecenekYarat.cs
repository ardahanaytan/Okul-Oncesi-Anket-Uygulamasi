using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecenekYarat : MonoBehaviour
{
    public Transform secenekContent;
    public GameObject secenekPanelPrefab;
    public Button yeniSecenek;
    public ResimIslemleri resimIslemleri;
    public GameObject siradakiSoruButton;
    public GameObject anketiTamamlaButton;

    void Start()
    {
        SecenekNumarasiDuzenle();
    }

    public void YeniSecenekEkle()
    {
        GameObject yeniPanel = Instantiate(secenekPanelPrefab, secenekContent);
        SecenekNumarasiDuzenle();

        ResimIslemleri panelScript = yeniPanel.GetComponent<ResimIslemleri>();
        panelScript.siradakiSoru = siradakiSoruButton;
        panelScript.anketiTamamla = anketiTamamlaButton;

        if (panelScript != null)
        {
            // Eski panelin aktifliğini kapat
            if (resimIslemleri != null)
                resimIslemleri.enabled = false;

            // Yeni paneli aktif olarak ayarla
            resimIslemleri = panelScript;
            resimIslemleri.BuPaneliAktifEt(yeniSecenek);
        }
    }

    private void SecenekNumarasiDuzenle()
    {
        int secenekNumarasi = 1;

        foreach (Transform secenek in secenekContent)
        {
            TMP_Text secenekBaslik = secenek.Find("Text (TMP)")?.GetComponent<TMP_Text>();
            if (secenekBaslik != null)
            {
                secenekBaslik.text = secenekNumarasi + ". Seçenek";
                secenekNumarasi++;
            }
        }
    }
}
