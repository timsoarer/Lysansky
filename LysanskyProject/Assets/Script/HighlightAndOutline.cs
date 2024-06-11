using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class HighlightAndOutline : MonoBehaviour
{
    private Renderer rend;
    public Material sharedMaterial;
    private Outline outline;
    private GameObject tooltip;

    public string objectName = "Object"; // Name of the object for the tooltip
    public GameObject tooltipPrefab; // Prefab of the tooltip

    private void Start()
    {
        rend = GetComponent<Renderer>();
        outline = gameObject.GetComponent<Outline>();

        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }

        outline.enabled = false; // Disable outline by default
    }

    private void OnMouseEnter()
    {
        if (this.gameObject != GameObject.Find("Player")) // Exclude the player
        {
            outline.enabled = true; // Enable outline effect
            ShowTooltip(); // Show tooltip
        }
    }

    private void OnMouseExit()
    {
        outline.enabled = false; // Disable outline effect
        HideTooltip(); // Hide tooltip
    }

    private void ShowTooltip()
    {
        if (tooltipPrefab != null)
        {
            tooltip = Instantiate(tooltipPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            tooltip.transform.SetParent(transform); // Set tooltip as a child of the object
            tooltip.GetComponent<Text>().text = objectName; // Set tooltip text
            tooltip.SetActive(true); // Show tooltip
        }
    }

    private void HideTooltip()
    {
        if (tooltip != null)
        {
            Destroy(tooltip);
        }
    }
}
