using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct Condition {
    public ushort[] requiredQuests;
    public InvItem[] requriedItems;

    public QuestGiving[] giveQuests;
}

public class ConditionListener : MonoBehaviour
{
    public Condition[] listenedConditions;
    private QuestSystem questSystem;
    private InventorySystem inventorySystem;
    private DialogueSystem dialogueSystem;
    private Shop shop;
    public GameObject angryPetr;
    public Dialogue failDialogue;

    public Item currency;
    public 

    bool RequirementsMet (Condition c) {
        foreach (ushort i in c.requiredQuests)
        {
            if (questSystem.quests[i].currentPoints < questSystem.quests[i].requiredPoints)
            {
                return false;
            }
        }

        foreach (InvItem i in c.requriedItems)
        {
            if (!inventorySystem.HasItem(i.itemType, i.count))
            {
                return false;
            }
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        questSystem = GetComponent<QuestSystem>();
        inventorySystem = GetComponent<InventorySystem>();
        shop = GetComponent<Shop>();
        dialogueSystem = GetComponent<DialogueSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        for (ushort i = 0; i < listenedConditions.Length; i++)
        {
            if (RequirementsMet(listenedConditions[i]))
            {
                foreach (QuestGiving questGiveParams in listenedConditions[i].giveQuests)
                {
                    questSystem.GiveNewQuests(questGiveParams);
                }
                listenedConditions[i].giveQuests = new QuestGiving[0];
            }
        }
        if (!questSystem.quests[10].show && !inventorySystem.HasItem(currency, 1) && questSystem.quests[5].currentPoints > 0)
        {
            shop.ExitShop();
            dialogueSystem.StartDialogue(failDialogue);
            angryPetr.SetActive(true);
        }
        if (questSystem.quests[10].currentPoints >= questSystem.quests[10].requiredPoints)
        {
            SceneManager.LoadScene("Outro");
        }
    }
}
