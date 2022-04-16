using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    public CarProperties properties;
    public float maxAngle;
    public float currentAngle;
    public float wheelBase;
    private Suspension suspension;
    // Start is called before the first frame update
    void Start()
    {
        suspension = GetComponentInParent<Suspension>();
        properties = GetComponentInParent<Car>().GetComponentInChildren<CarProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        wheelBase = properties.wheelBase;
        float centerAngle = Input.GetAxis("Horizontal") * maxAngle;
        currentAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (wheelBase / Mathf.Tan(Mathf.Deg2Rad * centerAngle) - suspension.transform.localPosition.x));
        transform.localEulerAngles = new Vector3(0, currentAngle, 0);
    }
}
