using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void YeniAnketYarat()
    {
        SceneManager.LoadScene("AnketEkleScene");
    }

    public void AnketUygula()
    {
        SceneManager.LoadScene("AnketListesiScene");
    }

    public void AnketSonuclari()
    {
        SceneManager.LoadScene("AnketSonuclariScene");
    }
    public void AnketDuzenle()
    {
        SceneManager.LoadScene("DuzenleListeScene");
    }
}
