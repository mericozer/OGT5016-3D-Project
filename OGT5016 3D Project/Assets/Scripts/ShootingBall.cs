using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBall : MonoBehaviour
{
    public enum BallColor
    {
        None,
        Red,
        Green,
        Yellow,
        Blue
    }

    public BallColor color;
    
    private bool playerNear = false;
    private bool inSequence = false;
    private bool launched = false;
    private bool inCircle = false;
    
    private float checkTime = 0f;

    [SerializeField] private Collider coll;

    [SerializeField] private ShootProjection projection;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!inSequence)
                {
                    inSequence = true;
                    //start shooting sequence
                    HoldState();
                    CanvasController.instance.ShootingState(true);
                    //projection.enabled = true;
                    projection.ActivateLine();
                    

                }
                else
                {
                    Put();
                    //stop shooting sequence
                }
                
            }

            if (inSequence)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Shoot();
                    projection.CleanLine();
                    PlayerController.instance.isShooting = false;
                    //projection.enabled = false;
                    StartCoroutine(ShootDelay());

                }
            }
        }

        if (launched)
        {
            checkTime += Time.deltaTime;
            if (checkTime > 10f)
            {
                if (!inCircle)
                {
                    CanvasController.instance.FailLoseState();
                }
            }
        }
    }
    
    public void HoldState()
    {
        PlayerController.instance.HoldShootingObject(gameObject);
       // rb.useGravity = false;
       
        rb.constraints = RigidbodyConstraints.FreezeAll;
        coll.enabled = false;
        CanvasController.instance.CustomInteractiveText(true, "'Space' to shoot the ball");
        PlayerController.instance.isShooting = true;
        // StartCoroutine(HoldDelay());
    }

    public void Put()
    {
        rb.constraints = RigidbodyConstraints.None;
        inSequence = false;
        PlayerController.instance.PutObject();
        gameObject.transform.parent = null;
        coll.enabled = true;
        projection.CleanLine();
        CanvasController.instance.ShootingState(false);
        CanvasController.instance.CustomInteractiveText(true, "'E' to hold the ball");
        PlayerController.instance.isShooting = false;
    }

    private void Shoot()
    {
        //rb.useGravity = true;
        playerNear = false;
        launched = true;
        rb.constraints = RigidbodyConstraints.None;
        float shootSpeed = CanvasController.instance.GetShootValue();
        if (shootSpeed >= 26 && shootSpeed <= 34)
        {
            shootSpeed = 30;
        }
        Debug.Log("shoot power is: " + shootSpeed);
        rb.velocity = (transform.forward * shootSpeed);
        //rb.AddForce(transform.forward * shootSpeed * 0.5f, ForceMode.Impulse);
        gameObject.transform.parent = null;
        CanvasController.instance.CustomInteractiveText(false, "");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.CustomInteractiveText(true, "'E' to hold the ball");
            playerNear = true;
        }

        if (other.CompareTag("Circle"))
        {
            inCircle = true;
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.CustomInteractiveText(false, "'E' to hold the ball");
            playerNear = false;
        }
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController.instance.PutObject();
    }
}
