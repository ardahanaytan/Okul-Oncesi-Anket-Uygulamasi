using UnityEngine;
using TMPro;

public class SoruPanel : MonoBehaviour
{
    public TMP_Dropdown soruTipiDropdown;

    public string GetSoruTipi()
    {
        return soruTipiDropdown.options[soruTipiDropdown.value].text;
    }
}
