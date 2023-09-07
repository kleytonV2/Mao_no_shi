using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlejarCamara : MonoBehaviour
{

   
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().orthographicSize = 7f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
