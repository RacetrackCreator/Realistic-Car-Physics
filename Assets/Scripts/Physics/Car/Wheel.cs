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
    public bool powered;
    [Space]
    [Header("Debug")]
    public float Torque;

    public Vector3 Axis;
    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 2;
        car = GetComponentInParent<Car>();
        transmission = GetComponentInParent<Transmission>();
        engine = GetComponentInParent<Engine>();
        //suspension.wheel = GetComponent<Wheel>();
    }

    // Update is called once per frame
    void Update()
    {
        Axis = transform.up * Mathf.Sign(AngularVelocity); //FIXME: this is just for cilinder wheels, use other (better) axis later
        
    }

    private void FixedUpdate()
    {
        
    }
    //README: This will calculate wheel torque and then set engine's RPM's to match calculated wheel angular velocity.
    public void ApplyTransmissionTorqueAndResistances() //for now wheels dont slip
    {
       
    }
}
