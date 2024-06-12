using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public struct Quest {
    public string name;
    public ushort requiredPoints;
    public ushort currentPoints;
    public bool show;
}

public class QuestSystem : MonoBehaviour
{
    public Quest[] quests;

    public TextMeshProUGUI[] dialogueList;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool Completed(ushort questID)
    {
        return quests[questID].currentPoints >= quests[questID].requiredPoints;
    }

    public void ShowQuest(ushort questID)
    { // Reveal new quests only through this function! Otherwise HUD won't update
        quests[questID].show = true;
        UpdateHUD();
    }

    public void HideQuestIfComplete(ushort questID)
    { // Hide quests only through this function! Otherwise HUD won't update
        if (Completed(questID)) {
            quests[questID].show = false;
        }
        UpdateHUD();
    }

    public void AddPoints(ushort questID, ushort points = 0)
    { // Add points only through this function! Otherwise HUD won't update
        quests[questID].currentPoints += points;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        ushort dialogueListNum = 0;
        for (ushort i = 0; i < quests.Length; i++)
        {
            if (quests[i].show && dialogueListNum < dialogueList.Length)
            {
                dialogueList[dialogueListNum].text = "- " + quests[i].name;
                if (quests[i].requiredPoints > 1)
                {
                    dialogueList[dialogueListNum].text += " (" + quests[i].currentPoints.ToString() + "/" + quests[i].requiredPoints.ToString() + ")";
                }

                if (Completed(i))
                {
                    dialogueList[dialogueListNum].color = Color.yellow;
                }
                else
                {
                    dialogueList[dialogueListNum].color = Color.white;
                }
                dialogueListNum++;
            }
        }

        while (dialogueListNum < dialogueList.Length)
        {
            dialogueList[dialogueListNum].text = "";
            dialogueListNum++;
        }
    }
}
