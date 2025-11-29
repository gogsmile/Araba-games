using UnityEngine;

[CreateAssetMenu(fileName = "NewFileData", menuName = "Game/File Data")]
public class FileDataSO : ScriptableObject
{
    public int Index;
    public string FileName;
    [TextArea] public string Description;
    public int BasePrice;
    [Range(0f, 100f)] public float DropChance;
    public int MemoryPoints; // сколько очков памяти даёт
}
