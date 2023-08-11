using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
   public static AIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void MakeMove()
    {
        
    }
}
