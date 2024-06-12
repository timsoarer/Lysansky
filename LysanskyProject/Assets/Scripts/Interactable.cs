using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct Interaction {
    public bool questRequired;
    public ushort questID;
}

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{   
    [Serializable]
    public enum InteractType {None, Teleport, Quest, Shop, Talk};
    [Header("Basic Properties")]
    public string interactText = "Имя объекта";
    public bool destroyOnInteract = false;
    public InteractType interactType;
    public float minInteractionRange = 15.0f;
    public GameObject tooltipPrefab;
    public Vector3 tooltipOffset;
    [Header("Teleport")]
    public Vector3 teleportLocation;

    private Camera mainCamera;
    private GameObject player;
    private GameObject tooltipInstance;
    private Outline outline;
    private GameObject canvas;
    
    bool InRange() {
        return Vector3.Distance(transform.position, player.transform.position) <= minInteractionRange;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.GetComponent<Outline>();  
        outline.enabled = false; 
        canvas = GameObject.Find("Canvas");
        mainCamera = Camera.main;
        player = mainCamera.gameObject.GetComponent<FollowObject>().characterCapsule.gameObject;
    }

    void Update ()
    {

    }

    void Interact() {
        if (interactType == InteractType.Teleport)
        {
            player.transform.position = teleportLocation;
        }
    }

    void OnMouseEnter()
    {
        if (InRange())
        {
            outline.enabled = true; // Enable outline effect
            tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);
            tooltipInstance.GetComponent<TextMeshProUGUI>().text = interactText;
        }
    }

    void OnMouseOver()
    {
        if (InRange())
        {
            if (tooltipInstance == null)
            {
                outline.enabled = true; // Enable outline effect
                tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);
                tooltipInstance.GetComponent<TextMeshProUGUI>().text = interactText;
            }
            tooltipInstance.transform.position = mainCamera.WorldToScreenPoint(transform.position + tooltipOffset);
            if (Input.GetKeyDown(KeyCode.E))
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
