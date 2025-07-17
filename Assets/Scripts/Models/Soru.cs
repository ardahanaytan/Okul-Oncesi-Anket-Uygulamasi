using System;
using System.Collections.Generic;

[Serializable]
public class Soru
{
    public string soruMetni;
    public string gorselYolu;
    public string sesYolu;
    public string soruTipi;
    public List<Secenek> secenekler;
}
