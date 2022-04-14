using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    private Rigidbody rb;
    private Car car;
    [HideInInspector]
    public Vector3 Velocity;

    [Header("Editor Asignments")]
    public Wheel wheel;

    [Header("Suspension Attributes")]
    public float susDist;
    public Vector3 susDir;

    [Range(3, 31)] [Tooltip("An odd number greater or equal to 3 is recomended")]
    public int raycastN;

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

    public void FixedUpdate() //TODO make raycasts with #OnDrawGizmos() Algorythm
    {
        RaycastHit hit;

        Vector3 baseVec = transform.position + rb.transform.rotation * susDir * (susDist - wheel.radius);
        float angleStep = 180 / (raycastN - 1);

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

            wheel.transform.position = hit.point - rb.transform.rotation * susDir * wheel.radius;
        }
        else
        {
            wheel.transform.position = transform.position + rb.transform.rotation * susDir * (susDist - wheel.radius);
        }
        wheel.transform.rotation = transform.rotation * Quaternion.Euler(90, 90, 0);
    }
    [ExecuteInEditMode]
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        rb = GetComponentInParent<Rigidbody>();

        Vector3 baseVec = transform.position + rb.transform.rotation * susDir * (susDist - wheel.radius);


        Vector3 vec1 = baseVec + new Vector3(0, 0, wheel.radius);
        Vector3 vec2 = baseVec - new Vector3(0, 0, wheel.radius);

        float totalAngle = Vector3.Angle(vec1, vec2); //Degrees
        float angleStep = 180 / (raycastN - 1);

        for(int i = 0; i < raycastN; i++) //trig in unity returns radians
        {
            float angle = (angleStep * i + 180) * Mathf.Deg2Rad;
            Vector3 vecf = baseVec + Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z)
                * new Vector3(0, Mathf.Sin(angle), Mathf.Cos(angle)) * wheel.radius;
            Gizmos.DrawLine(transform.position, vecf);
        }      
    }
}

public struct SuspensionCollision
{
    public SuspensionCollision(float _dist, float _angle, float _radius)
    {
        dist = _dist;
        angle = _angle;
        radius = _radius;
    }
    public float dist { get; }
    public float angle { get; }
    public float totalDist { get { return dist - radius * Mathf.Sin(angle); } }
    public float radius { get; }

    public static SuspensionCollision CheckSmaller(SuspensionCollision col1, SuspensionCollision col2)
    {
        if (col1.totalDist < col2.totalDist)
            return col1;
        else
            return col2;
    }
}