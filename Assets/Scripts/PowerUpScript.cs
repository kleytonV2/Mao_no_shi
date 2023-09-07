using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"){
            Destroy(this.gameObject);
        }
    }
}
