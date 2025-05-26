using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
public class Playerdetails : Combatdetails
{
    
    
       public override int tag
    {
        get { return 0; }
    }

    void Awake()
    {
        eledebuffs = new Dictionary<string,(float,float)>();
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
        Combatdetails targetDetails = target.GetComponent<Combatdetails>();
        if (attackType.attackDebuffs.Contains("elementalaffliction"))
        {
            if (targetDetails.eledebuffs.ContainsKey(element))
            {
                Scenebattlemanager.multiplier = 2f;
                targetDetails.eledebuffs[element] = (ele, baseDamage);

            }
            else
            {
                targetDetails.eledebuffs.Add(element, (ele, baseDamage));
                targetDetails.dotTracker.Updatedots(targetDetails.eledebuffs);
                GameObject bar = targetDetails.afflictionsbar;
                bar.transform.GetChild(targetDetails.barindex).gameObject.GetComponent<Image>().sprite = Scenebattlemanager.elespritedict[element];
                bar.transform.GetChild(targetDetails.barindex).gameObject.SetActive(true);
                targetDetails.barindex += 1;
            }
            
          
        
       
      
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
