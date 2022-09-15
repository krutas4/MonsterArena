using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Monster> monsterprefabs;
     private Monster monsterA;
     private Monster monsterB;
    [SerializeField] private Transform monsterSlotA;
    [SerializeField] private Transform monsterSlotB;
    [SerializeField] private MonsterUi monsterAUi;
    [SerializeField] private MonsterUi monsterBUi;

    [SerializeField] private TextMeshProUGUI commenteryText;

    private GameInput input;

    private bool isMonsterATurn = true;

    private void Awake()
    {
        input = new GameInput();
        input.Player.Next.performed += PerformNewAction;
    }
    private void OnEnable()
    {
        input.Enable();
    }
    

    private void Start()
    {
        StartNewBattle();

        commenteryText.SetText(sourceText:$"{monsterA.GetTitle()} trift auf {monsterB.GetTitle()}!");
    }

    private void OnDisable()
    {
        input.Disable();
    }
    private void OnDestroy()
    {
        input.Player.Next.performed -= PerformNewAction;
    }
    private void StartNewBattle()
    {
        int challangerAIndex=Random.Range(0,monsterprefabs.Count);// [0,5)
        int challangerBIndex = Random.Range(0, monsterprefabs.Count);// [0,5)
        if (monsterprefabs.Count >= 2)
        {
            while (challangerAIndex == challangerBIndex)
            {
                challangerBIndex = Random.Range(0, monsterprefabs.Count);
            }
            
        }
        Monster challangeA=monsterprefabs[challangerAIndex];
        Monster challangeB=monsterprefabs[challangerBIndex];

        monsterA = RegisterNewMonster(challangeA, monsterSlotA, monsterAUi);
        monsterB = RegisterNewMonster(challangeB, monsterSlotB, monsterBUi);

        commenteryText.SetText(sourceText: $"{monsterA.GetTitle()} trifft auf {monsterB.GetTitle()}");

    }
   
    private void PerformNewAction(InputAction.CallbackContext context)
    {

        if(monsterA.HasFainted() || monsterB.HasFainted())
        {
            StartNewBattle();
            return;
        }
        Debug.Log("Enter key pressed");
        commenteryText.SetText("Nächste Aktion!");
        //ToDo
        Monster attacker;
        Monster defender;
        MonsterUi defenderUi;
        if (isMonsterATurn)// Monster A greift an
        {
            attacker = monsterB;
            defender = monsterA;
            defenderUi = monsterAUi;
        }
        else
        {
            attacker = monsterA;
            defender = monsterB;
            defenderUi = monsterBUi;
        }
        string attackDescription = attacker.Attack(defender);
        UpdateHealth(defender, defenderUi);
        commenteryText.SetText(attackDescription);
        isMonsterATurn=!isMonsterATurn;
       
    }
    private Monster RegisterNewMonster(Monster monsterPrefab,Transform monsterSlot, MonsterUi monsterUi)
    {
        // TODO Remove old monsters slot
        // Spawn monster from slot
        Monster newSpawn= Instantiate(monsterPrefab, monsterSlot);

        UpdateTitle(monsterPrefab, monsterUi);
        UpdateHealth(monsterPrefab, monsterUi);

        return newSpawn;
    }
    private void ClearSlot(Transform slot)
    {
        for(int i = 0; i < slot.childCount - 1; i--)
        {
            Transform child = slot.GetChild(i);
            Destroy(child.gameObject);
        }
    }
    private void UpdateTitle(Monster monster, MonsterUi monsterUi)
    {
        monsterUi.UpdateTitle(monster.GetTitle());
    }
    private void UpdateHealth(Monster monster, MonsterUi monsterUi)
    {
        monsterUi.UpdateHealth(monster.GetCurrentHealth(), monster.GetMaxHealth());
    }
}
