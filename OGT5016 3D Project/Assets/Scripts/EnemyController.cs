using System.Collections;
using System.Collections.Generic;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    //Plays enemy attack and shout
    public AudioSource soundSource;
    public AudioClip[] audioList;

    [SerializeField] private float maxRange = 20f; //range for enemy vision
    [SerializeField] private float attackRange = 20f; //range for attack
    private float attackTimer = 3f; //every 3 seconds, enemy fires an electric bullet
    
    private float shoutTimer = 4f; //if it in range, enemy shouts to the player
    
    private GameObject target; //player object

    [SerializeField] private Transform initialPoint; //enemy ai patrol initial point
    [SerializeField] private Transform endPoint; //enemy ai patrol end point

    [SerializeField] private GameObject electricBullet;
    [SerializeField] private GameObject shotPoint; //barrel point
    
    private bool isGoingInitial = true; //checks if ai goes to the initial patrol point

    private NavMeshAgent navAI;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        
        navAI = GetComponent<NavMeshAgent>();
        target = PlayerController.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targerVector = target.transform.position - transform.forward;
        float angle = Vector3.Angle(targerVector, transform.forward);
        float distance = Vector3.Distance(target.transform.position, transform.position);
        
        if (distance <= maxRange)
        {
            if (angle <= 180) //enemy sees tthe player
            {
                Shout(); //shout sound
                
                Chase(); 
                
                if (distance <= attackRange) //player in the attack range
                {
                    Debug.Log("EXTERMINATE");
                    Attack();
                }
            }
            else //player is in the enemy range but enemy doesnt see the player
            {
                Patrol();
            }
        }
        else //player is not in the enemy range
        {
            Patrol();
        }

        
    }

    //shout sound plays in every 4 seconds 
    public void Shout()
    {
        shoutTimer += Time.deltaTime;
        if (shoutTimer >= 4f)
        {
            shoutTimer = 0;
            soundSource.PlayOneShot(audioList[0]);
        }
    }

    //enemy attack in every 3 seconds
    public void Attack()
    {
        
        attackTimer += Time.deltaTime;
        if (attackTimer >= 3f)
        {
            attackTimer = 0;
            
            soundSource.PlayOneShot(audioList[1]);
            //CanvasController.instance.UpdateBatteryPercentage(-10f,true);
            GameObject instCannon = Instantiate(electricBullet, shotPoint.transform.position, Quaternion.identity);
            Rigidbody instBulletRB = instCannon.GetComponent<Rigidbody>();
            instBulletRB.AddForce(shotPoint.transform.forward * 3000f, ForceMode.Force);
            
            
        }
        
        
    }

    //sets destination to player
    private void Chase()
    {
        navAI.SetDestination((target.transform.position));
    }

    //sets destination for patrolling
    private void Patrol()
    {
        Vector3 destination;

        if (isGoingInitial)
        {
            navAI.SetDestination(initialPoint.position);
            destination = transform.position - initialPoint.position;

            if (destination.magnitude < 6f)
            {
                isGoingInitial = false;
            }
        }
        else
        {
            navAI.SetDestination(endPoint.position);
            destination = transform.position - endPoint.position;

            if (destination.magnitude < 6f)
            {
                isGoingInitial = true;
            }
        }
    }

    //shows up enemy ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , maxRange);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position , attackRange);
    }
}
