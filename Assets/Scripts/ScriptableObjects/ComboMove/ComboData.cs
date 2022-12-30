using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "Scriptable/Combo Move")]
[Serializable]
public class ComboData : ScriptableObject
{
    public List<int> comboMove = new List<int>(3);
}
