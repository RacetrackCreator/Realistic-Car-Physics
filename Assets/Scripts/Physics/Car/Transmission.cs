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

    private Engine engine;
    // Start is called before the first frame update
    void Start()
    {
        engine = GetComponent<Engine>();   
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

        TransmittedTorque = engine.CurrentEngineTorque * CurrentGearRatio * FinalDriveRatio;
        TransmittedAngularVelocity = 2 * Mathf.PI * engine.EngineFrequency /
            (FinalDriveRatio * CurrentGearRatio);
    }
}
