using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SellerData
{
    public string SellerName;
    [Range(1, 100)] public int Rating;

    [Header("Archive Settings")]
    public int MinFiles = 1;
    public int MaxFiles = 5;
    public List<FileDataSO> PossibleFiles = new List<FileDataSO>();
}
