using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "RessourceDescriptionData", menuName = "ScriptableObjects/RessourceDescriptionData", order = 1)]
public class RessourceDescription : ScriptableObject {


    [SerializeField] private List<RessourceInfo> _ressources;

    public List<RessourceInfo> Ressources {
        get {
            return _ressources;
        }

        set {
            _ressources = value;
        }
    }
}
