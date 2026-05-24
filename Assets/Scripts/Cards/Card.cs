using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Card : ScriptableObject
{
    public string description;
    public int usesRemaining=1;
    public int id;
    public abstract void Apply();
}
