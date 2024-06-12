using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Quest {
    public string name;
    public ushort requiredPoints;
    public bool show;
}

public class QuestSystem : MonoBehaviour
{
    public Quest[] quests;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
