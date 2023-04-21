using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;

    public float speed;
    public float gravity = -9.81f;
    public float mass;

    public bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(CheckAnim())
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        if (canMove)
        {
            Move();
        }

        if(Input.GetKey(KeyCode.O))
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

        //Check run
        if(Input.GetButton("Sprint"))
        {
            velocity = velocity * 2.0f;
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if(dir.magnitude >= 0.1f)
        {
            //look
            transform.rotation = Quaternion.LookRotation(dir);

            //move
            controller.Move(velocity);
        }

        //animation
        animator.SetFloat("Speed", velocity.magnitude);

        //gravity
        velocity.y += gravity * mass * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
}
