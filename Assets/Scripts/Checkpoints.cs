using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private GameManagerController gm;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        gm= GameObject.FindGameObjectWithTag("GM").GetComponent<GameManagerController>();
        audioSource=this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2d(Collider2D other){
        if(other.tag == "CheckPoint"){
            audioSource.Play();
            gm.lastCheckpointPos = transform.position;
        }
    }
}
