using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(FollowObject))]
[RequireComponent(typeof(InventorySystem))]
[RequireComponent(typeof(QuestSystem))]
public class DialogueSystem : MonoBehaviour
{
    private Movement playerMover;
    private FollowObject cameraMover;
    private InventorySystem playerInventory;
    private QuestSystem questSystem;
    public Dialogue exampleDialogue;
    public Transform exampleAngle;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;

    bool inDialogue = false;
    Dialogue currentDialogue;
    ushort dialoguePosition = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraMover = GetComponent<FollowObject>();
        playerMover = cameraMover.characterCapsule.gameObject.GetComponent<Movement>();
        playerInventory = GetComponent<InventorySystem>();
        questSystem = GetComponent<QuestSystem>();
        StartDialogue(exampleDialogue, exampleAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if (inDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialoguePosition++;
                if (dialoguePosition < currentDialogue.dialogue.Length)
                {
                    dialogueText.text = currentDialogue.dialogue[dialoguePosition].speech;
                    nameText.text = currentDialogue.dialogue[dialoguePosition].speaker;
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    
    public void StartDialogue(Dialogue d, Transform cameraAngle = null)
    {
        playerMover.AllowMovement = false;
        cameraMover.InCutscene = true;
        cameraMover.cutscenePosition = cameraAngle;
        inDialogue = true;
        currentDialogue = d;
        dialoguePosition = 0;
        playerInventory.showUI = false;

        dialogueBox.SetActive(true);
        dialogueText.text = currentDialogue.dialogue[0].speech;
        nameText.text = currentDialogue.dialogue[0].speaker;
    }

    void EndDialogue()
    {
        if (currentDialogue.giveItem != null && currentDialogue.giveAmount > 0)
        {
            playerInventory.GiveItem(currentDialogue.giveItem, currentDialogue.giveAmount);
        }
        if (currentDialogue.teleportPlayer)
        {
            playerMover.gameObject.GetComponent<Transform>().position = currentDialogue.teleportPosition;
        }

        foreach (QuestGiving questGiveParams in currentDialogue.givenQuests)
        {
            if (questGiveParams.showQuest)
            {
                questSystem.ShowQuest(questGiveParams.questID);
            }

            if (questGiveParams.hideIfComplete)
            {
                questSystem.HideQuestIfComplete(questGiveParams.questID);
            }

            if (questGiveParams.addPoints != 0)
            {
                questSystem.AddPoints(questGiveParams.questID, questGiveParams.addPoints);
            }
        }
        
        if (currentDialogue.continuationDialogue != null)
        {
            currentDialogue = currentDialogue.continuationDialogue;
            dialoguePosition = 0;

            dialogueText.text = currentDialogue.dialogue[0].speech;
            nameText.text = currentDialogue.dialogue[0].speaker;
        }
        else
        {
            playerMover.AllowMovement = true;
            cameraMover.InCutscene = false;
            inDialogue = false;
            dialogueBox.SetActive(false);
            playerInventory.showUI = true;
        }
    }
}
