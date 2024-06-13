using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Shop : MonoBehaviour
{
    private InventorySystem inventorySystem;
    private Movement playerMover;
    private FollowObject cameraMover;
    public Image[] shopImages;
    public Item[] shopItems;
    public ushort[] shopCosts;
    public Item currency;
    public TextMeshProUGUI shopDescription;
    public TextMeshProUGUI shopTitle;
    public TextMeshProUGUI shopPrice;
    public GameObject shopBox;
    public int selected {get; set;}
    private int lastSelected = 0;
    private bool inShop = false;
    // Start is called before the first frame update
    void Start()
    {
        selected = 0;
        inventorySystem = GetComponent<InventorySystem>();
        cameraMover = GetComponent<FollowObject>();
        playerMover = cameraMover.characterCapsule.gameObject.GetComponent<Movement>();
    }

    public void EnterShop()
    {
        inShop = true;
        shopBox.SetActive(true);
        UpdateHUD();
    }

    public void ExitShop()
    {
        shopBox.SetActive(false);
        inShop = false;
    }

    bool isMoving()
    {
        return Math.Abs(Input.GetAxis("Horizontal")) > 0.1f || Math.Abs(Input.GetAxis("Vertical")) > 0.1f || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSelected != selected)
        {
            lastSelected = selected;
            UpdateHUD();
        }

        if (inShop && isMoving())
        {
            ExitShop();
        }
    }

    void UpdateHUD()
    {
        for (ushort i = 0; i < shopImages.Length; i++)
        {
            shopImages[i].sprite = shopItems[i].itemImage;
        }
        shopTitle.text = shopItems[lastSelected].itemName;
        shopDescription.text = shopItems[lastSelected].description;
        shopPrice.text = shopCosts[lastSelected].ToString();
    }

    public void ShopBuyItem()
    {
        if (inventorySystem.HasItem(currency, shopCosts[lastSelected]))
        {
            inventorySystem.TakeItem(currency, shopCosts[lastSelected], true);
            inventorySystem.GiveItem(shopItems[lastSelected], 1);
        }
    }
}
