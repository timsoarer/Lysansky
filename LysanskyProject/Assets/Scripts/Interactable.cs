using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{   
    [Serializable]
    public enum InteractType {None, Teleport, Quest, Shop, Talk};
    public string interactText = "Имя объекта";
    public bool destroyOnInteract = false;
    public InteractType interactType;
    public GameObject tooltipPrefab;
    private GameObject tooltipInstance;
    private Outline outline;
    public Camera mainCamera;
    public GameObject canvas;
    public Vector3 tooltipOffset;
    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.GetComponent<Outline>();  
        outline.enabled = false; 

    }

    void Update ()
    {
        if (tooltipInstance != null) {
            tooltipInstance.transform.position = mainCamera.WorldToScreenPoint(transform.position + tooltipOffset);
        }
    }

    void OnMouseEnter()
    {
        outline.enabled = true; // Enable outline effect
        tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);
        tooltipInstance.GetComponent<TextMeshProUGUI>().text = interactText;
    }

    void OnMouseExit()
    {
        outline.enabled = false; // Disable outline effect
        Destroy(tooltipInstance);
        tooltipInstance = null;
    }
}
