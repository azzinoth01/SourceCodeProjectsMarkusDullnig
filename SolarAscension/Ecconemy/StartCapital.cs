using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "StartCapitalData", menuName = "ScriptableObjects/StartCapitalData", order = 3)]
public class StartCapital : ScriptableObject {

    [SerializeField] private List<RessourcesValue> _startValues;

    public List<RessourcesValue> StartValues {
        get {
            return _startValues;
        }

        set {
            _startValues = value;
        }
    }
}
