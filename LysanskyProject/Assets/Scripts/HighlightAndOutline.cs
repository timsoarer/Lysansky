using UnityEngine;

public class HighlightAndOutline : MonoBehaviour
{
    private Renderer rend;
    public Material sharedMaterial;
    private Outline outline;
    private GameObject tooltip;

    public string objectName = "Object"; // Название объекта для таблички
    public GameObject tooltipPrefab; // Префаб таблички

    private void Start()
    {
        rend = GetComponent<Renderer>();
        outline = gameObject.GetComponent<Outline>();

        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }

        outline.enabled = false; // Выключаем выделение по умолчанию
    }

    private void OnMouseEnter()
    {
        if (this.gameObject != GameObject.Find("Player")) // Исключить игрока
        {
            outline.enabled = true; // Включаем эффект выделения
            ShowTooltip(); // Показываем табличку
        }
    }

    private void OnMouseExit()
    {
        outline.enabled = false; // Выключаем эффект выделения
        HideTooltip(); // Прячем табличку
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
