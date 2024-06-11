using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class DragRigidbody : MonoBehaviour
    {
        // ���������� ��� ��������� ������� � ������ ����������
        public float spring = 350.0f;
        public float damper = 5.0f;
        public float drag = 5.0f;
        public float angularDrag = 5.0f;
        public float distance = 0.2f;
        public string playerTag = "Player"; // ��� ��� ������� ������
        public float grabRadius = 1.0f; // ������ ������� �������

        // ���������� ��� ���������� ����������� �������
        private SpringJoint m_SpringJoint;
        public Rigidbody RigidbodyJoint;
        public bool isKin;

        private void Update()
        {
            HandleInput();
        }

        // ��������� ����� �� ������������
        private void HandleInput()
        {
            if (RigidbodyJoint != null)
            {
                HandleRigidbodyJoint();
            }

            // �������� ������� ����� ������ ���� ��� ��������� �������
            if (Input.GetMouseButtonDown(0))
            {
                TryPickUpObject();
            }
        }

        // ��������� ��������� ����������� �������
        private void HandleRigidbodyJoint()
        {
            if (RigidbodyJoint.isKinematic)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ReleaseObject();
                }
            }
        }

        // ������������ �������
        private void ReleaseObject()
        {
            RigidbodyJoint.isKinematic = false;
            RigidbodyJoint = null;
        }

        // ������� �������� ������
        private void TryPickUpObject()
        {
            var mainCamera = FindCamera();
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                return;
            }

            if (hit.collider.CompareTag("GravityObject"))
            {
                HandleGravityObject(hit);
            }

            if (!hit.rigidbody || hit.rigidbody.isKinematic || hit.collider.CompareTag(playerTag))
            {
                return;
            }

            if (m_SpringJoint == null)
            {
                CreateSpringJoint();
            }

            AttachSpringJoint(hit);
            StartCoroutine(DragObject(hit.distance));
        }

        // ��������� �������, ������� ������������ ����������
        private void HandleGravityObject(RaycastHit hit)
        {
            Rigidbody hitRigidbody = hit.collider.gameObject.GetComponent<Rigidbody>();
            if (hitRigidbody.isKinematic)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hitRigidbody.isKinematic = false;
                    isKin = false;
                }
            }
        }

        // �������� ������� ��� ��������� �������
        private void CreateSpringJoint()
        {
            var go = new GameObject("Rigidbody dragger");
            Rigidbody body = go.AddComponent<Rigidbody>();
            m_SpringJoint = go.AddComponent<SpringJoint>();
            body.isKinematic = true;
        }

        // ������������� ������� � �������
        private void AttachSpringJoint(RaycastHit hit)
        {
            m_SpringJoint.transform.position = hit.point;
            m_SpringJoint.anchor = Vector3.zero;
            m_SpringJoint.spring = spring;
            m_SpringJoint.damper = damper;
            m_SpringJoint.maxDistance = distance;
            m_SpringJoint.connectedBody = hit.rigidbody;
            RigidbodyJoint = m_SpringJoint.connectedBody;
        }

        // �������������� �������
        private IEnumerator DragObject(float distance)
        {
            if (!isKin)
            {
                var oldDrag = m_SpringJoint.connectedBody.drag;
                var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
                m_SpringJoint.connectedBody.drag = drag;
                m_SpringJoint.connectedBody.angularDrag = angularDrag;

                var mainCamera = FindCamera();
                while (Input.GetMouseButton(0))
                {
                    var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    m_SpringJoint.transform.position = ray.GetPoint(distance);
                    yield return null;
                }

                if (m_SpringJoint.connectedBody)
                {
                    m_SpringJoint.connectedBody.drag = oldDrag;
                    m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
                    m_SpringJoint.connectedBody = null;
                }
            }
        }

        // ����� ������ ��� ��������������
        private Camera FindCamera()
        {
            return GetComponent<Camera>() ?? Camera.main;
        }

        // ��������� ������� ������� ������� � ��������� Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, grabRadius);
        }
    }
}
