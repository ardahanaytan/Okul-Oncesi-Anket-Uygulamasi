using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MailButtonManager : MonoBehaviour
{
    public TMP_Text anketAdiText;

    public void MaileGec()
    {
        UygulanacakAnketAdi.SetAnketAdi(anketAdiText.text);
        SceneManager.LoadScene("MailScene");
    }
    
}
