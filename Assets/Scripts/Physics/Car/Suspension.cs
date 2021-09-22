using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    private Rigidbody rb;
    private Car car;

    [HideInInspector]
    public Vector3 Velocity;

    [Header("Suspension Attributes")]
    public float susDist;
    public Vector3 susDir;
    [Header("Spring Attributes")]

    public float springK;

    [Header("Damper Attributes")]
    public float damperK;

    [Header("Debug")]
    public Vector3 totalForce;
    public float oldCompression, compressionVelocity, compression;


    // Start is called before the first frame update
    void Start()
    {
        susDir = susDir.normalized;
        rb = GetComponentInParent<Rigidbody>();
        car = rb.GetComponent<Car>();
    }

    private Vector3 oldPos;
    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, susDir, out hit, susDist))
        {
            compression = susDist - hit.distance;
            compressionVelocity = (compression - oldCompression) / Time.fixedDeltaTime;
            oldCompression = compression;
            Vector3 springForce = -compression * springK * (rb.transform.rotation * susDir); //Spring
            Vector3 damperForce = -compressionVelocity * damperK * (rb.transform.rotation * susDir); //Damper
            totalForce = springForce + damperForce;
            Vector3 forcePos = transform.position;
            rb.AddForceAtPosition(totalForce, forcePos, ForceMode.Force);
        }
    }
    [ExecuteInEditMode]
    public void OnDrawGizmos()
    {
        rb = GetComponentInParent<Rigidbody>();
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + rb.transform.rotation * susDir * susDist);
        Gizmos.DrawSphere(transform.position + rb.transform.rotation * susDir * susDist, .1f);
    }
}