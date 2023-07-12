using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerControl : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    public float speed;
    float gravity = 0;
    public float mass;

    public bool canMove = true;
    public bool NPCInteract_ = false;
    public bool isDead = false;

    public GameObject listPS;
    public GameObject dust;
    public AudioSource sound;
    public Animator bubbleAnimation;
    public Animator bubbleAnimation_Ship;
    private float distanceMoved;
    
    PhotonView photonView;

    public TextTyping textTyping;

    //bool triggerSwim = false;
    // Start is called before the first frame update
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (photonView.IsMine)
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();

            StartCoroutine(setGravity());
        }    
    }

    IEnumerator setGravity()
    {
        yield return new WaitForSeconds(0.5f);

        gravity = -9.81f;
    }
    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }   
        if(CheckAnim())
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        //Debug.Log(textTyping.isTypingText);
        if (canMove && !textTyping.isTypingText && !NPCInteract_ && !isDead)
        {
            Move();
        }

        if(Input.GetKey(KeyCode.Period) && TimeManager.Instance != null)
        {
            TimeManager.Instance.Tick();
        }
    }

    public void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 velocity = speed * Time.deltaTime * dir;
        float times = Time.deltaTime;
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            distanceMoved += velocity.magnitude;

            if (distanceMoved >= 1.0f) // check if distance moved is greater than threshold
            {
               if(transform.position.y > 0)
                {
                    dustEffect();
                }    
                distanceMoved = 0.0f; // reset distance moved
            }
        }

        //Check run
        if (transform.position.y > 0 || transform.position.y < -800)
        {
            if (Input.GetButton("Sprint"))
            {

                velocity = velocity * 2.0f;
                animator.SetBool("Run", true);


            }
            else
            {
                animator.SetBool("Run", false);
            }
        }

        if(dir.magnitude >= 0.1f)
        {
            //look
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 14 * Time.deltaTime);

            //move
            if(transform.position.y > 0 || transform.position.y < -800)
            {
                controller.Move(velocity);
            }
            else
            {
                controller.Move(velocity*0.5f);
            }          
        }

        //animation
        animator.SetFloat("Speed", velocity.magnitude);
        /*
        else
        {
            //animation bơi
            if(!triggerSwim)
            {
                animator.Play("swim");
                triggerSwim = true;
            }     
        }*/

        //gravity
        velocity.y += gravity * mass * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        /*
        if (transform.position.y > -0.9f)
        {
            velocity.y += gravity * mass * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }*/
    }
    public string CheckAnimString()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        string currentAnimationName = stateInfo.shortNameHash.ToString();

        //Debug.Log(currentAnimationName);
        return (currentAnimationName);
    }
    public bool CheckAnim()
    {
        string curAnimation = CheckAnimString();
        if (curAnimation == "2029649767" ||
           curAnimation == "1759212382" ||
           curAnimation == "637809919" ||
           curAnimation == "-1174223327" ||
           curAnimation == "-526453220" ||
           curAnimation == "-743040919") 
        {
            return true;
        }

        return false;
    }


    public void dustEffect()
    {
            GameObject obj = Instantiate(dust, listPS.transform);
            obj.transform.position = gameObject.transform.position;

            ParticleSystem dustPS = obj.GetComponentInChildren<ParticleSystem>();

            dustPS.Play();
            sound.Play();
            StartCoroutine(disappear(obj,dustPS));
            Destroy(obj, 3f);
    }

    IEnumerator disappear(GameObject obje , ParticleSystem dustSP)
    {
   
        SpriteRenderer spriteRenderer = obje.GetComponentInChildren<SpriteRenderer>();
        float duration = 2.5f; // Time in seconds to fade out the sprite
        float timer = 0.0f; // Timer to track progress
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1.0f, 0.0f, t);
            spriteRenderer.color = color;

    
            yield return null;
        }

    }
    public void NPCiteractoff()
    {
        NPCInteract_ = false;
 
    }
}
