using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    private Rigidbody rb;
    public Car car;
    [HideInInspector]
    public Vector3 Velocity;

    [Header("Editor Asignments")]
    public Wheel wheel;

    [Header("Suspension Attributes")]
    public float susDist;
    public Vector3 susDirRel;
    public Vector3 susDir; // Absolute

    [Range(1, 31)] [Tooltip("An odd number greater or equal to 3 is recomended")]
    public int raycastN;

    [Header("Spring Attributes")]
    public float springK;

    [Header("Damper Attributes")]
    public float damperK;

    [Header("Debug")]
    public float totalForce;
    public float oldCompression, compressionVelocity, compression;
    public bool wheelOnGround;
    [HideInInspector]
    public RaycastHit hit;


    // Start is called before the first frame update
    [ExecuteAlways]
    void Start()
    {
        wheel = GetComponentInChildren<Wheel>();
        rb = GetComponentInParent<Rigidbody>();
        if (!car)
            car = rb.GetComponent<Car>();
        
    }

    private Vector3 oldPos;
    // Update is called once per frame
    void Update()
    {

    }

    float AngleStep() {
        return raycastN == 1 ? 0 : 180 / (raycastN - 1);
    }
    void UpdateGeometry() {
        susDir = transform.TransformVector(susDirRel).normalized;
    }

    public void FixedUpdate() //TODO make raycasts with #OnDrawGizmos() Algorithm
    {
        UpdateGeometry();
        
        Vector3 baseVec = transform.position + susDir * (susDist - wheel.radius);

        if (Physics.Raycast(transform.position, susDir, out hit, susDist))
        {
            compression = susDist - hit.distance;
            compressionVelocity = (compression - oldCompression) / Time.fixedDeltaTime;
            float springForce = -compression * springK; //Spring
            float damperForce = -compressionVelocity * damperK; //Damper
            totalForce = springForce + damperForce;
            totalForce = Mathf.Min(totalForce, 0);
            wheelOnGround = true;
        }
        else
        {
            compression = 0;
            wheelOnGround = false;
        }
        oldCompression = compression;


        Vector3 forcePos = transform.position;
        //rb.AddForceAtPosition(totalForce, forcePos, ForceMode.Force);
        rb.AddForceAtPosition(-hit.normal * totalForce, hit.point, ForceMode.Force);
        wheel.transform.position = transform.position + susDir * (susDist - compression - wheel.radius);

        //wheel.transform.rotation = transform.rotation * Quaternion.Euler(90, 90, 0);
    }
    [ExecuteInEditMode]
    public void OnDrawGizmos()
    {
        UpdateGeometry();
        Gizmos.color = Color.white;

        rb = GetComponentInParent<Rigidbody>();

        Vector3 baseVec = transform.position + susDir * (susDist - wheel.radius);


        Vector3 vec1 = baseVec + new Vector3(0, 0, wheel.radius);
        Vector3 vec2 = baseVec - new Vector3(0, 0, wheel.radius);

        float totalAngle = Vector3.Angle(vec1, vec2); //Degrees
        float angleStep = AngleStep(); //FIXME : Do not copy lines

        for(int i = 0; i < raycastN; i++) //trig in unity returns radians
        {
            float angle = (angleStep * i + 180) * Mathf.Deg2Rad;
            Vector3 vecf = baseVec + transform.TransformVector(new Vector3(0, Mathf.Sin(angle), Mathf.Cos(angle))) * wheel.radius;
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