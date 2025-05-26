using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class Scenebattlemanager : MonoBehaviour
{
public static Dictionary<string, Sprite> elespritedict = new Dictionary<string, Sprite>();

// Example of adding







    [SerializeField]
    public GameObject secondturnorderPanel;
    public string atktriggerName;
    bool isrunningcoroutine;
    int attackIndex;
    [SerializeField]
    public GameObject currentPlayer;
    [SerializeField]
    GameObject buttonCursor;
    [SerializeField]
    GameObject cursor;
    [SerializeField]
    GameObject firstbutton;
    GameObject currentSelectedObject;
    [SerializeField]
    GameObject[]playerUi;
    public static  float multiplier;
    public List<(int,float, GameObject,float)> battlePriority = new List<(int,float, GameObject,float)>();
    public List<(int,float,GameObject,float)> secondturn = new List<(int,float,GameObject,float)>();
   

    int panelIndex;
    int playerIndex;
    int enemyIndex;
    [SerializeField]
    public List<GameObject> playerteam;
    [SerializeField]
    public List<GameObject> enemyteam;
    public   Attacktype attackType;
    GameObject target;
    [SerializeField]
    public GameObject orderPanel;
    private enum State { PLAYERTURN, ENEMYTURN, ACTIONCARRYING };
    public enum UI{ADD,SWAP,REMOVE,ADDALL,REMOVEALL};
    State currentState;
    private void Awake()
    {
    // dictionary initialization
        elespritedict["fire"] = Resources.Load<Sprite>("elements/fire");
        elespritedict["water"] = Resources.Load<Sprite>("elements/water");
        elespritedict["earth"] = Resources.Load<Sprite>("elements/earth");
        elespritedict["wind"] = Resources.Load<Sprite>("elements/wind");
        int i = 0;
        foreach (GameObject player in playerteam)
        {
            i += 1;
            battlePriority.Add((0, player.GetComponent<Combatdetails>().speed, player, player.GetComponent<Combatdetails>().sameSpeedpriority));
        }
        foreach (GameObject enemy in enemyteam)
        {
            battlePriority.Add((0, enemy.GetComponent<Combatdetails>().speed, enemy, enemy.GetComponent<Combatdetails>().sameSpeedpriority));
        }
        List<(int, float, GameObject, float)> cloneList = new List<(int, float, GameObject, float)>(battlePriority);
        cloneList.Sort((a, b) => b.Item1 != a.Item1 ? b.Item1.CompareTo(a.Item1) : (b.Item2 != a.Item2 ? b.Item2.CompareTo(a.Item2) : b.Item4.CompareTo(a.Item4)));
        if (cloneList[0].Item3.GetComponent<Combatdetails>().tag == 0)
        {
            for (int j = 0; j < i; j++)
            {
                battlePriority[j] = (1, battlePriority[j].Item2, battlePriority[j].Item3, battlePriority[j].Item4);
            }
        }
        else
        {
            for (int j = i; j < battlePriority.Count; j++)
            {
                battlePriority[j] = (1, battlePriority[j].Item2, battlePriority[j].Item3, battlePriority[j].Item4);
            }
        }
        battlePriority.Sort((a, b) => b.Item1 != a.Item1 ? b.Item1.CompareTo(a.Item1) : (b.Item2 != a.Item2 ? b.Item2.CompareTo(a.Item2) : b.Item4.CompareTo(a.Item4)));
       

        currentPlayer = battlePriority[0].Item3;

        multiplier = 1f;
        buttonCursor.SetActive(false);
        UpdateUI(UI.ADDALL, 0, 0,orderPanel,null);
    }


    public void UpdateUI()
    {
        
    }
    public void Callattack()
    {

        Attack(battlePriority[0].Item3, attackType, target);

    }
    public void Enemyattack(GameObject user ,Attacktype attackType,GameObject targetObject){
        currentState  =State.ACTIONCARRYING;
        isrunningcoroutine = false;

        Combatdetails attackAction = user.GetComponent<Combatdetails>();
        Combatdetails targetDetails = targetObject.GetComponent<Combatdetails>();
        Health targetHealth = targetObject.GetComponent<Health>();
        if(attackAction!=null)
        if(!attackType.type.Equals("heal"))
        targetHealth.Reducehealth(((multiplier*attackAction.baseDamage)-((multiplier*attackAction.baseDamage)*(targetDetails.resistance/100f))));
        target.GetComponent<Animator>().SetTrigger("hurt");
        
        var( setteamPrior,setSpeed, setPlayer,setPriority)  = battlePriority[0];
        setSpeed-=50f;
        Resetall();
        battlePriority[0] = (setteamPrior,setSpeed,setPlayer,setPriority);
    
        Checkcurrentturn();
    }
    public void Callenemyattack()
    {

        Enemyattack(battlePriority[0].Item3, attackType, target);
    }
    
    public void Attack(GameObject user,Attacktype attackType ,GameObject targetObject)
    {  
         currentState  =State.ACTIONCARRYING;
        Combatdetails attackAction;
        attackAction = user.GetComponent<Combatdetails>();
        Combatdetails targetDetails = targetObject.GetComponent<Combatdetails>();
        Health targetHealth = targetObject.GetComponent<Health>();
       
        attackAction.Attack(attackType,targetObject);
       
        if(!attackType.type.Equals("heal"))
        {
        targetHealth.Reducehealth(((multiplier*attackAction.baseDamage)-((multiplier*attackAction.baseDamage)*(targetDetails.resistance/100f))));
        target.GetComponent<Animator>().SetTrigger("hurt");
       
        }
        
        Resetall();
        var( setteamPrior,setSpeed, setPlayer,setPriority)  = battlePriority[0];
        setSpeed-=50f;
        battlePriority[0] = (setteamPrior,setSpeed,setPlayer,setPriority);
       
        
        Checkcurrentturn();
        
    }
    void Newcycle()
    {
        battlePriority.Clear();
        secondturn.Clear();
        print("new cycle started");
        int i = 0;
        foreach (GameObject player in playerteam)
        {
            player.GetComponent<Combatdetails>().eledebuffs.Clear();
            player.GetComponent<Combatdetails>().barindex = 0;
            foreach (Transform bar in player.GetComponent<Combatdetails>().afflictionsbar.transform)
            {
                bar.gameObject.GetComponent<Image>().sprite = null;
                bar.gameObject.SetActive(false);
            }
            i += 1;
            battlePriority.Add((0, player.GetComponent<Combatdetails>().speed, player, player.GetComponent<Combatdetails>().sameSpeedpriority));
        }
        foreach (GameObject enemy in enemyteam)
        {
            enemy.GetComponent<Combatdetails>().eledebuffs.Clear();
            enemy.GetComponent<Combatdetails>().barindex = 0;
            foreach (Transform bar in enemy.GetComponent<Combatdetails>().afflictionsbar.transform)
            {
                bar.gameObject.GetComponent<Image>().sprite = null;
                bar.gameObject.SetActive(false);
            }
            enemy.GetComponent<Combatdetails>().eledebuffs.Clear();
            battlePriority.Add((0, enemy.GetComponent<Combatdetails>().speed, enemy, enemy.GetComponent<Combatdetails>().sameSpeedpriority));
        }
        List<(int, float, GameObject, float)> cloneList = new List<(int, float, GameObject, float)>(battlePriority);
        cloneList.Sort((a, b) => b.Item1 != a.Item1 ? b.Item1.CompareTo(a.Item1) : (b.Item2 != a.Item2 ? b.Item2.CompareTo(a.Item2) : b.Item4.CompareTo(a.Item4)));
        if (cloneList[0].Item3.GetComponent<Combatdetails>().tag == 0)
        {
            for (int j = 0; j < i; j++)
            {
                battlePriority[j] = (1, battlePriority[j].Item2, battlePriority[j].Item3, battlePriority[j].Item4);
            }
        }
        else
        {
            for (int j = i; j < battlePriority.Count; j++)
            {
                battlePriority[j] = (1, battlePriority[j].Item2, battlePriority[j].Item3, battlePriority[j].Item4);
            }
        }
        battlePriority.Sort((a, b) => b.Item1 != a.Item1 ? b.Item1.CompareTo(a.Item1) : (b.Item2 != a.Item2 ? b.Item2.CompareTo(a.Item2) : b.Item4.CompareTo(a.Item4)));
        UpdateUI(UI.ADDALL, 0, 0, orderPanel, null);
     
    }
    public void Setcurrentturn(GameObject nowPlayer)
    {
        if(nowPlayer.GetComponent<Combatdetails>().tag==0)
         { 
            currentPlayer = nowPlayer;
            currentState = State.PLAYERTURN;
         }
        else
        {
            
        currentState = State.ENEMYTURN;
        }
    }
    IEnumerator Applydots(GameObject nowPlayer)
    {   
       
      
        Combatdetails nowDetails = nowPlayer.GetComponent<Combatdetails>();
       
        foreach(KeyValuePair<string, float> dotItem in nowDetails.dotTracker.dots)
            {
               
                yield return new WaitForSeconds(1.5f);
                nowPlayer.GetComponent<Animator>().SetTrigger("hurt");
                nowPlayer.GetComponent<Health>().currentHealth-= dotItem.Value;
            }
            nowDetails.dotTracker.dots.Clear();
        
       
            
        
            StopCoroutine("Applydots");
            Setcurrentturn(nowPlayer);
          }
    public void UpdateUI(UI action,int ind_1,int ind_2,GameObject targetPanel,GameObject targetPanel2)
    {
            GameObject target1 = targetPanel.transform.GetChild(ind_1).gameObject;
            GameObject target2 = targetPanel.transform.GetChild(ind_2).gameObject;
            int i = 0;
        switch (action)
        {

            case UI.REMOVEALL:
                i = 0;
                if(targetPanel2==null)
                foreach (var (teamprior, speed, guy, samespeedprior) in (targetPanel == orderPanel ? battlePriority : secondturn))
                {
                    GameObject curobj = targetPanel.transform.GetChild(i).gameObject;
                    curobj.GetComponent<Image>().sprite = null;
                    curobj.SetActive(false);
                    
                    i += 1;
                }
                else
                {
                    foreach (var (teamprior, speed, guy, samespeedprior) in (targetPanel == orderPanel ? battlePriority : secondturn))
                    {
                        
                        GameObject curobj = targetPanel2.transform.GetChild(i).gameObject;
                        curobj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = speed.ToString();
                        curobj.GetComponent<Image>().sprite = guy.GetComponent<SpriteRenderer>().sprite;
                        curobj.SetActive(true);
                        curobj = targetPanel.transform.GetChild(i).gameObject;
                        curobj.GetComponent<Image>().sprite = null;
                        curobj.SetActive(false);
                        i += 1;
                    }
                }
                break;
            case UI.REMOVE:
                print("remove called");
                target1.GetComponent<Image>().sprite = null;
                GameObject replacement = Instantiate(target1, targetPanel.transform.position, Quaternion.identity);
                target1.transform.SetSiblingIndex(targetPanel.transform.childCount - 1);
                target1.GetComponent<Image>().sprite = null;
                target1.SetActive(false);
        
                
                break;
            case UI.ADDALL:
                i = 0;
              
                    foreach (var (teamprior, speed, guy, samespeedprior) in (targetPanel == orderPanel ? battlePriority : secondturn))
                    {
                        GameObject curobj = targetPanel.transform.GetChild(i).gameObject;
                        curobj.GetComponent<Image>().sprite = guy.GetComponent<SpriteRenderer>().sprite;
                        curobj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = speed.ToString();
                        curobj.SetActive(true);
                        i += 1;
                    }
                
                break;
            case UI.SWAP:
                {
                    GameObject curobj = targetPanel2.transform.GetChild(ind_2).gameObject;
                    var (teamprior, speed, guy, samespeedprior) = (targetPanel == orderPanel ? battlePriority[ind_1] : secondturn[ind_1]);
                    var (teamprior2, speed2, guy2, samespeedprior2) = (targetPanel2 == orderPanel ? battlePriority[ind_2] : secondturn[ind_2]);
                    curobj.GetComponent<Image>().sprite = guy.GetComponent<SpriteRenderer>().sprite;
                    curobj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = speed2.ToString();
                    curobj.SetActive(true);
                    curobj = targetPanel.transform.GetChild(ind_1).gameObject;
                    curobj.GetComponent<Image>().sprite = null;
                    curobj.SetActive(false); 
                    curobj.transform.SetSiblingIndex(targetPanel.transform.childCount - 1);
                }
                break;


        }
    }
    public void Checkcurrentturn()
    {
        

        var (teamprior, speed, player, labelPriority) = battlePriority[0];

        if (speed < 50f)
        {
            UpdateUI(UI.REMOVE, 0, 0, orderPanel,null);
        
            battlePriority.Remove((teamprior, speed, player, labelPriority));
            
        }
        else if (speed >= 50f)
        {
            secondturn.Add((teamprior, speed, player, labelPriority));
            UpdateUI(UI.SWAP, 0,(secondturn.Count - 1), orderPanel, secondturnorderPanel);
            battlePriority.Remove((teamprior, speed, player, labelPriority));
            
        }

        


        if (battlePriority.Count < 1)
        {

            if (secondturn.Count > 0)
            {
                foreach (var (iterteamPrior, iterSpeed, iterPlayer, iterPriority) in secondturn)
                {
                    battlePriority.Add((iterteamPrior, iterSpeed, iterPlayer, iterPriority));
                    

                }
                UpdateUI(UI.REMOVEALL, 0, 0, secondturnorderPanel, orderPanel);
                secondturn.Clear();
            }
            else
            {
                Newcycle();
            }

        }

        battlePriority.Sort((a, b) => b.Item1 != a.Item1 ? b.Item1.CompareTo(a.Item1) : (b.Item2 != a.Item2 ? b.Item2.CompareTo(a.Item2) : b.Item4.CompareTo(a.Item4)));


        var (nowteamprior, nowSpeed, nowPlayer, nowPriority) = battlePriority[0];


        Combatdetails nowDetails = nowPlayer.GetComponent<Combatdetails>();
        if (nowDetails.dotTracker.dots.Count > 0)
        {
            StartCoroutine(Applydots(nowPlayer));
            return;

        }


        Setcurrentturn(nowPlayer);


    }
    IEnumerator Waitforapplication()
    {
        yield return new WaitForSeconds(3f);
        
        StopCoroutine(Waitforapplication());
        print("the coroutine has stopped");
    }
    void Resetall()
    {
        multiplier = 1f;
        playerUi[panelIndex].SetActive(false);
        panelIndex = 0;
        currentPlayer = null;
        playerIndex = 0;
        enemyIndex = 0;
        attackType = null;
       playerUi[panelIndex].SetActive(true);
       buttonCursor.SetActive(false);
       cursor.SetActive(false);
    }
    // Start is called before the first frame update 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
        switch(currentState)
        {
        case State.PLAYERTURN:
        handleplayerturn();
        break;
        case State.ENEMYTURN:
        handleenemyturn();
        break;
        case State.ACTIONCARRYING:
        break;
        }

    }
    
   void handleplayerturn()
    {
        switch (panelIndex)
        {   
            //set up when player turn
            case 0:
                cursor.SetActive(true);
                playerUi[panelIndex].SetActive(false);
                panelIndex += 1;
                playerUi[panelIndex].SetActive(true);
                cursor.transform.position = currentPlayer.transform.position;
                EventSystem.current.SetSelectedGameObject(firstbutton);
                break;
            // choose attack or special attack
            case 1:
                buttonCursor.SetActive(true);
                currentSelectedObject = EventSystem.current.currentSelectedGameObject;
                buttonCursor.GetComponent<RectTransform>().localPosition = currentSelectedObject.GetComponent<RectTransform>().localPosition;

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    playerUi[panelIndex].SetActive(false);

                    if (currentSelectedObject.CompareTag("SPEbutton"))
                    {
                        panelIndex += 2;
                        playerUi[panelIndex].SetActive(true);
                        Attacktype[] iterType = currentPlayer.GetComponent<Combatdetails>().attackTypes;
                        GameObject[] attackButtons = playerUi[panelIndex].GetComponent<Specialattackbuttonmanager>().buttons;

                        for (int i = 0; i < currentPlayer.GetComponent<Combatdetails>().attackTypes.Length - 1; i++)
                        {
                            attackButtons[i].SetActive(true);
                            attackButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = iterType[i + 1].name;
                        }

                        EventSystem.current.SetSelectedGameObject(attackButtons[0]);
                    }
                    else
                    {
                        panelIndex += 1;
                        playerUi[panelIndex].SetActive(true);
                        cursor.SetActive(true);
                        buttonCursor.SetActive(false);
                    }
                }
                break;





            //choose target for selected attack 
            case 2:
                if (Input.GetKeyDown(KeyCode.X))
                {
                    attackType = null;
                    buttonCursor.SetActive(true);
                    cursor.SetActive(false);
                    playerUi[panelIndex].SetActive(false);
                    panelIndex -= 1;
                    playerUi[panelIndex].SetActive(true);
                    EventSystem.current.SetSelectedGameObject(firstbutton);
                }

                cursor.transform.position = enemyteam[enemyIndex].transform.position;

                if (Input.GetKeyDown(KeyCode.DownArrow) && (enemyIndex + 1 < enemyteam.Count))
                {
                    enemyIndex += 1;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) && (enemyIndex - 1 >= 0))
                {
                    enemyIndex -= 1;
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    currentSelectedObject.GetComponent<Button>().onClick.Invoke();
                    target = enemyteam[enemyIndex];
                    currentState = State.ACTIONCARRYING;
                    currentPlayer.GetComponent<Animator>().SetTrigger(atktriggerName);
                }
                break;
            // if special attack was chosen , initiate that attack instead and then switch to case 2(choose target for the attack)
            case 3:
                currentSelectedObject = EventSystem.current.currentSelectedGameObject;
                buttonCursor.GetComponent<RectTransform>().localPosition = currentSelectedObject.GetComponent<RectTransform>().localPosition;

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    playerUi[panelIndex].SetActive(false);
                    panelIndex -= 1;
                    playerUi[panelIndex].SetActive(true);
                    cursor.SetActive(true);
                    buttonCursor.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    GameObject[] attackButtons = playerUi[panelIndex].GetComponent<Specialattackbuttonmanager>().buttons;

                    for (int i = 0; i < currentPlayer.GetComponent<Combatdetails>().attackTypes.Length - 1; i++)
                    {
                        attackButtons[i].SetActive(false);
                    }

                    playerUi[panelIndex].SetActive(false);
                    panelIndex -= 2;
                    playerUi[panelIndex].SetActive(true);
                    EventSystem.current.SetSelectedGameObject(firstbutton);
                }
                break;
        }
    }

        
        
    
    
    IEnumerator AttackPlayer()
    {
        isrunningcoroutine = true;
        yield return new WaitForSeconds(2f);
      
        battlePriority[0].Item3.GetComponent<Animator>().SetTrigger("attack");
        
        StopCoroutine(AttackPlayer());
         
    }
    void handleenemyturn()
    {
        target = playerteam[Random.Range(0,(playerteam.Count-1))];
   
        attackType = battlePriority[0].Item3.GetComponent<Combatdetails>().attackTypes[0];
        if(!isrunningcoroutine)
        StartCoroutine(AttackPlayer());
    }
}
