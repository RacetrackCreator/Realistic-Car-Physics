using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Suspension suspension;
    public Car car;
    // Start is called before the first frame update
    void Start()
    {
        car = GetComponentInParent<Car>();
        //suspension.wheel = GetComponent<Wheel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
