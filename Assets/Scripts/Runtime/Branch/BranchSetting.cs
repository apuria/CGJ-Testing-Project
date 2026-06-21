using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;

[CreateAssetMenu(fileName = "New Branch Setting", menuName = "Dialogue/Branch Setting")]
public class BranchSetting : ScriptableObject, IStateData
{
    public List<Branch> branches;
}
