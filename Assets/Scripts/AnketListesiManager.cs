using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

public class AnketListesiManager : MonoBehaviour
{
    private List<string> anketler = new List<string>();
    public GameObject anketPanelPrefab;
    public Transform anketlerContent;
    private string anketAdi;

    void Awake()
    {
        string dosyaYolu = System.IO.Path.Combine(Application.persistentDataPath, "Anketler");
        if (Directory.Exists(dosyaYolu))
        {
            string[] anketlerinDosyaYollari = Directory.GetDirectories(dosyaYolu);
            foreach (var anketDosyaYolu in anketlerinDosyaYollari)
            {
                anketler.Add(Path.GetFileName(anketDosyaYolu));
            }
        }
    }

    void Start()
    {
        foreach (var anket in anketler)
        {
            GameObject panel = Instantiate(anketPanelPrefab, anketlerContent);
            panel.GetComponentInChildren<TMP_Text>().text = anket;
        }
    }

    public string GetAnketAdi()
    {
        return anketAdi;
    }

    public void SetAnketAdi(string anketAdi)
    {
        this.anketAdi = anketAdi;
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
