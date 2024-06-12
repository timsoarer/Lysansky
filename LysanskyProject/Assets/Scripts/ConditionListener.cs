using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Condition c in listenedConditions)
        {
            if (RequirementsMet(c))
            {
                foreach (QuestGiving questGiveParams in c.giveQuests)
                {
                    questSystem.GiveNewQuests(questGiveParams);
                }
            }
        }
    }
}
