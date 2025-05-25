using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assigntype : MonoBehaviour
{
    [SerializeField]
    Scenebattlemanager battleManager;
    [SerializeField]
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setit()
    {
        Playerdetails playerDetails = battleManager.currentPlayer.GetComponent<Playerdetails>();
        battleManager.attackType = playerDetails.attackTypes[index];
        battleManager.atktriggerName = playerDetails.attackTypes[index].animTrigger;
        
    }
}

