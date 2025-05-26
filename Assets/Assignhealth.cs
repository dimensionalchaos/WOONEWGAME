using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Assignhealth : MonoBehaviour
{
    [SerializeField]
    Vector3 healthbaroffset;
    [SerializeField]
    GameObject healthbar;
    [SerializeField]
    GameObject afflictionsbar;
    Scenebattlemanager battleManager;
    [SerializeField]
    Vector3 afflictionsbaroffset;
    void Awake()
    {
        battleManager = GameObject.FindWithTag("battlemanager").GetComponent<Scenebattlemanager>();
    }
    void Start()
    {

        foreach (var enemy in battleManager.enemyteam)
        {
            GameObject afflictionsclonebar = Instantiate(afflictionsbar, transform.position, Quaternion.identity);
            enemy.GetComponent<Details>().afflictionsbar = afflictionsclonebar;
            afflictionsclonebar.transform.parent = transform;
            Vector3 actualoffset = new Vector3(-1, afflictionsbaroffset.y, afflictionsbaroffset.z);
            afflictionsclonebar.transform.localPosition = transform.InverseTransformPoint(enemy.transform.position + actualoffset);
            afflictionsclonebar.GetComponent<RectTransform>().sizeDelta = afflictionsbar.GetComponent<RectTransform>().sizeDelta;
            afflictionsclonebar.GetComponent<HorizontalLayoutGroup>().reverseArrangement = true;
            afflictionsclonebar.SetActive(true);

            GameObject clonebar = Instantiate(healthbar,transform.position,Quaternion.identity);
            enemy.GetComponent<Health>().healthbar  = clonebar;
            clonebar.transform.parent = transform;
            clonebar.transform.localPosition = transform.InverseTransformPoint(enemy.transform.position+healthbaroffset);
            clonebar.SetActive(true);
        }

        foreach  (var player in battleManager.playerteam)
        {
            GameObject afflictionsclonebar = Instantiate(afflictionsbar, transform.position, Quaternion.identity);
            player.GetComponent<Playerdetails>().afflictionsbar = afflictionsclonebar;
            afflictionsclonebar.transform.parent = transform;
            afflictionsclonebar.transform.localPosition = transform.InverseTransformPoint(player.transform.position + afflictionsbaroffset);
            afflictionsclonebar.GetComponent<RectTransform>().sizeDelta = afflictionsbar.GetComponent<RectTransform>().sizeDelta;
            afflictionsclonebar.SetActive(true);

            GameObject clonebar = Instantiate(healthbar,transform.position,Quaternion.identity);
            player.GetComponent<Health>().healthbar  = clonebar;
            clonebar.transform.parent = transform;
            clonebar.transform.localPosition = transform.InverseTransformPoint(player.transform.position+healthbaroffset);
            clonebar.SetActive(true);   

        }
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
       
    }
}
