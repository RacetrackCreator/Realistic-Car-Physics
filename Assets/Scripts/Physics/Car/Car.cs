using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Vector3 u;
    private Rigidbody rb;


    [Header("Body")]
    public float CoeffitientOfFriction;
    public float FrontalArea;

    [Header("Wheels")]
    public Wheel[] wheels;

    [Header("Constants")]
    public float AerodynamicDragConst;
    public float RollingDragConst;

    [Header("Debug")]
    public Vector3 TotalForce;
    [Space()]
    public Vector3 LongitudinalForce;
    [Space()]
    public Vector3 AerodynamicDrag;
    public Vector3 RollingDrag;
    public Vector3 RollingResistance;
    public Vector3 TractionForce;
    public Vector3 BrakingForce;
    [Space()]
    public Vector3 Velocity;
    public int PoweredWheels = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AerodynamicDragConst = 0.5f * CoeffitientOfFriction * FrontalArea * Constants.AirDensity;
        RollingDragConst = 30 * AerodynamicDragConst;

        wheels = GetComponentsInChildren<Wheel>();

        foreach(Wheel wheel in wheels)
        {
            if (wheel.powered)
                PoweredWheels++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        u = new Vector3(transform.forward.x, 0, transform.forward.z);
        
        Velocity = rb.velocity;
    }

    private void FixedUpdate()
    {
        //TractionForce = u.normalized * CurrentEngineForce; NOTE: Switching to new thesis paper
        //AerodynamicDrag = -AerodynamicDragConst * Velocity * Velocity.magnitude; //Fdrag = Cdrag * v * |v|
        //RollingResistance = -RollingDragConst * Velocity;

        //LongitudinalForce = TractionForce + BrakingForce + AerodynamicDrag + RollingResistance;

        ApplyWheelForces();

        TotalForce = LongitudinalForce;
        rb.AddForce(TotalForce);

    }

    private void ApplyWheelForces()
    {
        foreach (Wheel wheel in wheels)
        {
            // -- Traction -- //
            wheel.ApplyTransmissionTorqueAndResistances();
        }
    }
}
