using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[CreateAssetMenu(fileName = "New_Animation_Con_List", menuName = "List/Animation_Controller")]
public class AnimationControllerList : ScriptableObject
{
    public List<RuntimeAnimatorController> controllers;


}
