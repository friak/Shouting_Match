using UnityEngine.InputSystem;
using UnityEngine;
using System.IO.Ports;

// Sets up controllers with new input system and arduino controllers
public class PlayerControlInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;

    private SerialPort sPort;

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
        sPort = playerController.IsPlayer1 ? GameStateManager.Instance.SPort1 : GameStateManager.Instance.SPort2;
    }

    private void Update()
    {
        // Arduino Communication
        if (sPort.IsOpen)
        {
            try
            {
                int data = CheckIfDataReceived();
                if (data >= 100)
                {
                    playerController.SetAttackFromArduino(true, GetAttacklevel(data));
                }else if (data > 10 && data <=100)
                {
                    playerController.SetAttackFromArduino(false, 0);
                }

                if (data == 3)
                {
                    playerController.SetJumpFromArduino(true);
                }
                else if (data == 2)
                {
                    playerController.SetJumpFromArduino(false);
                    playerController.SetCrouchFromArduino(false);
                }
                else if (data == 4)
                {
                    playerController.SetCrouchFromArduino(true);
                }

                if(data == -1 || data == 0 || data == 1)
                {
                    playerController.SetMoveFromArduino(data);
                }
            }
            catch (System.Exception e)
            {
               //  Debug.Log(playerController.IsPlayer1 + ": An error occured while reading from arduino: " + e);
            }
        }
    }

    public int CheckIfDataReceived()
    {
       // sPort.BaseStream.Flush();
       // sPort.DiscardInBuffer();
        sPort.Write("V");       // ask for data
        string input = sPort.ReadLine();
        string pl = playerController.IsPlayer1 ? "P1: " : "P2: ";
        Debug.Log( pl + "" + input);
        return int.Parse(input);
    }

    private int GetAttacklevel(int data)
    {
        switch (playerController.Combat)
        {
            case CombatType.DURATION:
                {
                    if (data > 1000 && data <= 10000)
                    {
                        return 1;
                    }
                    else if (data > 10000 && data <= 100000)
                    {
                        return 2;
                    }
                    return 3;
                }
            case CombatType.VOLUME:
                {
                    if (data > 100 && data <= 150)
                    {
                        return 2;
                    }
                    else if (data > 150)
                    {
                        return 3;
                    }
                    return 1;
                }
            default:
                return 1;
        }
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
