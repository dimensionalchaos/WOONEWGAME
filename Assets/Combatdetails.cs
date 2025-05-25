using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatdetails : MonoBehaviour
{
    public int teampriority = 0;
    public virtual int tag
    {
        get { return -1; }
        // You can add set if you want to allow assignment in base class
    }
    [Header("Enter 3 for ranger , 2 for warrior, 1 for wizard or 0 for an enemy unit")]
    public float sameSpeedpriority;
    public Dottracker dotTracker;
    public float ele;
    public Scenebattlemanager sceneManager;
    public string element;
    public float resistance;
    public List<string> debuffs;
    [SerializeField]
    public Dictionary<string, (float, float)> eledebuffs;
    [SerializeField]
    public Attacktype[] attackTypes;
    [SerializeField]
    public float speed;
    [SerializeField]

    public float baseDamage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual void Attack(Attacktype attackType,GameObject target)
    {

    }
}
