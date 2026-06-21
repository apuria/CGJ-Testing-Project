using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Branch
{
    [Tooltip("要执行的分支选择")]
    public string id;
    [TextArea(1, 1)]
    public string text;
}
