using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text tooltipText; // Текстовый компонент таблички
    public Transform target; // Объект, за которым следует табличка

    public void SetText(string text)
    {
        tooltipText.text = text;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + Vector3.up * 2;
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0); // Поворачиваем табличку к камере
        }
    }
}
