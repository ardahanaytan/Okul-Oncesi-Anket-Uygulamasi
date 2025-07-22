using TMPro;
using UnityEngine;

public class AnketManager : MonoBehaviour
{   
    AnketListesiManager alm = new AnketListesiManager();

    void Start()
    {
        string anketAdi = alm.GetAnketAdi();
        Debug.Log(anketAdi);
    }

}
