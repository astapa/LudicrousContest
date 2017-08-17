using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    public int reactionTime = 2;
    public int hp = 5;
    public int xvelocity;
    public int yvelocity;
    public LinkedList<int> statusEffects;
    public bool isCrouched;
}
