using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnketPanelManager : MonoBehaviour
{
    AnketListesiManager alm = new AnketListesiManager();
    public TMP_Text anketAdiText;

    public void AnketeGec()
    {
        alm.SetAnketAdi(anketAdiText.text);
        SceneManager.LoadScene("AnketScene");
    }
    

}
