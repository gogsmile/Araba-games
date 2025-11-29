using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArchiveNameList", menuName = "Game/Archive Names")]
public class ArchiveNameListSO : ScriptableObject
{
    public List<string> Names = new List<string>();
}
