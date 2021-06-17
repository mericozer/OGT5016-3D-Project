using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    public AudioSource soundSourceWalk; //walk and run sound
    public AudioSource soundSourceJump; //jump sound
    public AudioClip[] audioList;
    public bool soundPlay = false;
    public bool runSound = false;
    
    [SerializeField] float jumpHeight = 5f; 
    [SerializeField] float gravity = -10f;
    [SerializeField] float speed = 10f;
    [SerializeField] float runningSpeed = 10f;
    [SerializeField] float onGroundSpeed = 10f;
    [SerializeField] float onAirSpeed = 1f;
    [SerializeField] float offset = 0.2f;
    [SerializeField] float mouseSensitivity = 300f;
    [SerializeField] float batteryNormalCost = -1f;
    [SerializeField] float batteryRunCost = -4f;

    private CharacterController charController;

    private Animator anim;

    [SerializeField] private Transform groundCheck; //extra object under the player for ground check
    [SerializeField] private Transform holdingPoint; //point where boxes stands while holding
    [SerializeField] private Transform shootingPoint; //point where boxes stands while holding
    public GameObject holdingObject;
    
    Vector3 velocity;

    private bool isGrounded = false;
    private bool ground = true;
    private bool isHolding = false;
    [SerializeField] private LayerMask groundLayer;

    public void Awake()
    {
        instance = this;
    }

    public void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        charController = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

        //soundSource = GetComponent<AudioSource>();


        //Tryout for saved position but not works properly for now
        /* if (PlayerPrefs.HasKey("XPosition"))
         {
             Debug.Log("IN THE PLAYER CONTROLLER TRANSS");
             transform.position = new Vector3(PlayerPrefs.GetFloat("XPosition"), PlayerPrefs.GetFloat("YPosition"), PlayerPrefs.GetFloat("ZPosition"));
             Debug.Log("IN THE PLAYER CONTROLLER TRANSS AND X IS " + PlayerPrefs.GetFloat("XPosition"));
         }*/

    }
    public void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, offset, groundLayer); //checks if player is on the ground
		
        //to reset gravity velocity after falling to the ground
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -1f; //?
            anim.SetBool("Jump", false);
            //Debug.Log("jump finished");

            if (!ground)
            {
                ground = true;
                ChangeSpeed();
            }

        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //every axis
        Vector3 movementDirection = transform.right * x + transform.forward * z;
        //just forward
        //Vector3 movementDirection =  transform.forward * z;

        //movement 
        charController.Move(movementDirection * speed * Time.deltaTime);

        //if player has a movement at that moment
        //x != 0 || if i want to add horizontal check Dont forget
        if (!anim.GetBool("Jump") && ( z != 0))
        {
            if (!isHolding) //walking or running normaly
            {
                if (Input.GetKey(KeyCode.LeftShift)) //if running
                {
                    if (!runSound) //if run sound is not playing yet
                    {
                        soundSourceWalk.Stop();
                        soundSourceWalk.clip = audioList[1];
                        soundSourceWalk.Play();
                        runSound = true;
                    }
                    
                    anim.SetBool("Run", true);
                    CanvasController.instance.batteryCost = batteryRunCost;
                    speed = runningSpeed;
                }
                else //walking
                {
                    //
                    anim.SetBool("Run", false);
                    anim.SetBool("Walk", true);

                    if (runSound) //if player starts walking after running
                    {
                        soundSourceWalk.Stop();
                        soundSourceWalk.clip = audioList[0];
                        soundSourceWalk.Play();
                        runSound = false;
                    }
                    if (!soundPlay) //if walk sound is not playing yet
                    {
                        soundSourceWalk.clip = audioList[0];
                        soundSourceWalk.Play();
                        soundPlay = true;
                    }
                }
                
            }
            else //walking with a box
            {
                if (runSound)
                {
                    soundSourceWalk.Stop();
                    soundSourceWalk.clip = audioList[0];
                    soundSourceWalk.Play();
                    runSound = false;
                }
                if (!soundPlay)
                {
                    soundSourceWalk.clip = audioList[0];
                    soundSourceWalk.Play();
                    soundPlay = true;
                }
                anim.SetBool("HoldWalk", true);
            }
           
        }
        else //stops walking running
        {
            
            if (runSound)
            {
                soundSourceWalk.clip = audioList[0];
            }
            soundSourceWalk.Stop();
            soundPlay = false;
            runSound = false;
            
            if (!isHolding)
            {
               
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
            }
            else
            {
                anim.SetBool("HoldWalk", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            CanvasController.instance.batteryCost = batteryNormalCost;  //running makes player spend more battery than usual, walking spends less
            speed = onGroundSpeed; //change speed to walking
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && !isHolding) //jumping
        {
           
            anim.SetBool("Jump", true);
            ground = false;
            ChangeSpeed();
            velocity.y = Mathf.Sqrt(jumpHeight * -gravity);
            soundSourceJump.Play();
            
        }

        velocity.y += gravity * Time.deltaTime;

        //impact of gravity
        charController.Move(velocity * Time.deltaTime);

        //mouse rotation
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * MouseX);
        

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            CanvasController.instance.WinState();
        }

        if (other.CompareTag("Bullet"))
        {
            CanvasController.instance.UpdateBatteryPercentage(-10,true);
            Destroy(other.gameObject);
        }
    }

    //hold object state
    public void HoldObject(GameObject obj)
    {
        holdingObject = obj;
        obj.transform.parent = gameObject.transform;
        obj.transform.position = holdingPoint.position;
        anim.SetBool("Hold", true);
        isHolding = true;
    }
    
    public void HoldShootingObject(GameObject obj)
    {
        holdingObject = obj;
        obj.transform.parent = shootingPoint;
        obj.transform.position = shootingPoint.position;
        obj.transform.rotation = shootingPoint.rotation;
        anim.SetBool("Hold", true);
        isHolding = true;
    }
    
    //put object state
    public void PutObject()
    {
        anim.SetBool("Hold", false);
        isHolding = false;
    }

   
    //change speed between jumping and walking
    private void ChangeSpeed()
    {
        if (speed == onGroundSpeed)
        {
            speed = onAirSpeed;
        }
        else
        {
            speed = onGroundSpeed;
        }
    }

    //prevent unnecessary walking sounds 
    public void StopSound()
    {
        soundSourceWalk.Stop();
        soundSourceJump.Stop();
    }
}
