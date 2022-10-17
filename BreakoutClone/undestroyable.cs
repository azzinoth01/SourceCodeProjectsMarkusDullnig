using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class undestroyable : MonoBehaviour
{
 

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

}
