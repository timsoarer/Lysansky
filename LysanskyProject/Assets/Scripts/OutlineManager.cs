using UnityEngine;
using System.Collections.Generic;

public class OutlineManager : MonoBehaviour
{
    public GameObject targetObject; // ������, ������ �������� ����� ���������� ��������� (��������, �����)
    public float radius = 10f; // ������ ������ �������
    public KeyCode activationKey = KeyCode.E; // ������� ��� ��������� ���������

    private List<HighlightAndOutline> highlightedObjects = new List<HighlightAndOutline>();
    private bool isActivated = false;

    private void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            isActivated = true;
            UpdateOutlines();
        }
        else if (Input.GetKeyUp(activationKey))
        {
            isActivated = false;
            DisableAllOutlines();
        }

        if (isActivated)
        {
            RefreshOutlines();
        }
    }

    private void UpdateOutlines()
    {
        Collider[] hitColliders = Physics.OverlapSphere(targetObject.transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            HighlightAndOutline highlightAndOutline = hitCollider.GetComponent<HighlightAndOutline>();

            if (highlightAndOutline != null && !highlightedObjects.Contains(highlightAndOutline))
            {
                Outline outline = hitCollider.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                    highlightedObjects.Add(highlightAndOutline);
                }
            }
        }
    }

    private void RefreshOutlines()
    {
        for (int i = highlightedObjects.Count - 1; i >= 0; i--)
        {
            HighlightAndOutline highlightAndOutline = highlightedObjects[i];

            if (highlightAndOutline != null)
            {
                float distance = Vector3.Distance(targetObject.transform.position, highlightAndOutline.transform.position);
                Outline outline = highlightAndOutline.GetComponent<Outline>();

                if (distance > radius && outline.enabled)
                {
                    outline.enabled = false;
                    highlightedObjects.RemoveAt(i);
                }
                else if (distance <= radius && !outline.enabled)
                {
                    outline.enabled = true;
                }
            }
            else
            {
                highlightedObjects.RemoveAt(i);
            }
        }

        UpdateOutlines();
    }

    private void DisableAllOutlines()
    {
        foreach (var highlightAndOutline in highlightedObjects)
        {
            if (highlightAndOutline != null)
            {
                Outline outline = highlightAndOutline.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
            }
        }

        highlightedObjects.Clear();
    }
}
