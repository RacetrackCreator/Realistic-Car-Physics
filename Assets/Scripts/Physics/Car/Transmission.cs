using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmission : MonoBehaviour
{

    public float[] GearRatios;
    public float FinalDriveRatio;
    public int gearI = 0;

    public float CurrentGearRatio;

    public float TransmittedTorque;
    public float TransmittedAngularVelocity;

    public List<Wheel> wheels = new List<Wheel>();
    public List<Engine> engines = new List<Engine>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            gearI++;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            gearI--;
        gearI = Mathf.Clamp(gearI, 0, GearRatios.Length-1);

        CurrentGearRatio = GearRatios[gearI];
        if (Input.GetKeyDown(KeyCode.A))
            print("aaaaaaaaa");
        print(Input.inputString);

        
    }

    void FixedUpdate()
    {
        TransmittedTorque = engines[0].CurrentEngineTorque * CurrentGearRatio * FinalDriveRatio / wheels.Count;
        TransmittedAngularVelocity = 0;
    }
}
