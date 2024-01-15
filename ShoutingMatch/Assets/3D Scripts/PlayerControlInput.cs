using UnityEngine.InputSystem;
using UnityEngine;
using System.IO.Ports;

// Sets up controllers with new input system and arduino controllers
public class PlayerControlInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;

    private SerialPort sPort;
    [SerializeField] private string port;
    [SerializeField] private int baudRate = 9600;

    private void Awake()
    {
        // UNITY INPUT SYSTEM
        playerInput = new PlayerInput();
        playerController = GetComponent<PlayerController>();
        // needs to be removed when finished testing ?
        SubscribeActions();
    }

    private void Start()
    {

        // ARDUINO INPUT
        sPort = new SerialPort(port, baudRate);
        try
        {
            sPort.Open();
            sPort.ReadTimeout = 25;
            Debug.Log("Port open: " + sPort.PortName);
        }
        catch (System.Exception e)
        {
            Debug.Log("Port Not Found! " + e);
        }
    }

    private void Update()
    {
        // Arduino Communication
        if (sPort.IsOpen)
        {
            try
            {
                ArduinoControl(sPort.ReadByte());
            }
            catch (System.Exception e)
            {
               //  Debug.Log(playerController.IsPlayer1 + ": An error occured while reading from arduino: " + e);
            }
        }
    }

    private void ArduinoControl(int data)
    {
        if(data > 100)
        {
            playerController.SetAttackFromArduino(true, GetAttacklevel(data));
            Debug.Log("====================================\nAttack: " + data);
        }else if (data == 0)
        {
            playerController.SetAttackFromArduino(false, 0);
        }


        if (data == 1)
        {
            playerController.SetCrouchFromArduino(true);
            Debug.Log("CROUCH");
        }
        else if (data == 2)
        {
            playerController.SetJumpFromArduino(false);
            playerController.SetCrouchFromArduino(false);
            Debug.Log("NO JUMP OR CROUCH");
        }
        else if (data == 3)
        {
            playerController.SetJumpFromArduino(true);
            Debug.Log("JUMP");
        }

        if (data == 4)
        {
            playerController.SetMoveFromArduino(1);
            Debug.Log("RIGHT");
        }
        else if( data == 5)
        {
            playerController.SetMoveFromArduino(0); ;
            Debug.Log("NO LEFT OR RIGHT");
        }
        else if (data == 6)
        {
            playerController.SetMoveFromArduino(-1);
            Debug.Log("LEFT");
            return;
        }
    }

    private int GetAttacklevel(int data)
    {
        if(data > 100 && data <= 150)
        {
            return 2;
        }
        else if (data > 150)
        {
            return 3;
        }
        return 1;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerController.SetMoveFromArduino(context.ReadValue<float>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        playerController.SetJumpFromArduino(context.ReadValueAsButton());
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        playerController.SetCrouchFromArduino(context.ReadValueAsButton());
    }

    public void OnAttack1(InputAction.CallbackContext context)
    {
        playerController.SetAttackFromArduino(context.ReadValueAsButton(), 1);
    }
    public void OnAttack2(InputAction.CallbackContext context)
    {
        playerController.SetAttackFromArduino(context.ReadValueAsButton(), 2);
    }
    public void OnAttack3(InputAction.CallbackContext context)
    {
        playerController.SetAttackFromArduino(context.ReadValueAsButton(), 3);
    }
    public void OnAttackEnd(InputAction.CallbackContext context)
    {
        playerController.ResetAttackTriggers();
    }

    private void SubscribeActions()
    {
        if (playerController.IsPlayer1)
        {
            playerInput.P1_Controls.Move.started += OnMove;
            playerInput.P1_Controls.Move.performed += OnMove;
            playerInput.P1_Controls.Move.canceled += OnMove;
            playerInput.P1_Controls.Jump.performed += OnJump;
            playerInput.P1_Controls.Jump.canceled += OnJump;
            playerInput.P1_Controls.Crouch.started += OnCrouch;
            playerInput.P1_Controls.Crouch.canceled += OnCrouch;
            playerInput.P1_Controls.Attack1.started += OnAttack1;
            playerInput.P1_Controls.Attack1.canceled += OnAttackEnd;
            playerInput.P1_Controls.Attack2.started += OnAttack2;
            playerInput.P1_Controls.Attack2.canceled += OnAttackEnd;
            playerInput.P1_Controls.Attack3.started += OnAttack3;
            playerInput.P1_Controls.Attack3.canceled += OnAttackEnd;
            Debug.Log("METHODS SUBSCRIBED FOR PL 1");
        }
        else
        {
            playerInput.P2_Controls.Move.started += OnMove;
            playerInput.P2_Controls.Move.performed += OnMove;
            playerInput.P2_Controls.Move.canceled += OnMove;
            playerInput.P2_Controls.Jump.performed += OnJump;
            playerInput.P2_Controls.Jump.canceled += OnJump;
            playerInput.P2_Controls.Crouch.started += OnCrouch;
            playerInput.P2_Controls.Crouch.canceled += OnCrouch;
            playerInput.P2_Controls.Attack1.started += OnAttack1;
            playerInput.P2_Controls.Attack1.canceled += OnAttackEnd;
            playerInput.P2_Controls.Attack2.started += OnAttack2;
            playerInput.P2_Controls.Attack2.canceled += OnAttackEnd;
            playerInput.P2_Controls.Attack3.started += OnAttack3;
            playerInput.P2_Controls.Attack3.canceled += OnAttackEnd;
            Debug.Log("METHODS SUBSCRIBED FOR PL 2");
        }
    }

    private void OnEnable()
    {
        if (playerController.IsPlayer1)
        {
            playerInput.P1_Controls.Enable();
        }
        else
        {
            playerInput.P2_Controls.Enable();
        }

    }

    private void OnDisable()
    {
        if (playerController.IsPlayer1)
        {
            playerInput.P1_Controls.Disable();
        }
        else
        {
            playerInput.P2_Controls.Disable();
        }
    }
}
