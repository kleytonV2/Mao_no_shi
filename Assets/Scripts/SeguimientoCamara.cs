using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    public GameObject personaje;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate() {
        float posX = personaje.transform.position.x;
        float posY = personaje.transform.position.y;

        transform.position = new Vector3(posX, posY,transform.position.z);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
