using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueLine
{
    public string speaker;
    [Multiline(4)]
    public string speech;
}

[System.Serializable]
public struct QuestGiving
{
    public bool showQuest;
    public bool hideIfComplete;
    public ushort questID;
    public ushort addPoints;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] dialogue;
    [Header("Inventory")]
    public Item giveItem = null;
    public ushort giveAmount = 0;
    public Dialogue continuationDialogue = null;
    [Header("Teleportation")]
    public bool teleportPlayer = false;
    public Vector3 teleportPosition = Vector3.zero;
    [Header("Quests")]
    public QuestGiving[] givenQuests;
}

