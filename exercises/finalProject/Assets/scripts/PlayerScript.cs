using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update

    //key codes
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode jump;
    public KeyCode slide;

    //mechanics
    public float horizontalVelocity = 0f;
    private int lanePos = 0;
    private bool lockedMove = false;
    private bool lockAnim = false;
    private bool amIdead = false;

    //animator
    public Animator Anmtr;

    //camera
    float cameraFollowDist = 5;
    float cameraFollowUp = 2;
    
    float currentVeloctiy = 6;
    float initialVelocity = 6;

    
    //manipulating the colliders for then animations
    public CapsuleCollider capColl;
    private CapsuleCollider standard;
    Vector3 ccCenter;
    float ccHeight;

    void Start()
    {
        Anmtr = GetComponent<Animator>();
        capColl = GetComponent<CapsuleCollider>();
        ccCenter = GetComponent<CapsuleCollider>().center;
        ccHeight = GetComponent<CapsuleCollider>().height;

        standard = capColl;

    }

    // Update is called once per frame
    void Update()
    {
        if (amIdead)
            return;

        if (!lockAnim)
        {
            Anmtr.SetBool("isSliding", false);
            Anmtr.SetBool("isJumping", false);
        }
        
        GetComponent<Rigidbody>().velocity = new Vector3(horizontalVelocity, 0, currentVeloctiy);
        updateColliderForAnimation();
        if(Input.GetKeyDown(moveLeft) && (lanePos>-1) && !lockedMove)
        {
            horizontalVelocity = -2f;
            StartCoroutine(stopHorizontalSlide());
            lanePos -= 1;
            lockedMove = true;

        }
        if (Input.GetKeyDown(moveRight) && (lanePos < 1) && !lockedMove)
        {
            horizontalVelocity = 2f;
            StartCoroutine(stopHorizontalSlide());
            lanePos += 1;
            lockedMove = true;
        }
        if (Input.GetKeyDown(slide))
        {
            Anmtr.SetBool("isSliding", true);
            StartCoroutine(stopForAnim());
            lockAnim = true;
        }
        if (Input.GetKeyDown(jump))
        {
            Anmtr.SetBool("isJumping", true);
            StartCoroutine(stopForAnim());
            lockAnim = true; 
        }

        //camera stuff
        Vector3 cameraPos = transform.position;
        cameraPos.y = transform.position.y + 1;
        Camera.main.transform.position = cameraPos;

        //Vector3 cameraPosition = transform.position + (Vector3.up * cameraFollowUp) + (-transform.forward * cameraFollowDist);
        //cameraPosition.y = transform.position.y + cameraFollowUp;
        //cameraPosition.x = 0;
        //Camera.main.transform.position = cameraPosition;
        //Camera.main.transform.LookAt(transform.position + transform.forward * camLookAhead);

    }

    IEnumerator stopHorizontalSlide()
    {
        yield return new WaitForSeconds(.5f);
        horizontalVelocity = 0f;
        lockedMove = false;
    }
    
    IEnumerator stopForAnim()
    {
        // + Anmtr.GetCurrentAnimatorStateInfo(0).normalizedTime
        yield return new WaitForSeconds(Anmtr.GetCurrentAnimatorStateInfo(0).length+.1f);
        lockAnim = false;
    }

    public void setSpeed(float modifier)
    {
        currentVeloctiy = initialVelocity + modifier;
    }

    public void updateColliderForAnimation()
    {
        bool currSlide = Anmtr.GetBool("isSliding");
        //bool currSlide = Anmtr.GetCurrentAnimatorStateInfo(0).IsName("isSliding");
        Debug.Log(currSlide);
        bool currjump = Anmtr.GetBool("isJumping");
        if (currSlide)
        {
            Debug.Log("in the slide if statement");
            float slideHeight = .5f;
            Vector3 slideVect = new Vector3(0, .5f, 0);
            capColl.center = slideVect;
            capColl.height = slideHeight;

        }
        else if (currjump)
        {
            float jumpHeight = .9f;
            Vector3 jumpColliderVector = new Vector3(0, 2, 0);
            capColl.center = jumpColliderVector;
            capColl.height = jumpHeight;

            //GetComponent<CapsuleCollider>().center= jumpColliderVector;
            //GetComponent<CapsuleCollider>().height = jumpHeight;
        }
        else
        {
            capColl.center = ccCenter;
            capColl.height = ccHeight;

        }
    }
    public float getSpeed()
    {
        return currentVeloctiy;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string objectCollided = collision.gameObject.tag;
        Debug.Log(objectCollided);
        if(objectCollided == "Obstacle")
        {
            //trigger end of game
            if(collision.gameObject.transform.position.z > transform.position.z + .175f)        //only want to endgame if hit object infront of you not the side, used .175f bc half of radius of capsule colider
                endGame();
        }
        
    }
    private void endGame()
    {
        Debug.Log("Dead");
        amIdead = true;
        GetComponent<scorescript>().amDead();
    }
}
