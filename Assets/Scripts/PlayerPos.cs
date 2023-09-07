using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{

    private GameManagerController gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManagerController>();
        transform.position = gm.lastCheckpointPos;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
