using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class Details : Combatdetails
{
    int tag = 1;
     void Awake()
     {
        eledebuffs = new Dictionary<string,(float,float)>();
        
        dotTracker = new Dottracker();
        sceneManager = GameObject.FindWithTag("battlemanager").GetComponent<Scenebattlemanager>();
     }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

      
    }
    public void Callglobalenemyattack()
    {
        sceneManager.Callenemyattack();
    }
    
}
