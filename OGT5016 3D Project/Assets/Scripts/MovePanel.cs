using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanel : MonoBehaviour
{
    //Select the movement type
    public enum MoveType
    {
        Position,
        Rotation
    }

    public MoveType move;
    
    //movement sound
    public AudioSource soundSource;
    public AudioClip[] audioList;

    private Vector3 newPos; //takes the wanted position
    private Vector3 startPos; //takes the starting position
    
    [SerializeField] private float speed = 1f; //movemnt speed
    [SerializeField] private float x = 0f;
    [SerializeField] private float y = 0f;
    [SerializeField] private float z = 0f;

    [SerializeField] private bool haveSound; //if movement have sound
    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;
    
    
    [SerializeField] private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        //if it is a position movement, starting position is important
        if (move == MoveType.Position)
        {
            newPos = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
            startPos = transform.position;

            journeyLength = Vector3.Distance(startPos, newPos);
        }

        soundSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (isMoving)
        {

            if (move == MoveType.Position) //makes movement slowly
            {

                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - startTime) * speed;

                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourney = distCovered / journeyLength;

                // Set our position as a fraction of the distance between the markers.
                transform.position = Vector3.Lerp(startPos, newPos, fractionOfJourney);

                if (haveSound)
                {
                    soundSource.PlayOneShot(audioList[0]);
                    haveSound = false;
                }
                
            }
            
            else if(move == MoveType.Rotation) //makes movement slowly

            {
                if (haveSound)
                {
                    soundSource.PlayOneShot(audioList[1]); //just for the button sound in level, not a general code
                    soundSource.PlayOneShot(audioList[0]); //movement sound
                    haveSound = false;
                }
                
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(x, y, z), Time.deltaTime * speed);
                
               
            }

            
        }

    }

    //movement is activated or not
    public void ActivateObject()
    {
        isMoving = true;
        startTime = Time.time;
    }

   
}
