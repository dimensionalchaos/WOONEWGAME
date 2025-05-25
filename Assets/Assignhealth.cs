using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignhealth : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    GameObject healthbar;
    Scenebattlemanager battleManager;
    void Awake()
    {
        battleManager = GameObject.FindWithTag("battlemanager").GetComponent<Scenebattlemanager>();
    }
    void Start()
    {

        foreach (var enemy in battleManager.enemyteam)
        {
            GameObject clonebar = Instantiate(healthbar,transform.position,Quaternion.identity);
            enemy.GetComponent<Health>().healthbar  = clonebar;
            clonebar.transform.parent = transform;
            clonebar.transform.position = transform.InverseTransformPoint(enemy.transform.position+offset);
            clonebar.SetActive(true);
        }

        foreach  (var player in battleManager.playerteam)
        {
              GameObject clonebar = Instantiate(healthbar,transform.position,Quaternion.identity);
            player.GetComponent<Health>().healthbar  = clonebar;
            clonebar.transform.parent = transform;
            clonebar.transform.position = transform.InverseTransformPoint(player.transform.position+offset);
            clonebar.SetActive(true);   

        }
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
       
    }
}
