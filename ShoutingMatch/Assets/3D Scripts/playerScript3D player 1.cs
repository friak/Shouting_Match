using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class playerScriptTwo3D : MonoBehaviour
{
    float actionCooldown = 1.0f;
    float timeSinceAction = 0.0f;
    bool groundTouch = true;
    public GameObject DirectionPointer;
    
    Rigidbody m_Rigidbody;
    public float m_Thrust = 50f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAction += Time.deltaTime;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GroundTouch")
        {
            groundTouch = true;
            Debug.Log("touching Ground");
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "GroundTouch")
        {
            groundTouch = false;
            Debug.Log("not touching Ground");
        }
    }
    
    void PlayerJumpActionButton()
    {
        if (timeSinceAction > actionCooldown)
        {
            timeSinceAction = 0;
            m_Rigidbody.AddForce(Vector3.up * (this.GetComponent<player>().speed *20));
            Debug.Log("in the air");

        }
    }
    void FixedUpdate()
    {
        void attackList(string directional, string attackLevel)
        {
            //do stuff
        }
        if (Input.GetKey("j"))
        {
            {
                Debug.Log("light attack");
                attackList("", "light");
            }
        }
        if (Input.GetKey("k"))
        {
            {
                Debug.Log("medium attack");
                attackList("", "medium");
            }
        }
        if (Input.GetKey("l"))
        {
            {
                Debug.Log("attack attack");
                attackList("", "heavy");
            }
        }

        if (Input.GetKey("a") && Input.GetKey("w"))
        {
            Debug.Log("block");
            
        }
        if (Input.GetKey("d") && Input.GetKey("w"))
        {
            Debug.Log("air move");

        }
        if (Input.GetKey("s") && Input.GetKey("d"))
        {
            Debug.Log("croch move");

        }

        else if (Input.GetKey("a"))
        {
            {
                m_Rigidbody.AddForce(Vector3.left * this.GetComponent<player>().speed);
            }
        }
        else if (Input.GetKey("w") && groundTouch==true)
        {
        
                PlayerJumpActionButton();
            


        }
        else if (Input.GetKey("d"))
        {
            m_Rigidbody.AddForce(Vector3.right * this.GetComponent<player>().speed);
            
        }
        else if (Input.GetKey("s"))
        {
            m_Rigidbody.AddForce(Vector3.down * this.GetComponent<player>().speed);
            Debug.Log("crouch");
        }
    }
}
