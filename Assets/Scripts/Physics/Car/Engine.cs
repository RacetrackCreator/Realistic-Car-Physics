using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour // Were gonna use Metric System everywhere for the good of us all
{
    public float EngineForce;
    public float CurrentEngineForce;

    public float CurrentEngineTorque;

    public long EngineFrequency;

    public float RPM;

    public float accelPedal; //0 - 1

    private const float constTorque = 50f;

    public Transmission transmission;
    // Start is called before the first frame update
    void Start()
    {
        transmission.engines.Add(GetComponent<Engine>());   
    }

    // Update is called once per frame
    void Update()
    {
        RPM = EngineFrequency / 60;
        accelPedal = Input.GetKey(KeyCode.W) ? 1 : 0;  // for now
        //CurrentEngineTorque = accelPedal * TorqueCurve(EngineFrequency);
        CurrentEngineTorque = accelPedal * constTorque;
    }

    float TorqueCurve(float hz) //looking at rmp graph so converting Hz to rpm
    {
        if (hz < 250 * 60)
            return 0;
        if ((250 * 60) <= hz && hz < (1600 * 60))
            return .074f * hz + 1.481f;
        if ((1600 * 60) <= hz && hz < (3000 * 60))
            return .014f * hz + 97.143f;
        if ((3000 * 60) <= hz && hz < (5000 * 60))
            return .005f * hz + 125;
        if ((5000 * 60) <= hz && hz < (7500 * 60))
            return -.012f * hz + 210;
        if ((7500 * 60) < hz && hz < (8000 * 60))
            return -.04f * hz + 420;
        print("too much revolutions");
        return 120;
    }
}
 