using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnketManager : MonoBehaviour
{
    public TextMeshProUGUI anketAdiText;
    public TMP_InputField katilimciAdiInput;
    public GameObject isimPanel;
    void Awake()
    {
        string anketAdi = UygulanacakAnketAdi.GetAnketAdi();
        anketAdiText.text = anketAdi;
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("AnketListesiScene");
    }

    public void KatilimciIsmiKaydet()
    {
        string katilimciIsmi = katilimciAdiInput.text;
        isimPanel.SetActive(false);
        Debug.Log("Katilimci ismi kaydedildi: " + katilimciIsmi);
    }

}
