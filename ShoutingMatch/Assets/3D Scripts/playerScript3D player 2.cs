using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class playerScript3D : MonoBehaviour
{
    float actionCooldownTwo = 1.0f;
    float timeSinceActionTwo = 0.0f;
    bool groundTouchTwo = true;
    public GameObject DirectionPointer;
    
    Rigidbody m_RigidbodyTwo;
    public float m_Thrust = 50f;
    // Start is called before the first frame update
    void Start()
    {
        m_RigidbodyTwo = GetComponent<Rigidbody>();
        m_RigidbodyTwo.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceActionTwo += Time.deltaTime;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GroundTouch")
        {
            groundTouchTwo = true;
            Debug.Log("touching Ground");
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "GroundTouch")
        {
            groundTouchTwo = false;
            Debug.Log("not touching Ground");
        }
    }
    
    void PlayerTwoJumpActionButton()
    {
        if (timeSinceActionTwo > actionCooldownTwo)
        {
            timeSinceActionTwo = 0;
            m_RigidbodyTwo.AddForce(Vector3.up * (this.GetComponent<player>().speed *20));
            Debug.Log("in the air");

        }
    }
    void FixedUpdate()
    {
        void attackList(string directional, string attackLevel)
        {
            //do stuff
        }
        if (Input.GetKey("p"))
        {
            {
                Debug.Log("light attack");
                attackList("", "light");
            }
        }
        if (Input.GetKey("["))
        {
            {
                Debug.Log("medium attack");
                attackList("", "medium");
            }
        }
        if (Input.GetKey("]"))
        {
            {
                Debug.Log("attack attack");
                attackList("", "heavy");
            }
        }

        if (Input.GetKey("right") && Input.GetKey("up"))
        {
            Debug.Log("block");
            
        }
        if (Input.GetKey("left") && Input.GetKey("up"))
        {
            Debug.Log("air move");

        }
        if (Input.GetKey("down") && Input.GetKey("left"))
        {
            Debug.Log("croch move");

        }

        else if (Input.GetKey("left"))
        {
            {
                m_RigidbodyTwo.AddForce(Vector3.left * this.GetComponent<player>().speed);
            }
        }
        else if (Input.GetKey("up") && groundTouchTwo==true)
        {
        
                PlayerTwoJumpActionButton();
            


        }
        if (Input.GetKey("right"))
        {
            m_RigidbodyTwo.AddForce(Vector3.right * this.GetComponent<player>().speed);
            
        }
        if (Input.GetKey("down"))
        {
            m_RigidbodyTwo.AddForce(Vector3.down * this.GetComponent<player>().speed);
            Debug.Log("crouch");
        }
    }
}
