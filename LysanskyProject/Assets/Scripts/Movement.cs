using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
    public float Speed = 0.3f;
    public float horizontalDrag = 0.01f;
    public float RotationSpeed = 5f;
    public Transform CameraTransform;

    public LayerMask GroundLayer = 1; // 1 == "Default"

    private Rigidbody _rb;
    private CapsuleCollider _collider;

    [HideInInspector]
    public bool AllowMovement = true;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        //_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (GroundLayer == gameObject.layer)
            Debug.LogError("Player SortingLayer must be different from Ground SortingLayer!");
    }

    private void FixedUpdate()
    {
        if (AllowMovement)
        {
            MoveLogic();
            RotateTowardsCursor();
        }
    }


    private void MoveLogic()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Vector3 cameraDirPosition = new Vector3(CameraTransform.position.x, transform.position.y, CameraTransform.position.z);
        Quaternion cameraDirRotation = Quaternion.LookRotation(transform.position - cameraDirPosition);

        Vector3 movement = cameraDirRotation * Vector3.right * horizontal + cameraDirRotation * Vector3.forward * vertical;
        
        _rb.AddForce(movement * Speed, ForceMode.VelocityChange);
        _rb.velocity = _rb.velocity - new Vector3(_rb.velocity.x * horizontalDrag, 0f, _rb.velocity.z * horizontalDrag);
    }

    private void RotateTowardsCursor()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            var lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            var targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
    }

    private bool _isGrounded
    {
        get
        {
            var bottomCenterPoint = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);
            return Physics.CheckCapsule(_collider.bounds.center, bottomCenterPoint, _collider.bounds.size.x / 2 * 1f, GroundLayer);
        }
    }
}

