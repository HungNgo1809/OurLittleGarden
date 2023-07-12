using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehavior : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent nav;
    public animalHouseComponent animalHouseComponent;
    public int randomTimeA = 1;
    public int randomTimeB = 10;
    
    public Vector3 randomPosition = new Vector3(0,0,0);

    private bool isTakeControl = false;
    private bool isGoingSomeWhere = false;
    private bool isRunningSomeWhere = false;
    private bool isRollingSomewhere = false;
    private bool isNPCEnter = false;
    #region AnimalAction
    public enum AnimalAction
    {
        DoNothing, GoingSomeWhere , Eating , Sitting , RuningSomeWhere, PlayerNearby
    }

    public AnimalAction action;

    public void SwitchStateAction(AnimalAction action_)
    {
        if(!nav.isStopped)
        {
            nav.isStopped = true;
            nav.ResetPath();
        }
      
        switch (action_)
        {
            case AnimalAction.DoNothing:
                int i = Random.Range(0, 3);
              
                if(i == 0)
                {
                    SwitchState(AnimalState.IdleA);
                }
                else if(i == 1)
                {
                    SwitchState(AnimalState.IdleB);
                }
                else if (i == 2)
                {
                    SwitchState(AnimalState.IdleC);
                }

                break;

            case AnimalAction.GoingSomeWhere:
                isGoingSomeWhere = true;
                isTakeControl = true;
                randomPosition = GetRandomPosition(animalHouseComponent.centerPoint.transform.position,animalHouseComponent.limitFromCenterPoint);
                break;
            case AnimalAction.Eating:
                SwitchState(AnimalState.Eat);
                break;
            case AnimalAction.Sitting:
                SwitchState(AnimalState.Sit);
                break;
            case AnimalAction.RuningSomeWhere:
                isRunningSomeWhere = true;
                isTakeControl = true;
                randomPosition = GetRandomPosition(animalHouseComponent.centerPoint.transform.position, animalHouseComponent.limitFromCenterPoint);
                break;
            case AnimalAction.PlayerNearby:
                StartCoroutine(PlayerNearbyAnimation());
                break;
        }

        action = action_;
    }


#endregion


 #region AnimalState
    public enum AnimalState
    {
        IdleA, IdleB , IdleC , Walk, Eat , Fear, Sit , Run, Death
    }

    public AnimalState state;


    public void SwitchState(AnimalState seller)
    {

        switch (seller)
        {
            case AnimalState.IdleA:
                animator.Play("Idle_A");

                break;
            case AnimalState.IdleB:
                animator.Play("Idle_B");

                break;
            case AnimalState.IdleC:
                animator.Play("Idle_C");

                break;
            case AnimalState.Walk:
                //animator.SetBool("isDoneWalking", false);
                animator.Play("Walk");

                break;
            case AnimalState.Eat:
                //animator.SetBool("isDoneWalking", false);
                animator.Play("Eat");

                break;
            case AnimalState.Fear:
                //animator.SetBool("isDoneWalking", false);
                animator.Play("Fear");

                break;
            case AnimalState.Sit:
                //animator.SetBool("isDoneWalking", false);
                animator.Play("Sit");

                break;
            case AnimalState.Run:
                //animator.SetBool("isDoneWalking", false);
                animator.Play("Run");

                break;
            case AnimalState.Death:
                //animator.SetBool("isDoneWalking", false);
                animator.Play("Death");

                break;
        }

        state = seller;
    }
    #endregion




    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoingSomeWhere)
        {
            GoingSomeWhereAnimal();
            float distance = Vector3.Distance(gameObject.transform.position, randomPosition);
            if (distance < 0.4f)
            {
                isGoingSomeWhere = false;
                isTakeControl = false;
                SwitchStateAction(AnimalAction.DoNothing);
                StartCoroutine(ChangeActionCoroutine(randomTimeA, randomTimeB));
                nav.avoidancePriority = 50;
            }

        }
        if (isRunningSomeWhere)
        {
            RunningSomeWhereAnimal();
            float distance = Vector3.Distance(gameObject.transform.position, randomPosition);
            if (distance < 0.4f)
            {
                isRunningSomeWhere = false;
                isTakeControl = false;
                SwitchStateAction(AnimalAction.DoNothing);
                StartCoroutine(ChangeActionCoroutine(randomTimeA, randomTimeB));
                nav.avoidancePriority = 50;
            }

        }
        /*
        if (isRollingSomewhere)
        {
            RollingSomeWhereAnimal();
            float distance = Vector3.Distance(gameObject.transform.position, randomPosition);
            if (distance < 0.4f)
            {
                isRollingSomewhere = false;
                isTakeControl = false;
                SwitchStateAction(AnimalAction.DoNothing);
                StartCoroutine(ChangeActionCoroutine(randomTimeA, randomTimeB));
                nav.avoidancePriority = 50;
            }

        }
        */
    }

    public void GoingSomeWhereAnimal()
    {
        nav.avoidancePriority = 49;
        SwitchState(AnimalState.Walk);
        nav.SetDestination(randomPosition);

    }
    public void RunningSomeWhereAnimal()
    {
        nav.avoidancePriority = 49;
        SwitchState(AnimalState.Run);
        nav.SetDestination(randomPosition);

    }
    /*
    public void RollingSomeWhereAnimal()
    {
        nav.avoidancePriority = 49;
        SwitchState(AnimalState.Roll);
        nav.SetDestination(randomPosition);

    }
    */
    private IEnumerator ChangeActionCoroutine(int begin , int end)
    {
      
        yield return new WaitForSeconds(Random.Range(begin, end));

    
        AnimalAction newAction = (AnimalAction)Random.Range(0, 5); // hien tai co 2 action nen = 2
        SwitchStateAction(newAction);

     
        if(isTakeControl != true)
        {
            StartCoroutine(ChangeActionCoroutine(begin, end));
        }
        
    }

    public Vector3 GetRandomPosition(Vector3 center, float range)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, range, NavMesh.AllAreas) && hit.position.y <= 2)
            {
                //Debug.Log(hit.position);
                return hit.position;

            }
        }
        Debug.LogWarning("Sau khi 10 lan thu co diem nam ngoai vi tri di duoc");
        return center;
    }

    IEnumerator PlayerNearbyAnimation()
    {
        SwitchState(AnimalState.Fear);
        yield return new WaitForSeconds(2f);
        SwitchStateAction(AnimalAction.RuningSomeWhere);
    }


    public IEnumerator PlayDeath()
    {
        StopAllCoroutines();
        isTakeControl= true;
        SwitchState(AnimalState.Death);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
     
        if (other.tag == "Player")
        {
            isTakeControl = true;
            SwitchStateAction(AnimalAction.PlayerNearby);
            
        }
        if (other.tag == "NPC" && !isNPCEnter)
        {
            isTakeControl = true;
            SwitchStateAction(AnimalAction.PlayerNearby);
            isNPCEnter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC" )
        {
            isNPCEnter = false;
        }
    }
}
