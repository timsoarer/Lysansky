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

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] dialogue;
    public Item giveItem = null;
    public ushort giveAmount = 0;
    public Dialogue continuationDialogue = null;
    public bool teleportPlayer = false;
    public Vector3 teleportPosition = Vector3.zero;
}

