using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Item", menuName = "Game Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite itemImage;
}
