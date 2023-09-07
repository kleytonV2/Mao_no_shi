using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameStates{

    menu,
    playing,
    pause,
    gameOver,
    gameWin
}
public class GameManagerController : MonoBehaviour
{

    public static GameManagerController instance;

    public int playerLives = 4;

    public int bossLives = 8;

    public GameStates gameState;

    public Vector2 lastCheckpointPos;

    AudioSource AudioSource;

    public AudioClip[] listAudio; 
    Scene scene;

    
    private void Awake(){
        if(instance==null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(this);
        }
        gameState = GameStates.playing;
    } 

    
    // Start is called before the first frame update
    void Start()
    {
    AudioSource = this.gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "MenuPrincipal")
        {
            AudioSource.clip = listAudio[0];
            if(!AudioSource.isPlaying){
                AudioSource.Play();
            }
        }else if(scene.name == "sceneTutorial"){
            AudioSource.clip = listAudio[1];
            if(!AudioSource.isPlaying){
                AudioSource.Play();
            }
        }else if(scene.name == "2level"){
            AudioSource.clip = listAudio[2];
            if(!AudioSource.isPlaying){
                AudioSource.Play();
            }
        }else if(scene.name == "bossEscenario"){
            AudioSource.clip = listAudio[3];
            if(!AudioSource.isPlaying){
                AudioSource.Play();
            }
        }else{
            AudioSource.Stop();
        }
    
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        
    }

    public GameStates getGameState(){
        return this.gameState;
    }

    public void setGameStates(GameStates _gm){
        this.gameState = _gm;
        gameStateUpdate();
    }

    void gameStateUpdate(){
        switch(gameState){
            case GameStates.pause:
                setGameInPause();
                break;
            case GameStates.gameOver:
                setGameInGameOver();
                break;
            case GameStates.gameWin:
                setGameInGameWin();
                break;
            case GameStates.menu:
                setGameInMenu();
                break;
            default:
                setGameInPlaying();
                break;
        }
    }

    private void setGameInMenu(){
        SceneManager.LoadScene("MenuPrincipal");
        playerLives = 4;
        bossLives = 8;

    }


    private void setGameInPlaying()
    {
        //throw new NotImplementedException();
    }

    private void setGameInGameWin()
    {
        SceneManager.LoadScene("GameWin");
    
        //throw new NotImplementedException();
    }

    private void setGameInGameOver()
    {   
        this.playerLives -=1;
        if(this.playerLives>0){
            Debug.Log("Has mort, te quedan " + this.playerLives);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        }else{

            SceneManager.LoadScene("GameOver");
            playerLives = 4;
        }
        
    }

    public int getPlayerLives(){
        return playerLives;
    }

    private void setGameInPause()
    {
        //throw new NotImplementedException();
    }
}
