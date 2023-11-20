using UnityEngine;
using System.IO.Ports;
using System.Collections.Generic;

public class Player
{
    public CharacterSO Character { get; private set; }
    public SerialPort SPort { get; private set; }
    public Dictionary<string, int> Controls { get; private set; }


    public Player(string port, CharacterSO cso, Dictionary<string, int> controls)
    {
        Character = cso;
        SPort = new SerialPort(port, 9600);
        Controls = controls;
        
        // Arduino Communication
        try
        {
            SPort.Open();
            SPort.ReadTimeout = 25;
            Debug.Log("Port open: " + SPort.PortName); 
        }
        catch (System.Exception)
        {
            Debug.Log("Port Not Found!");
        }
    }

    public void ChangeCharacter(CharacterSO character)
    {
        Character = character;
    }
}

