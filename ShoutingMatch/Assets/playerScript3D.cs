using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript3D : MonoBehaviour
{
    float actionCooldown = 1.0f;
    float timeSinceAction = 0.0f;

    
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
    void PlayerJumpActionButton()
    {
        if (timeSinceAction > actionCooldown)
        {
            timeSinceAction = 0;
            m_Rigidbody.AddForce(Vector3.up * 1000);

        }
    }
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
        
                PlayerJumpActionButton();
            


        }
        if (Input.GetKey("a"))
        {
            if (Input.GetKey("w"))
            {
                m_Rigidbody.AddForce(Vector3.down * 1000);
                m_Rigidbody.AddForce(Vector3.left * 100);
            }
        }
        if (Input.GetKey("d"))
        {
            m_Rigidbody.AddForce(Vector3.right * 20);
        }
        if (Input.GetKey("s"))
        {
            m_Rigidbody.AddForce(Vector3.down * 40);
        }
    }
}
