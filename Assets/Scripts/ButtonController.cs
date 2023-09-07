using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audioSource;
    void Start(){
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }
    public void loadSceneTutorial(){
        audioSource.Play();
        GameManagerController.instance.setGameStates(GameStates.playing);
        SceneManager.LoadScene("sceneTutorial");
        
        
    }

    public void loadMenuPrincipal(){
        audioSource.Play();
        GameManagerController.instance.setGameStates(GameStates.menu);

        
    }

}
