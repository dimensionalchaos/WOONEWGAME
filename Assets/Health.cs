using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
   
    
    Scenebattlemanager battleManager;
    public float maxHealth;
    public float currentHealth;
    public GameObject healthbar;
    void Awake()
    {
        currentHealth = maxHealth;
        battleManager = GameObject.FindWithTag("battlemanager").GetComponent<Scenebattlemanager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void Reducehealth(float damage)
{
    currentHealth -= damage;
    if (currentHealth <= 0f)
    {
        print("dying was called but death cannot take me");
        List<int> toRemove = new List<int>();

        battleManager.enemyteam.Remove(gameObject);

        // Track indices to remove from battlePriority
        int index = 0;
        foreach (var (iterteamPrior,iterSpeed, iterEntity, iterPriority) in battleManager.battlePriority)
        {
            if (iterEntity == gameObject)
            {
                toRemove.Add(index);
            }
            index++;
        }
            // Remove items from battlePriority in reverse order
            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                battleManager.battlePriority.RemoveAt(toRemove[i]);
                battleManager.UpdateUI(Scenebattlemanager.UI.REMOVE, toRemove[i], 0, battleManager.orderPanel, null);
        }

        // Track indices to remove from secondturn
        toRemove.Clear();
        index = 0;
        foreach (var (iterteamprior,iterSpeed, iterEntity,iterPriority) in battleManager.secondturn)
        {
            if (iterEntity == gameObject)
            {
                toRemove.Add(index);
            }
            index++;
        }
            // Remove items from secondturn in reverse order
            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                battleManager.secondturn.RemoveAt(toRemove[i]);
                battleManager.UpdateUI(Scenebattlemanager.UI.REMOVE, toRemove[i], 0, battleManager.secondturnorderPanel, null);
        }

        Destroy(healthbar);
        Destroy(gameObject);
    }
}

    // Update is called once per frame
    void Update()
    {
        healthbar.GetComponent<Slider>().value = currentHealth/maxHealth;
    }
}
