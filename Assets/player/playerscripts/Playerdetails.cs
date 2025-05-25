using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class Playerdetails : Combatdetails
{
       public override int tag
    {
        get { return 0; }
    }

    void Awake()
    {
       dotTracker = new Dottracker();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public override void Attack(Attacktype attackType,GameObject target)
    {
        Details targetDetails = target.GetComponent<Details>();
        if(attackType.attackDebuffs.Contains("elementalaffliction"))
        {   if(targetDetails.eledebuffs.ContainsKey(element))
            {
                 Scenebattlemanager.multiplier =2f;
                 targetDetails.eledebuffs[element] = (ele,baseDamage);
            }
            else
            
            targetDetails.eledebuffs.Add(element,(ele,baseDamage));
            targetDetails.dotTracker.Updatedots(targetDetails.eledebuffs);
          
        
       
      
    }
    }
    private void Callglobalattack()
    {
        Scenebattlemanager sceneBattlemanager = GameObject.FindWithTag("battlemanager").GetComponent<Scenebattlemanager>();
        sceneBattlemanager.Callattack();
        
    }
}

[System.Serializable]
public class Attacktype
{
    public string name;
    public string type;
  
    public string[] attackDebuffs;
    public string animTrigger;
}
