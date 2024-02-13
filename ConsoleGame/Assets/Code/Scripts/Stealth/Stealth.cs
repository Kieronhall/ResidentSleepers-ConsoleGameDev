using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : MonoBehaviour
{
    public int changePerSecond;
    public float detectionValue;
    sensors sensor;

    // Start is called before the first frame update
    void Start()
    {
        detectionValue = 10f;
        sensor = this.gameObject.GetComponent<sensors>();
    }

    // Update is called once per frame
    void Update()
    {
        //Detecting Player In Sensor
        if (sensor.Hit == true && detectionValue > 0f)
        {
            detectionValue -= changePerSecond * Time.deltaTime;
            detectionValue = Mathf.Max(detectionValue, 0f);

        }
        //Reseting Alertness Level
        if (sensor.Hit == false && detectionValue < 10f)
        {
            detectionValue += changePerSecond * Time.deltaTime;
            detectionValue = Mathf.Min(detectionValue, 10f);
        }
        if(sensor.Hit==true && detectionValue == 0f)
        {
            Debug.Log("Player Detected! Chase and Shoot Them!");
        }
    }
}
