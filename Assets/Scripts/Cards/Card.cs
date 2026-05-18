using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Card : ScriptableObject
{
    public string description="default";
    public abstract void Apply();
}
