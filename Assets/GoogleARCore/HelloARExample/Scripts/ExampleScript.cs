namespace GoogleARCore.HelloAR
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ExampleScript : MonoBehaviour
    {

        [SerializeField] private LayerMask pointerLayer;

        [SerializeField] private float acceleration;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float ignoreDistance;

        private float turnVelocity;

        private Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            FollowPointer(new Ray(Camera.main.transform.position, Camera.main.transform.forward));
        }

        private void FollowPointer(Ray pointerRay)
        {
            RaycastHit pointerHit;
            if (Physics.Raycast(pointerRay, out pointerHit, 200, pointerLayer))
            {
                Vector3 andyToPointer = pointerHit.point - transform.position;
                andyToPointer.y = 0f;
                Quaternion lookAtPointerRotation = Quaternion.LookRotation(andyToPointer);

                var distance = Vector3.Distance(pointerHit.point, transform.position);
                if (distance > ignoreDistance)
                {
                    Vector3 newRotation = transform.eulerAngles;
                    newRotation.y = Mathf.SmoothDampAngle(newRotation.y, lookAtPointerRotation.eulerAngles.y, ref turnVelocity, turnSpeed);
                    transform.eulerAngles = newRotation;

                    Vector3 forwardForce = (pointerHit.point - transform.position) * acceleration;
                    forwardForce = forwardForce * Time.deltaTime * rb.mass;
                    rb.AddForce(forwardForce);
                }
            }
        }
    }
}