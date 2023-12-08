using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu]
public class ThirdDimcharacter : ScriptableObject
{
    public new string name;
    public int speed;
    public double health;

    public AnimatorController controller;
    public Rig rig;
    public Animation idle;
    public Animation taunt;

}
