using UnityEngine;

[CreateAssetMenu(fileName = "Readme", menuName = "Project/Readme")]
public class Readme : ScriptableObject
{
    [System.Serializable]
    public class Section
    {
        public string heading;
        public string text;
        public string linkText;
        public string url;
    }

    public Texture2D icon;
    public string title;
    public Section[] sections;

    // ReadmeEditor.cs'in erişmeye çalıştığı alan
    public bool loadedLayout;
}
