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
                    CanvasController.instance.ActivateShooting();
                    //projection.enabled = true;
                    projection.ActivateLine();
                    
                }
                else
                {
                    inSequence = false;
                    PlayerController.instance.PutObject();
                    gameObject.transform.parent = null;
                    coll.enabled = true;
                    CanvasController.instance.CustomInteractiveText(true, "'E' to hold the ball");
                    //stop shooting sequence
                }
                
            }

            if (inSequence)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Shoot();
                    projection.CleanLine();
                    //projection.enabled = false;
                    StartCoroutine(ShootDelay());

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
        // StartCoroutine(HoldDelay());
    }

    private void Shoot()
    {
        //rb.useGravity = true;
        playerNear = false;
        rb.constraints = RigidbodyConstraints.None;
        float shootSpeed = CanvasController.instance.GetShootValue();
        rb.velocity = (transform.forward * 30);
        //rb.AddForce(transform.forward * shootSpeed * 0.5f, ForceMode.Impulse);
        gameObject.transform.parent = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.CustomInteractiveText(true, "'E' to hold the ball");
            playerNear = true;
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
