using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SellerData", menuName = "GameData/Seller")]
public class SellerDataSO : ScriptableObject
{
    public string SellerName;
    public int SellerRating;
    public int ArchivePrice;

    // какие файлы он может продавать
    public List<FileDataSO> PossibleFiles = new List<FileDataSO>();
}
