using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    private InventorySystem inventorySystem;
    private Movement playerMover;
    private FollowObject cameraMover;
    public Image[] shopImages;
    public Item[] shopItems;
    public short[] shopCosts;
    public TextMeshProUGUI shopDescription;
    public TextMeshProUGUI shopTitle;
    public TextMeshProUGUI shopPrice;
    public GameObject shopBox;
    public int selected {get; set;}
    private int lastSelected = 0;
    // Start is called before the first frame update
    void Start()
    {
        selected = 0;
        inventorySystem = GetComponent<InventorySystem>();
        cameraMover = GetComponent<FollowObject>();
        playerMover = cameraMover.characterCapsule.gameObject.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSelected != selected)
        {
            lastSelected = selected;
            UpdateHUD();
        }
    }

    void UpdateHUD()
    {

    }

    public void ShopBuyItem()
    {

    }
}
