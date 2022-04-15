using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Suspension suspension;
    public Car car;
    public Engine engine;
    public Transmission transmission;
    public float AngularVelocity;
    public float radius;
    [Space]
    [Header("Debug")]
    public float Torque;
    public Vector3 Force;
    public Vector3 Axis;
    public bool colliding;
    public Vector3 point;
    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 2;
        //suspension.wheel = GetComponent<Wheel>();
        if(transmission)
            transmission.wheels.Add(GetComponent<Wheel>());
        suspension = GetComponentInParent<Suspension>();
        car = GetComponentInParent<Car>();
    }

    // Update is called once per frame
    void Update()
    {
        Axis = transform.up * Mathf.Sign(AngularVelocity); //FIXME: this is just for cilinder wheels, use other (better) axis later
        
    }

    private void FixedUpdate()
    {
        if(transmission)
        {
            Torque = transmission.TransmittedTorque;
            Force = Vector3.Cross(Axis, suspension.hit.normal).normalized * Torque / radius;
            car.rb.AddForceAtPosition(Force, suspension.hit.point);
        }
    }
    //README: This will calculate wheel torque and then set engine's RPM's to match calculated wheel angular velocity.
    public void ApplyTransmissionTorqueAndResistances() //for now wheels dont slip
    {

    }
}
