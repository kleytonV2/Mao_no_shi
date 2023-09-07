using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlejarCamera : MonoBehaviour
{
    // Start is called before the first frame update
        void Start()
    {
        GetComponent<Camera>().orthographicSize = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
