using UnityEngine;

public class FollowCharacter : MonoBehaviour
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

    void LateUpdate()
    {
        if (characterCapsule != null)
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

            Vector3 targetPosition = characterCapsule.position + Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, height, -distance);
            //transform.position = targetPosition;
            transform.position = Vector3.Slerp(transform.position, targetPosition, rotationSmoothing * Time.deltaTime);
            transform.LookAt(characterCapsule);
        }
    }
}
