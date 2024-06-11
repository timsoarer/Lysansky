using UnityEngine;

public class HighlightAndOutline : MonoBehaviour
{
    private Renderer rend;
    public Material sharedMaterial;
    private Outline outline;
    private GameObject tooltip;

    public string objectName = "Object"; // �������� ������� ��� ��������
    public GameObject tooltipPrefab; // ������ ��������

    private void Start()
    {
        rend = GetComponent<Renderer>();
        outline = gameObject.GetComponent<Outline>();

        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }

        outline.enabled = false; // ��������� ��������� �� ���������
    }

    private void OnMouseEnter()
    {
        if (this.gameObject != GameObject.Find("Player")) // ��������� ������
        {
            outline.enabled = true; // �������� ������ ���������
            ShowTooltip(); // ���������� ��������
        }
    }

    private void OnMouseExit()
    {
        outline.enabled = false; // ��������� ������ ���������
        HideTooltip(); // ������ ��������
    }

    private void ShowTooltip()
    {
        if (tooltipPrefab != null)
        {
            tooltip = Instantiate(tooltipPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            tooltip.GetComponent<Tooltip>().SetText(objectName);
            tooltip.GetComponent<Tooltip>().target = this.transform;
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
