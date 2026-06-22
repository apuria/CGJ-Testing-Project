using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;

[CreateAssetMenu(fileName = "New Branch", menuName = "Dialogue/New Branch")]
public class BranchSetting : ScriptableObject, IStateData
{
    public List<Branch> branches;
}
