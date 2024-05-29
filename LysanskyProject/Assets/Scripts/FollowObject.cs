using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class FollowObject : MonoBehaviour
{
    public Transform characterCapsule;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float horizontalAngle = 45.0f;
    public float verticalAngle = 30.0f;

    public float rotationSpeed = 20.0f;
    public float zoomSpeed = 5.0f;
    public float maxVerticalAngle = 60.0f;
    public float minVerticalAngle = 10.0f;

    public float minDistance = 3.0f;
    public float maxDistance = 20.0f;

    public float rotationSmoothing = 5.0f;

    [HideInInspector]
    public bool InCutscene = false;
    [HideInInspector]
    public Transform cutscenePosition = null;

    // void Start () {
    //     List<AudioListener> objectsInScene = new List<AudioListener>();

    //     foreach (AudioListener go in Resources.FindObjectsOfTypeAll(typeof(AudioListener)) as AudioListener[])
    //     {
    //         Debug.LogWarning(go.gameObject.name);
    //     }
    // }

    void LateUpdate()
    {
        if (characterCapsule != null)
        {
            if (!InCutscene)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    horizontalAngle -= rotationSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    horizontalAngle += rotationSpeed * Time.deltaTime;
                }

                // if (Input.GetKey(KeyCode.UpArrow))
                // {
                //     verticalAngle = Mathf.Clamp(verticalAngle + rotationSpeed * Time.deltaTime, minVerticalAngle, maxVerticalAngle);
                // }
                // else if (Input.GetKey(KeyCode.DownArrow))
                // {
                //     verticalAngle = Mathf.Clamp(verticalAngle - rotationSpeed * Time.deltaTime, minVerticalAngle, maxVerticalAngle);
                // }

                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0)
                {
                    distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);
                }
            }
            
            Vector3 targetPosition;
            if (InCutscene && cutscenePosition != null)
            {
                targetPosition = cutscenePosition.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, cutscenePosition.rotation, rotationSmoothing * Time.deltaTime);
            }
            else
            {
                targetPosition = characterCapsule.position + Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, height, -distance);
                //transform.LookAt(characterCapsule);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(characterCapsule.position - transform.position), rotationSmoothing * Time.deltaTime);
            }
            //transform.position = targetPosition;
            transform.position = Vector3.Slerp(transform.position, targetPosition, rotationSmoothing * Time.deltaTime);
            
        }
    }
}
