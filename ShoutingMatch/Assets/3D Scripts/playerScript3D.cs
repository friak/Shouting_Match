using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript3D : MonoBehaviour
{
    float actionCooldown = 1.0f;
    float timeSinceAction = 0.0f;
    bool groundTouch = true;
    
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
            m_Rigidbody.AddForce(Vector3.up * 500);
            Debug.Log("in the air");

        }
    }
    void FixedUpdate()
    {
        if (Input.GetKey("a") && Input.GetKey("w"))
        {
            Debug.Log("block");
            
        }

        else if (Input.GetKey("a"))
        {
            {
                //m_Rigidbody.AddForce(Vector3.left * b10);
            }
        }
        else if (Input.GetKey("w") && groundTouch==true)
        {
        
                PlayerJumpActionButton();
            


        }
        if (Input.GetKey("d"))
        {
            m_Rigidbody.AddForce(Vector3.right * 10);
            
        }
        if (Input.GetKey("s"))
        {
            m_Rigidbody.AddForce(Vector3.down * 40);
            Debug.Log("crouch");
        }
    }
}
