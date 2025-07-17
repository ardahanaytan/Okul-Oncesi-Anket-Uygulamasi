using System.Collections;
using UnityEngine;

public class SoruSil : MonoBehaviour
{
    public YeniAnketEkle yae;
    public bool silindiMi = false;

    public void SoruSilButon()
    {
        yae.StartCoroutine(GuncelleVeSil());
    }

    IEnumerator GuncelleVeSil()
    {
        Debug.Log("zort");
        Destroy(transform.gameObject);
        yield return null;
        yae.SoruNumarasiGuncelle(); silindiMi = false;
        
    }
}
