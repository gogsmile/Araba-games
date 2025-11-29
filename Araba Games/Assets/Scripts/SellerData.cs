using UnityEngine;

[System.Serializable]
public class SellerData
{
    public string SellerName;
    public int Rating; // R
    public int ArchivePrice; // будет рассчитано по формуле
    public ArchiveData Archive;
}
