using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersonatgePrincipalMovement : MonoBehaviour
{

    private Vector3 position;

    public float weight;

    public int playerLives;

    public float speed;

    private SpriteRenderer spriteRenderer;

    private Animator animator;

    private Rigidbody2D rb;

    public float jumpForce = 5f;

    public LayerMask layerCollisionGround;

    public Transform marcadorSuelo;

    public bool estaEnSuelo;

    private bool dobleSalto;

    // public BarraDeVida barraVida;

    public AudioClip[] listAudio;

    private AudioSource audioSource;

    float timerLength = 5.0f;

    public BarraDeVida barraVida;

    private int isPlayerAttacking1;
    private int isPlayerAttacking2;

    private GameObject enemy;

    private CapsuleCollider2D playerCapsuleCollider;

    float timerTimePassed = 0.0f;

    bool runTimer = false;

    public bool estaQuieto = true;

    private GameObject boss;
    private Animator bossAnimator;




    void Start()
    {
        playerCapsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        
        if (GameObject.FindGameObjectWithTag("Boss")) {
            boss = GameObject.FindGameObjectWithTag("Boss");
            bossAnimator = boss.gameObject.GetComponent<Animator>();
        }
        
        
        Application.targetFrameRate = 120;
        speed = 200f;
        playerLives = GameManagerController.instance.playerLives;
        weight = 40f;
        Application.targetFrameRate = 120;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        animator = this.gameObject.GetComponent<Animator>();
        layerCollisionGround = LayerMask.GetMask("Ground");
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        estaEnSuelo = Physics2D.Raycast(marcadorSuelo.position, Vector2.down, 0.15f);
        //estaQuieto = rb.IsSleeping();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerController.instance.getGameState() == GameStates.playing) {

            

            var displacement = new Vector3(Input.GetAxis("Horizontal"), 0);
            transform.position += displacement * (speed / weight) * Time.deltaTime;
            //Debug.Log(Mathf.Abs(displacement.x));
            animator.SetFloat("Position", Mathf.Abs(displacement.x));

            if (displacement.x < 0 && transform.localScale.x >= 0) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0);
            } else {
                if (displacement.x > 0 && transform.localScale.x <= 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0);
            }

            if (estaEnSuelo) {
                dobleSalto = false;
            }

            animator.SetBool("isJumping", false);
            if (Input.GetKeyDown(KeyCode.W) && estaEnSuelo) {
                playerJump();
                animator.SetBool("isJumping", true);


            }

            if (Input.GetKeyDown(KeyCode.W) && !dobleSalto && !estaEnSuelo) {
                dobleSalto = true;
                playerJump();
                animator.SetBool("isJumping", true);
            }

            //ATAQUES
            animator.SetBool("ataque1", false);
            animator.SetBool("ataque2", false);
            if (Input.GetKeyDown(KeyCode.J)) {
                animator.SetBool("ataque1", true);
                audioSource.clip = listAudio[1];
                audioSource.Play();
            }
            else if (Input.GetKeyDown(KeyCode.I)) {
                animator.SetBool("ataque2", true);
                audioSource.clip = listAudio[2];
                audioSource.Play();
            }

            if (runTimer) {
                timerTimePassed += Time.deltaTime;
                if (timerTimePassed >= timerLength) {
                    timerTimePassed = 0f;
                    runTimer = false;
                    speed = 200f;
                }
            }

            animator.SetBool("golpeado", false);



        }
    }


    void OnTriggerEnter2D(Collider2D target) {
        if (target.tag == "Caida" || target.tag == "pinchos") {
            playerDeath();
        } else if (target.tag == "segundoEscenario") {
            target.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("2level");

        } else if (target.tag == "bossEscenario") {
            target.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("bossEscenario");
        } else if (target.tag == "Vida") {
            audioSource.clip = listAudio[6];
            audioSource.Play();
            cogerVida();
        } else if (target.tag == "mud") {
            weight = 70f;
            rb.gravityScale = 1.5f;
        } else if (target.tag == "CheckPoint") {
            audioSource.clip = listAudio[3];
            audioSource.Play();
            GameManagerController.instance.lastCheckpointPos = transform.position;
        } else if (target.tag == "PowerUp") {
            audioSource.clip = listAudio[5];
            audioSource.Play();
            runTimer = true;
            speed = 350f;
        }
    }

    void OnTriggerExit2D(Collider2D target) {
        if (target.tag == "mud") {
            weight = 40f;
            rb.gravityScale = 1f;
        }

    }



    void cogerVida() {
        GameManagerController.instance.playerLives += 1;
        this.playerLives += 1;
    }

    void playerDeath() {

        animator.SetBool("golpeado", true);
        this.playerLives -= 1;
        GameManagerController.instance.setGameStates(GameStates.gameOver);

        if (playerLives > 0) {
           // Debug.Log("Pasooooo");
            GameManagerController.instance.setGameStates(GameStates.playing);
            audioSource.clip = listAudio[4];
            audioSource.Play();
        }
        //barraVida.damage(10f);
    }

    


    void playerJump() {
        audioSource.clip = listAudio[0];
        audioSource.Play();
        
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    IEnumerator playDeathSound(float waitTime) {
        yield return new WaitForSeconds(waitTime);
       // Debug.Log(Time.time);
    }

    /*
     * Controlo aqui el los ataques del jugador, identificando si el arma de este hace contacto o no con el enemigo en cuestion
     * Mirar el metodo:  destruiEnemigo
     */
    void OnTriggerStay2D(Collider2D enemyEnContacto)
    {

        //&&(playerAnimator.GetBool(isEnemyAttacking1)||playerAnimator.GetBool(isEnemyAttacking2))

        isPlayerAttacking1 = Animator.StringToHash("ataque1");
        isPlayerAttacking2 = Animator.StringToHash("ataque2");
        //Debug.Log(enemyEnContacto.gameObject.name);
       
            
        if (animator.GetBool(isPlayerAttacking1) || animator.GetBool(isPlayerAttacking2))
        {
            if (enemyEnContacto.gameObject.tag == "Boss")
            {
                bossDeath(enemyEnContacto.gameObject);
            }
            else if (enemyEnContacto.gameObject.tag == "Enemy")
            {
                StartCoroutine(destruirEnemigo(enemyEnContacto.gameObject));//Le paso por parametro al metodo que se encarga de destruir los sprites, al enemigo actual que hizo contacto con el arma del player
            }

        }
            
    }
        
    /*
     * Este metodo simplemente hace que se active la animacion "golpeado" del boss durante 0.2 segundos
     */
    IEnumerator bossGolpeado(){
        bossAnimator.SetBool("golpeado",true);
        yield return new WaitForSeconds(0.2f);
        bossAnimator.SetBool("golpeado", false);
    }

    /*
     * Se encarga de reducir la vida del boss y destruirlo si su vida llega a 0
     * Mirar metodos: bossGolpeado, onTriggerStay2d
     */
    void bossDeath(GameObject boss)
    {
        barraVida.damage(10f);
        StartCoroutine(bossGolpeado());
        audioSource.clip = listAudio[7];
        audioSource.Play();
        if (barraVida.healthBar.size == 0)
        {
            // Debug.Log("Pasooooo");
            GameManagerController.instance.setGameStates(GameStates.gameWin);
            audioSource.clip = listAudio[4];
            audioSource.Play();
            Destroy(boss);
        }


    }

    /*
     * Se encarga de destruir el sprite del enemigo que acaba de ser atacado por el player
     * Mirar el metodo: onTriggerStay2d 
     */
    IEnumerator destruirEnemigo(GameObject enemigoAdestruir)
    {
        Debug.Log(enemigoAdestruir.name);
        audioSource.clip = listAudio[4];
        audioSource.Play();
        enemigoAdestruir.GetComponent<Animator>().SetBool("morir", true);
        yield return new WaitForSeconds(0.7f);
        enemigoAdestruir.GetComponent<Animator>().SetBool("morir", false); 
        Destroy(enemigoAdestruir);
    }
    

}
