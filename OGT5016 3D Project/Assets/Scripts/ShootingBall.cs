using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBall : MonoBehaviour
{
    private bool playerNear = false;
    private bool inSequence = false;

    [SerializeField] private Collider coll;

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
                }
                else
                {
                    inSequence = false;
                    //stop shooting sequence
                }
                
            }

            if (inSequence)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Shoot();
                }
            }
        }
    }
    
    public void HoldState()
    {
        PlayerController.instance.HoldObject(gameObject);
        rb.useGravity = false;
        //coll.enabled = false;
        //CanvasController.instance.InteractableText(false);
        // StartCoroutine(HoldDelay());
    }

    private void Shoot()
    {
        rb.useGravity = true;
        gameObject.transform.parent = null;
        Rigidbody instBulletRB = gameObject.GetComponent<Rigidbody>();
        float shootSpeed = CanvasController.instance.GetShootValue();
        instBulletRB.AddForce(transform.forward * shootSpeed * 10f, ForceMode.Force);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.InteractableText(true);
            playerNear = true;
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.InteractableText(false);
            playerNear = false;
        }
    }
}
