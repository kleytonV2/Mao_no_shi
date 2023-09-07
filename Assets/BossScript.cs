using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private Animator animator;
    public float speed;
    public float weight;
    private Transform target;
    private Transform targetLimiteDerecho;
    private Transform targetLimiteIzquierdo;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private GameObject player;
    private GameObject limiteDerecho;
    private GameObject limiteIzquierdo;
    float distanciaDerecha;
    float distanciaIzquierda;
    private int isBossAttacking1;
    private int isBossAttacking2;
    public int playerLives;

    AudioSource audioSource;
    public AudioClip[] listAudio;
    public bool inmunidad;
    private float playerPosition;
    private Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        inmunidad = false;
        weight=80f;
        speed=400f;
        player=GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.gameObject.GetComponent<Animator>();

        limiteDerecho = GameObject.FindGameObjectWithTag("limiteDerecho");
        limiteIzquierdo=GameObject.FindGameObjectWithTag("limiteIzquierdo");
        target = player.GetComponent<Transform>();
        targetLimiteDerecho=limiteDerecho.GetComponent<Transform>();
        targetLimiteIzquierdo=limiteIzquierdo.GetComponent<Transform>();
        animator = this.gameObject.GetComponent<Animator>();
        rb=this.gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        animator.SetFloat("distancia", 20f);//para que el boss empiece parado en c¡vez de ataxando
        playerLives = GameManagerController.instance.playerLives;
        audioSource = this.gameObject.GetComponent<AudioSource>();
       

    }

    // Update is called once per frame
    void Update()
    {
        
        distanciaIzquierda=transform.position.x-limiteIzquierdo.transform.position.x;
        distanciaDerecha=transform.position.x-limiteDerecho.transform.position.x;

        StartCoroutine(ataqueVolador());

        if (animator.GetFloat("distancia") > 16)
        {
            animator.SetBool("atacar", false);
            audioSource.clip = listAudio[0];
            audioSource.Play();
        }
        else {
            animator.SetBool("atacar", true);
        }
        /*animator.SetBool("morir",false);
        
        animator.SetBool("golpeado",false);
       */

    } 

    IEnumerator ataqueVolador(){

        if(distanciaDerecha<0)distanciaDerecha*=-1;
        if(distanciaIzquierda<0)distanciaIzquierda*=-1;

        //Debug.Log(distanciaIzquierda);
        if((distanciaIzquierda+2)>18){
            animator.SetFloat("distancia", distanciaIzquierda-4f);
            //transform.position = new Vector3(transform.position.x, -1f, transform.position.z);
            if (transform.localScale.x<0){
                animator.SetBool("parado",true);
                transform.localScale=new Vector3(-transform.localScale.x,transform.localScale.y,0);
            }
            yield return new WaitForSeconds(5.0f);
            transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
            animator.SetBool("parado",false);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetLimiteIzquierdo.position.x + 2f, targetLimiteIzquierdo.position.y), (speed / weight) * Time.deltaTime);

        } 
        else if((distanciaDerecha+2)>18){
            animator.SetFloat("distancia", distanciaDerecha-4f);
            //transform.position = new Vector3(transform.position.x, -1f, transform.position.z);
            if (transform.localScale.x>0){
                animator.SetBool("parado",true);
                transform.localScale=new Vector3(-transform.localScale.x,transform.localScale.y,0);
            }
            yield return new WaitForSeconds(5.0f);
            transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
            animator.SetBool("parado",false);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetLimiteDerecho.position.x - 2f, targetLimiteDerecho.position.y), (speed / weight) * Time.deltaTime);
        }

        

    }

    void OnTriggerStay2D(Collider2D objetoEnContacto)
    {

        isBossAttacking1 = Animator.StringToHash("atacar");
        //isBossAttacking2 = Animator.StringToHash("ataque2");
        //Debug.Log(enemyCapsuleCollider.GetType());
        //&&(playerAnimator.GetBool(isPlayerAttacking1)||playerAnimator.GetBool(isPlayerAttacking2))
        if (objetoEnContacto.gameObject.name == "chicaSamurai")
        {//Compruebo si el colider del objeto exterior es un capsulecollider2d colider
            Debug.Log(animator.GetFloat(isBossAttacking1));
            if (animator.GetBool(isBossAttacking1))//Compruebo si el BOSS está atacando
            {
                Debug.Log("Segundo if");
                StartCoroutine(destruirPlayer());
            }

        }
    }

    void playerDeath()
    {
        if (!inmunidad) {
            playerLives -= 1;
            GameManagerController.instance.playerLives -= 1;
            
            if (playerLives <= 0) {
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
        yield return new WaitForSeconds(3f);//el personaje se hace inmune al daño durante 3 segundos
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
