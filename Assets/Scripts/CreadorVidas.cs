using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreadorVidas : MonoBehaviour
{
    public GameObject vida;
    public float tiempoCreacion = 40f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("creando",40f,tiempoCreacion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void creando(){
        Vector3 spawnPosition = new Vector3(Random.Range(-8.3f,8.3f),-3.6f,0);
        GameObject vidas = Instantiate(vida,spawnPosition,Quaternion.identity);
    }
}
