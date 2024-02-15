using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Impulse_Script : MonoBehaviour
{
    public float force;
    public Rigidbody rigidbody;
    Cinemachine.CinemachineImpulseSource source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Fire()
    {
        rigidbody.AddForce(transform.forward * (100 * Random.Range(0.9f, 2f)), ForceMode.Impulse);
        source = GetComponent<Cinemachine.CinemachineImpulseSource>();

        source.GenerateImpulse(Camera.main.transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
