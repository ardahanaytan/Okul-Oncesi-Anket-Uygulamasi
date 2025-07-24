using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnketManager : MonoBehaviour
{
    public TextMeshProUGUI anketAdiText;
    public TMP_InputField katilimciAdiInput;
    public GameObject isimPanel;
    private string katilimciIsmi;
    private string anketAdi;
    private Anket anket;
    void Awake()
    {
        anketAdi = UygulanacakAnketAdi.GetAnketAdi();
        anketAdiText.text = anketAdi;
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("AnketListesiScene");
    }

    public void KatilimciIsmiKaydet()
    {
        katilimciIsmi = katilimciAdiInput.text;
        isimPanel.SetActive(false);

        AnketYonetici kayitci = new AnketYonetici();
        anket = kayitci.AnketiYukle(anketAdi);
    }

    

}
