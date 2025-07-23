using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnketPanelManager : MonoBehaviour
{
    public TMP_Text anketAdiText;

    public void AnketeGec()
    {
        UygulanacakAnketAdi.SetAnketAdi(anketAdiText.text);
        SceneManager.LoadScene("AnketScene");
    }
    

}
