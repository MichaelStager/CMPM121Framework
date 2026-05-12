using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUIContainer spellui;

    public int speed;

    public Unit unit;

    public GameEndUI gameEndUI;

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

        ApplyStatsForWave(1);

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpells(spellcaster.spells, spellcaster.currentSpellIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ApplyStatsForWave(int wave)
    {
        int newMaxHp = 95 + wave * 5;
        int newMaxMana = 90 + wave * 10;
        int newManaRegen = 10 + wave;
        int newSpellPower = wave * 10;
        int newSpeed = 5;

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
    }



}
