using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class InvItem {
    public Item itemType;
    public ushort count;

    public InvItem(Item _itemType, ushort _count)
    {
        this.itemType = _itemType;
        this.count = _count;
    }
}
// С помощью публичных методов HasItem(), GiveItem() и TakeItem() можно в любом месте игры дать, убрать и проверить наличие конкретных предметов.
public class Inventory {
    public List<InvItem> items = new List<InvItem>();
}

public class InventorySystem : MonoBehaviour
{
    public Inventory inventory = new Inventory();
    public short selected = 0;

    public Item carrot;
    public Item cabbage;
    public Item pills;


    public Image[] hudImages;
    public TextMeshProUGUI[] hudCounts;
    public ushort mainHudPanel;


    public TextMeshProUGUI hudDescription;
    public TextMeshProUGUI hudTitle;
    public GameObject itemStatsBox;

    public float scrollCooldown;
    float scrollTimer = 10.0f;
    public AudioSource scrollAudio;
    public bool showUI = true;
    public GameObject inventoryHUD;

    private void UpdateHUD() {
        for (short i = 0; i < hudImages.Length; i++)
        {
            short hudItemIndex = (short)(selected - mainHudPanel + i);
            if (hudItemIndex < 0 || hudItemIndex >= inventory.items.Count)
            {
                hudImages[i].enabled = false;
                hudCounts[i].text = "";    
            }
            else
            {
                hudImages[i].enabled = true;
                hudImages[i].sprite = inventory.items[hudItemIndex].itemType.itemImage;
                hudCounts[i].text = inventory.items[hudItemIndex].count.ToString();    
            }
        }
        if (selected < 0 || selected >= inventory.items.Count)
        {
            itemStatsBox.SetActive(false);
        }
        else
        {
            itemStatsBox.SetActive(true);
            hudDescription.text = inventory.items[selected].itemType.description;
            hudTitle.text = inventory.items[selected].itemType.itemName;
        }
    }

    private short FindItem(Item type)
    {
        for (short i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].itemType == type)
            {
                return i;
            }
        }
        return -1;
    }

    public bool HasItem(Item type, ushort minCount = 1)
    {
        short itemIndex = FindItem(type);
        if (itemIndex == -1)
        {
            return false;
        }
        else
        {
            if (inventory.items[itemIndex].count >= minCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void GiveItem(Item type, ushort count = 1)
    {
        short itemIndex = FindItem(type);
        if (itemIndex == -1)
        {
            inventory.items.Add(new InvItem(type, count));
        }
        else
        {
            inventory.items[itemIndex].count += count;
        }
        UpdateHUD();
    }

    // Start is called before the first frame update
    void Start()
    {
        GiveItem(carrot, 69);
        GiveItem(pills, 10);
        GiveItem(cabbage, 42);
        GiveItem(pills, 24);
    }

    void Update()
    {
        if (showUI)
        {
            if (!inventoryHUD.activeSelf) {
                inventoryHUD.SetActive(true);
            }
            if (scrollTimer <= scrollCooldown)
            {
                scrollTimer += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && selected > 0) {
                scrollTimer = 0.0f;
                selected--;
                UpdateHUD();
                scrollAudio.Play();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && selected <= inventory.items.Count) {
                scrollTimer = 0.0f;
                selected++;
                UpdateHUD();
                scrollAudio.Play();
            }
        }
        else
        {
            if (inventoryHUD.activeSelf) {
                inventoryHUD.SetActive(false);
            }
        }
    }
}

