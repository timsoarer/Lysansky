using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public enum InteractType {None, Teleport, Quest, Shop, Talk};

[Serializable]
public struct Interaction {

    public ushort[] requiredQuests;
    public InteractType interactType;

    [Header("Teleport")]
    public Vector3 teleportLocation;

    [Header("Talk")]
    public Dialogue dialogue;
    public Transform cameraAngle;

    [Header("Quest")]
    public QuestGiving[] giveQuests;
}

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{   
    
    [Header("Basic Properties")]
    public string interactText = "Имя объекта";
    public bool destroyOnInteract = false;
    public float minInteractionRange = 15.0f;
    public GameObject tooltipPrefab;
    public Vector3 tooltipOffset;

    public Interaction[] interactionList;
    private ushort currentInteraction = 0;

    private Camera mainCamera;
    private GameObject player;
    private GameObject tooltipInstance;
    private Outline outline;
    private GameObject canvas;
    private QuestSystem questSystem;
    private DialogueSystem dialogueSystem;
    
    bool CanInteract() {
        bool closeEnough = Vector3.Distance(transform.position, player.transform.position) <= minInteractionRange;
        bool hasType = interactionList[currentInteraction].interactType != InteractType.None;
        return closeEnough && hasType;
    }

    bool RequirementsMet (ushort interactionID) {
        foreach (ushort i in interactionList[interactionID].requiredQuests)
        {
            if (questSystem.quests[i].currentPoints < questSystem.quests[i].requiredPoints)
            {
                return false;
            }
        }
        return true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.GetComponent<Outline>();  
        outline.enabled = false; 
        canvas = GameObject.Find("Canvas");
        mainCamera = Camera.main;
        player = mainCamera.gameObject.GetComponent<FollowObject>().characterCapsule.gameObject;
        questSystem = mainCamera.gameObject.GetComponent<QuestSystem>();
        dialogueSystem = mainCamera.gameObject.GetComponent<DialogueSystem>();
    }

    void Update ()
    {
        if (currentInteraction < interactionList.Length - 1)
        {
            if (RequirementsMet((ushort)(currentInteraction + 1)))
            {
                currentInteraction++;
            }
        }
    }

    void Interact() {
        if (interactionList[currentInteraction].interactType == InteractType.Teleport)
        {
            player.transform.position = interactionList[currentInteraction].teleportLocation;
        }
        else if (interactionList[currentInteraction].interactType == InteractType.Talk)
        {
            dialogueSystem.StartDialogue(interactionList[currentInteraction].dialogue, interactionList[currentInteraction].cameraAngle);
        }
        else if (interactionList[currentInteraction].interactType == InteractType.Quest)
        {
            foreach (QuestGiving questGiveParams in interactionList[currentInteraction].giveQuests)
            {
                questSystem.GiveNewQuests(questGiveParams);
            }
        }
        else if (interactionList[currentInteraction].interactType == InteractType.Shop)
        {
            mainCamera.gameObject.GetComponent<Shop>().EnterShop();
        }

        if (destroyOnInteract)
        {
            if (tooltipInstance != null)
            {
                outline.enabled = false; // Disable outline effect
                Destroy(tooltipInstance);
                tooltipInstance = null;
            }
            Destroy(gameObject);
        }
    }

    void OnMouseEnter()
    {
        if (CanInteract())
        {
            outline.enabled = true; // Enable outline effect
            tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);
            tooltipInstance.GetComponent<TextMeshProUGUI>().text = interactText;
        }
    }

    void OnMouseOver()
    {
        if (CanInteract())
        {
            if (tooltipInstance == null)
            {
                outline.enabled = true; // Enable outline effect
                tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);
                tooltipInstance.GetComponent<TextMeshProUGUI>().text = interactText;
            }
            tooltipInstance.transform.position = mainCamera.WorldToScreenPoint(transform.position + tooltipOffset);
            if (Input.GetMouseButtonDown(0))
            {
                Interact();
            }
        }
        else
        {
            if (tooltipInstance != null)
            {
                outline.enabled = false; // Disable outline effect
                Destroy(tooltipInstance);
                tooltipInstance = null;
            }
        }
    }

    void OnMouseExit()
    {
        if (tooltipInstance != null)
        {
            outline.enabled = false; // Disable outline effect
            Destroy(tooltipInstance);
            tooltipInstance = null;
        }
    }
}
