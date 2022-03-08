using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Monster monsterA;
    [SerializeField] private Monster monsterB;

    [SerializeField] private MonsterUIHandler monsterAUI;
    [SerializeField] private MonsterUIHandler monsterBUI;
    
    [SerializeField] private TextMeshProUGUI commentary;

    private GameInput input;
    
    #region UnityEventFunctions
    private void Awake()
    {
        input = new GameInput();
    }

    private void Start()
    {   
        RegisterNewParticipants(monsterA, monsterB);
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

    private void RegisterNewParticipants(Monster contestantA, Monster contestantB)
    {
        UpdateTitle(contestantA, monsterAUI);
        UpdateTitle(contestantB, monsterBUI);
        UpdateHealth(contestantA, monsterAUI);
        UpdateHealth(contestantB, monsterBUI);
    }
    
    private void UpdateHealth(Monster monster, MonsterUIHandler monsterUIHandler)
    {
        monsterUIHandler.UpdateHealth(monster.GetCurrentHealth(), monster.GetMaxHealth());
    }

    private void UpdateTitle(Monster monster, MonsterUIHandler monsterUIHandler)
    {
        monsterUIHandler.UpdateTitle(monster.GetTitle());
        commentary.SetText($"{monsterA.GetTitle()} trifft auf {monsterB.GetTitle()}");
    }

    #endregion
}
