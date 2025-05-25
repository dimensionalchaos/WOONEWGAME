using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dottracker 
{
    
   public Dictionary<string,float>dots;
    public Dottracker()
    {
      this.dots = new Dictionary<string,float>();
      
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Updatedots(Dictionary<string,(float,float)> afflictions)
    {
        if(afflictions.ContainsKey("fire")&&afflictions.ContainsKey("wind"))
        {
            var(firstEle,firstbaseDamage)= afflictions["fire"];
            var(secondEle,secondbaseDamage)=afflictions["wind"];
            //Item1 represents the ele stat associated with that element and item2 the baseDamage stat associated with that element
            float dotDamage = ((10f/100f*(firstEle+secondEle))*(50f/100f*(firstbaseDamage+secondbaseDamage)))+((50f/100f)*(firstbaseDamage+secondbaseDamage));
            if(dots.ContainsKey("blast"))
            {
                dots["blast"]= dotDamage;
            }
            else{
                dots.Add("blast",dotDamage);
            }
    }
    if(afflictions.ContainsKey("fire")&&afflictions.ContainsKey("water"))
    {
         var(firstEle,firstbaseDamage) = afflictions["fire"];
            var(secondEle,secondbaseDamage)=afflictions["water"];
            //Item1 represents the ele stat associated with that element and item2 the baseDamage stat associated with that element
            float dotDamage = ((10f/100f*(firstEle+secondEle))*(50f/100f*(firstbaseDamage+secondbaseDamage)))+((50f/100f)*(firstbaseDamage+secondbaseDamage));
            if(dots.ContainsKey("steam"))
            {
                dots["steam"]= dotDamage;

            }
            else{
                dots.Add("steam",dotDamage);
            }
    }
  
    
}
}
