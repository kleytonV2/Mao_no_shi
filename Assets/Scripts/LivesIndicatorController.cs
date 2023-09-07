using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesIndicatorController : MonoBehaviour
{

    private Text myText;

    // Start is called before the first frame update
    void Start()
    {
    
        this.myText = this.gameObject.GetComponent<Text>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
     this.myText.text=GameManagerController.instance.playerLives + "";
        
    }
}
