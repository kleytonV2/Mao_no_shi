using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class enemigoGeneral : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public float weight;
    private Transform target;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private GameObject player;
    private float playerPosition;
    private Animator playerAnimator;
    private CapsuleCollider2D enemyCapsuleCollider;
    private int isEnemyAttacking1;
    private int isEnemyAttacking2;
    private int numeroAleatorio;
    AudioSource audioSource;
    public AudioClip[] listAudio;
    public int controladorAudio;
    public int playerLives;
    public bool inmunidad;
    // Start is called before the first frame update
    void Start()
    {
        inmunidad = false;
        controladorAudio = 0;
        weight =80f;
        speed=100f;
        player=GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.gameObject.GetComponent<Animator>();

        target = player.GetComponent<Transform>();
        animator = this.gameObject.GetComponent<Animator>();
        
        rb =this.gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        enemyCapsuleCollider=this.gameObject.GetComponent<CapsuleCollider2D>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        playerLives = GameManagerController.instance.playerLives;

    }

    // Update is called once per frame
    void Update()
    {
        
        var playerPosition=player.transform.position.x;
        var distancia=transform.position.x-playerPosition;
        //Debug.Log(animator.GetBool(valorDeMorir));
  
        
        if(distancia<0)distancia*=-1;
        animator.SetBool("parado",false);
        animator.SetBool("caminar",false);
        animator.SetBool("ataque1",false);
        animator.SetBool("ataque2",false);
        
            if (transform.position.x > playerPosition && transform.localScale.x >= 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0);
            }
            else
            {
                if (transform.position.x < playerPosition && transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0);
            }
            
      
            if (distancia>1&&distancia<5){
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = listAudio[0];
                    audioSource.Play();
                 
                }
                animator.SetBool("caminar",true);
                transform.position=Vector2.MoveTowards(transform.position,target.position,(speed/weight)*Time.deltaTime);
            }else if(distancia<=3){
                //Debug.Log(numeroAleatorio);
                StartCoroutine(atacar());
                
            }else{
                animator.SetBool("parado",true);
            }

        

    }


    IEnumerator atacar() {
        numeroAleatorio = Random.Range(2, 7);
        yield return new WaitForSeconds(1f);
        if (numeroAleatorio % 2 == 0) {
            
            animator.SetBool("ataque1", false);
            animator.SetBool("ataque2", false);
            animator.SetBool("ataque1", true);
            yield return new WaitForSeconds(1f);

        }
        else{
            animator.SetBool("ataque1", false);
            animator.SetBool("ataque2", false);
            animator.SetBool("ataque2", true);
            yield return new WaitForSeconds(1f);
        }
    }

    /*
     * Controlo aqui el los ataques del enemigo, identificando si el arma de este hace contacto o no con el player
     * Mirar el metodo:  destruiPlayer
     */
    void OnTriggerStay2D(Collider2D objetoEnContacto)
    {
        
        isEnemyAttacking1 = Animator.StringToHash("ataque1");
        isEnemyAttacking2 = Animator.StringToHash("ataque2");
        Debug.Log(objetoEnContacto.gameObject.name == "chicaSamurai");
        //&&(playerAnimator.GetBool(isPlayerAttacking1)||playerAnimator.GetBool(isPlayerAttacking2))
        if (objetoEnContacto.gameObject.name=="chicaSamurai"){//Compruebo si el objeto exterior es el player
            if(animator.GetBool(isEnemyAttacking1)|| animator.GetBool(isEnemyAttacking2))//Compruebo si este ENEMIGO está haciendo el ataque 1 o 2
            {
                StartCoroutine(destruirPlayer());
            }
             
        }
    }
    
    /*
     * Este metodo se encarga de reducir la vida del player en el game manager controller
     */
    void playerDeath()
    {

        if (!inmunidad)
        {
            playerLives -= 1;
            GameManagerController.instance.playerLives -= 1;
            
            if (playerLives <= 0)
            {
                playerAnimator.SetBool("golpeado", false);
                playerAnimator.SetBool("isJumping", false);
                playerAnimator.SetBool("parado", false);
                playerAnimator.SetBool("ataque1", false);
                playerAnimator.SetBool("ataque1", false);
                playerAnimator.SetBool("morir", true);
                Destroy(player.gameObject);
                SceneManager.LoadScene("GameOver");
                GameManagerController.instance.playerLives = 4;
            }
            StartCoroutine(controlarInmunidad());//el personaje se hace inmune al daño
        }
        /*
            GameManagerController.instance.setGameStates(GameStates.gameOver);
        
        if (playerLives > 0)
        {
            GameManagerController.instance.setGameStates(GameStates.playing);
            audioSource.clip = listAudio[4];
            audioSource.Play();
        }*/

    }
    IEnumerator controlarInmunidad()
    {
        inmunidad = true;
        yield return new WaitForSeconds(1f);//el personaje se hace inmune al daño durante 3 segundos
        inmunidad = false;

    }
    /*
     * Se encarga de destruir el sprite del player que acaba de ser atacado por el enemigo actual
     * Mirar el metodo: onTriggerStay2d 
     */
    IEnumerator destruirPlayer()
    {
        if (playerLives >= 1)
        {

            audioSource.clip = listAudio[1];
            audioSource.Play();
            playerAnimator.SetBool("golpeado", true);//en el update del player este tiene la funcion de ir para atras al ser golpeado
            yield return new WaitForSeconds(0.6f);
            playerAnimator.SetBool("golpeado", false);
            playerDeath();
        }

    }




}
