using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Suspension suspension;
    public Car car;
    public Engine engine;
    public Transmission transmission;
    public CarProperties properties;
    public float AngularVelocity;
    public float radius;
    public float latSpeed;
    public float longSpeed;
    public float deformationSpeed;
    public float deformation;
    public float latFrictionForce;
    [Space]
    [Header("Debug")]
    public float Torque;
    public Vector3 Force;
    public Vector3 Axis;
    public bool colliding;
    public Vector3 point;
    public float maxForce;
    public bool drifting;
    public float corneringAngle;

    [Header("Constants")]
    public float Cf;
    public float Kf;
    public float mu;
    public float relaxationConst;
    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 2;
        //suspension.wheel = GetComponent<Wheel>();
        if(transmission)
            transmission.wheels.Add(GetComponent<Wheel>());
        suspension = GetComponentInParent<Suspension>();
        car = GetComponentInParent<Car>();
        properties = car.GetComponentInChildren<CarProperties>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        Axis = transform.up; //FIXME: this is just for cilinder wheels, use other (better) axis later

        Cf = properties.Cf;
        Kf = properties.Kf;
        mu = properties.mu;
        relaxationConst = properties.relaxationConst;

        if(transmission)
        {
            Torque = transmission.TransmittedTorque;
            Force = Vector3.Cross(Axis, suspension.hit.normal).normalized * Torque / radius;
            car.rb.AddForceAtPosition(Force, suspension.hit.point);
        }
        if(suspension.wheelOnGround)
        {
            Vector3 patchSpeed = car.rb.GetPointVelocity(suspension.hit.point);
            latSpeed = Vector3.Dot(patchSpeed, Axis);
            Vector3 forward = Vector3.Cross(Axis, suspension.hit.normal);
            longSpeed = Mathf.Abs(Vector3.Dot(patchSpeed, forward));
            corneringAngle = Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan2(latSpeed, longSpeed));
            deformationSpeed = latSpeed - relaxationConst * longSpeed * deformation;
            deformation += deformationSpeed * Time.fixedDeltaTime;
            latFrictionForce = -Kf * deformation - Cf * deformationSpeed;
            maxForce = -suspension.totalForce * mu;
            drifting = false;
            if (Mathf.Abs(latFrictionForce) > maxForce)
            {
                drifting = true;
                latFrictionForce = maxForce * Mathf.Sign(latFrictionForce);
                deformation = -(latFrictionForce + Cf * latSpeed) / Kf;
            }
            car.rb.AddForceAtPosition(latFrictionForce * Axis, suspension.hit.point);
        }
        else
            deformation = 0;
    }
    //README: This will calculate wheel torque and then set engine's RPM's to match calculated wheel angular velocity.
    public void OnDrawGizmos() //for now wheels dont slip
    {
        if(suspension){
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Axis * 3);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(suspension.hit.point, car.rb.GetPointVelocity(suspension.hit.point));

            Gizmos.color = Color.yellow;
            if (drifting)
                Gizmos.DrawSphere(transform.position, radius);
                
        }
    }
}
