using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Monster> monsterPrefabs;
    
    [SerializeField] private Transform monsterSlotA;
    [SerializeField] private Transform monsterSlotB;

    [SerializeField] private MonsterUIHandler monsterAUI;
    [SerializeField] private MonsterUIHandler monsterBUI;
    
    [SerializeField] private TextMeshProUGUI commentary;

    private GameInput input;
    private Monster monsterA;
    private Monster monsterB;

    #region UnityEventFunctions

    private void Awake()
    {
        input = new GameInput();
    }

    private void Start()
    {
        StartNewBattle();
        
    }
    
    private void OnEnable()
    {
        input.Enable();
        input.Player.Next.performed += PerformNextAction;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Next.performed -= PerformNextAction;
    }

    #endregion

    #region BattleLogic
    private bool monsterATurn;
    
    private void PerformNextAction(InputAction.CallbackContext _)
    {
        if (monsterA.HasFainted() || monsterB.HasFainted())
        {
            StartNewBattle();
            return;
        }
        
        Monster attacker;
        Monster attackee;

        if (monsterATurn)
        {
            attacker = monsterA;
            attackee = monsterB;
        }
        else
        {
            attacker = monsterB;
            attackee = monsterA;
        }

        string attackDescription = attacker.Attack(attackee);
        
        UpdateHealth(attackee, monsterATurn ? monsterBUI : monsterAUI);
        
        commentary.SetText(attackDescription);
        monsterATurn = !monsterATurn;
    }

    private void StartNewBattle()
    {
        // Select a random number between 0 and the element count of the monster prefab list (exclusive)
        int challengerAIndex = Random.Range(0, monsterPrefabs.Count);
        int challengerBIndex = Random.Range(0, monsterPrefabs.Count);
        
        // Use the selected numbers to pick the corresponding monster
        Monster challengerA = monsterPrefabs[challengerAIndex];
        Monster challengerB = monsterPrefabs[challengerBIndex];
        
        // Spawn new monsters and update their UIs
        monsterA = RegisterNewParticipant(challengerA, monsterAUI, monsterSlotA);
        monsterB = RegisterNewParticipant(challengerB, monsterBUI, monsterSlotB);
    
        // Update commentary to announce battle
        commentary.SetText($"{monsterA.GetTitle()} trifft auf {monsterB.GetTitle()}");
    }

    private Monster RegisterNewParticipant(Monster monsterPrefab, MonsterUIHandler monsterUI, Transform monsterSlot)
    {
        ClearSlot(monsterSlot);
        Monster newSpawned = Instantiate(monsterPrefab, monsterSlot);
        UpdateTitle(newSpawned, monsterUI);
        UpdateHealth(newSpawned, monsterUI);
        return newSpawned;
    }
    
    private void UpdateHealth(Monster monster, MonsterUIHandler monsterUIHandler)
    {
        monsterUIHandler.UpdateHealth(monster.GetCurrentHealth(), monster.GetMaxHealth());
    }

    private void UpdateTitle(Monster monster, MonsterUIHandler monsterUIHandler)
    {
        monsterUIHandler.UpdateTitle(monster.GetTitle());
    }

    private void ClearSlot(Transform slot)
    {
        for (var i = slot.childCount - 1; i >= 0; i--)
        {
            Destroy(slot.GetChild(i).gameObject);
        }
    }

    #endregion
}
