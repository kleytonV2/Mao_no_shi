using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptEnemigo3 : MonoBehaviour
{
    private Animator animator;
    public float speed;
    public float weight;
    private Transform target;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private GameObject player;
    private float playerPosition;
    private Animator playerAnimator;
    private CapsuleCollider2D enemyCapsuleCollider;
    private int isPlayerAttacking1;
    private int isPlayerAttacking2;
    private int numeroAleatorio;
    AudioSource audioSource;
    public AudioClip[] listAudio;
    public int controladorAudio;
    // Start is called before the first frame update
    void Start()
    {
        controladorAudio = 0;
        weight = 80f;
        speed = 100f;
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.GetComponent<Transform>();
        animator = this.gameObject.GetComponent<Animator>();
        playerAnimator = player.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        enemyCapsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerPosition = player.transform.position.x;
        var distancia = transform.position.x - playerPosition;
        var valorDeMorir = Animator.StringToHash("morir");
        //Debug.Log(animator.GetBool(valorDeMorir));


        if (distancia < 0) distancia *= -1;
        animator.SetBool("parado", false);
        animator.SetBool("caminar", false);
        animator.SetBool("ataque1", false);
        animator.SetBool("ataque2", false);


        if (!(animator.GetBool(valorDeMorir)))
        {
            if (transform.position.x > playerPosition && transform.localScale.x >= 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0);
            }
            else
            {
                if (transform.position.x < playerPosition && transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0);
            }


            if (distancia > 1 && distancia < 5)
            {
                if (!audioSource.isPlaying && controladorAudio == 0)
                {
                    /*
                    audioSource.clip = listAudio[0];
                    audioSource.Play();
                    controladorAudio += 1;*/
                }
                animator.SetBool("caminar", true);
                transform.position = Vector2.MoveTowards(transform.position, target.position, (speed / weight) * Time.deltaTime);
            }
            else if (distancia <= 2)
            {
                //Debug.Log(numeroAleatorio);
                StartCoroutine(atacar());

            }
            else
            {
                animator.SetBool("parado", true);
            }
        }
        
    }
    IEnumerator atacar()
    {
        numeroAleatorio = Random.Range(2, 7);
        if (numeroAleatorio % 2 == 0)
        {
            /*
            audioSource.clip = listAudio[0];
            audioSource.Play();
            yield return new WaitForSeconds(0.1f);
            */
            animator.SetBool("ataque1", false);
            animator.SetBool("ataque2", false);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("ataque1", true);
            yield return new WaitForSeconds(0.5f);

        }
        else
        {
            /*
            audioSource.clip = listAudio[0];
            audioSource.Play();
            yield return new WaitForSeconds(0.1f);
            */
            animator.SetBool("ataque1", false);
            animator.SetBool("ataque2", false);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("ataque2", true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnTriggerStay2D(Collider2D objetoEnContacto)
    {

        isPlayerAttacking1 = Animator.StringToHash("ataque1");
        isPlayerAttacking2 = Animator.StringToHash("ataque2");
        Debug.Log(enemyCapsuleCollider.GetType());
        //&&(playerAnimator.GetBool(isPlayerAttacking1)||playerAnimator.GetBool(isPlayerAttacking2))
        if (objetoEnContacto.GetType() == typeof(BoxCollider2D))
        {
            if (playerAnimator.GetBool(isPlayerAttacking1) || playerAnimator.GetBool(isPlayerAttacking2))
            {
                StartCoroutine(destruirEnemigo());
            }

        }
    }

    IEnumerator destruirEnemigo()
    {
        audioSource.clip = listAudio[1];
        audioSource.Play();
        animator.SetBool("morir", true);
        yield return new WaitForSeconds(0.7f);
        Destroy(this.gameObject);
    }
}
