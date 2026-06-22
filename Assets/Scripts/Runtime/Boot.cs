using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Boot : MonoBehaviour
{

//TODO:
/*
1. 
2. 
3. 
*/
    void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);
    }

    async UniTaskVoid Start()
    {
        await UniTask.Yield();
    }
}
