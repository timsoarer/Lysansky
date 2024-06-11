using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text tooltipText; // Text in table
    public Transform target; // Object followed by sign

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
            transform.Rotate(0, 180, 0); // Turn the sign towards the camera
        }
    }
}
