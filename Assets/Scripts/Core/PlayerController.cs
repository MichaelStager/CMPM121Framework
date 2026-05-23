using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public string selectedClass = "mage";
    private Dictionary<string, CharacterClassData> classes;
    private CharacterClassData currentClass;

    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUIContainer spellui;

    public int speed;

    public Unit unit;

    public GameEndUI gameEndUI;

    public List<Relic> relics = new List<Relic>();
    private RelicContext relicContext;






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
    }

    public void StartLevel()
    {
        spellcaster = new SpellCaster(100, 10, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());

        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        classes = CharacterClassLoader.GetClasses();

        if (!classes.TryGetValue(selectedClass, out currentClass))
        {
            Debug.LogWarning("Class " + selectedClass + " not found. Falling back to mage.");
            currentClass = classes["mage"];
        }

        ApplyStatsForWave(1);

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpells(spellcaster.spells, spellcaster.currentSpellIndex);

        relicContext = new RelicContext
        {
            player = this,
            currentWave = 1
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void ApplyStatsForWave(int wave)
    {
        if (currentClass == null)
        {
            return;
        }

        Dictionary<string, int> variables = new Dictionary<string, int>();
        variables["wave"] = wave;

        int newMaxHp = RPNEvaluator.RPNEvaluator.Evaluate(currentClass.health, variables);
        int newMaxMana = RPNEvaluator.RPNEvaluator.Evaluate(currentClass.mana, variables);
        int newManaRegen = RPNEvaluator.RPNEvaluator.Evaluate(currentClass.mana_regeneration, variables);
        int newSpellPower = RPNEvaluator.RPNEvaluator.Evaluate(currentClass.spellpower, variables);
        int newSpeed = RPNEvaluator.RPNEvaluator.Evaluate(currentClass.speed, variables);

        hp.SetMaxHP(newMaxHp);

        spellcaster.max_mana = newMaxMana;
        spellcaster.mana = Mathf.Min(spellcaster.mana, spellcaster.max_mana);
        spellcaster.mana_reg = newManaRegen;
        spellcaster.spellPower = newSpellPower;

        speed = newSpeed;
    }





    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(CastAndRefresh(mouseWorld));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        unit.movement = value.Get<Vector2>()*speed;
        EventBus.Instance.PlayerMoveInput(unit.movement);
    }

    void Die()
    {
        Debug.Log("You Lost");

        unit.movement = Vector2.zero;

        if (gameEndUI != null)
        {
            gameEndUI.ShowLoss();
        }
    }

    IEnumerator CastAndRefresh(Vector3 mouseWorld)
    {
        yield return StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
        spellui.SetSpells(spellcaster.spells, spellcaster.currentSpellIndex);
        EventBus.Instance.SpellCast();

    }

    public bool HasRelic(string relicName)
    {
        return relics.Any(r => r != null && r.Data != null && r.Data.name == relicName);
    }

    public bool AddRelic(RelicData data)
    {
        if (data == null) return false;
        if (HasRelic(data.name)) return false; // no duplicates

        if (relicContext == null)
        {
            relicContext = new RelicContext { player = this, currentWave = 1 };
        }

        Relic relic = RelicFactory.Create(data, relicContext);
        if (relic == null) return false;

        relics.Add(relic);
        relic.Activate();

        Debug.Log("[Relic] Added: " + data.name);
        return true;
    }

    public void SetRelicWave(int wave)
    {
        if (relicContext != null)
        {
            relicContext.currentWave = wave;
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < relics.Count; i++)
        {
            if (relics[i] != null)
            {
                relics[i].Deactivate();
            }
        }
    }


}
