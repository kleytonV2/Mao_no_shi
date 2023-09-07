using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{

    public Scrollbar healthBar;

    public float health=100f;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = this.gameObject.GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar.size<=0){
            GameManagerController.instance.setGameStates(GameStates.gameWin);
        }
        
    }

    public void damage(float value){
        health -= value; 
        healthBar.size = health/100f;
    }
}
