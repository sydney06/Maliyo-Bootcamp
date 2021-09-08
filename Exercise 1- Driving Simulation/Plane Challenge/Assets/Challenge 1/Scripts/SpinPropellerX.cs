using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPropellerX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotates propeller 50 degree per second around z axis
        transform.Rotate(0, 0, 6000 * Time.deltaTime);
    }
}
