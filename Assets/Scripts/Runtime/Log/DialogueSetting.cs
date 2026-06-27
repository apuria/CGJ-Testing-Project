using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue", order = 0)]
public class DialogueSetting : ScriptableObject, IStateData
{
    public bool hasBackground = false;
    public Image BackGround;

    public List<Speaker> speakers;

    public List<Dialogue> dialogues;

}
