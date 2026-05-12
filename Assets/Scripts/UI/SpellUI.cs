using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUI : MonoBehaviour
{
    public GameObject icon;
    public RectTransform cooldown;
    public TextMeshProUGUI manacost;
    public TextMeshProUGUI damage;
    public GameObject highlight;
    public ISpell spell;
    float last_text_update;
    const float UPDATE_DELAY = 1;
    public GameObject dropbutton;
    public Button dropButtonComponent;
    private int spellIndex;
    private SpellUIContainer container;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        last_text_update = 0;

        if (dropbutton != null)
        {
            dropButtonComponent = dropbutton.GetComponent<Button>();
            dropButtonComponent.onClick.AddListener(OnDropClicked);
        }
    }

    public void SetDropContext(SpellUIContainer container, int spellIndex)
    {
        this.container = container;
        this.spellIndex = spellIndex;
    }

    private void OnDropClicked()
    {
        if (container != null)
        {
            container.DropSpell(spellIndex);
        }
    }

    public void SetSpell(ISpell spell)
    {
        this.spell = spell;
        GameManager.Instance.spellIconManager.PlaceSprite(spell.GetIcon(), icon.GetComponent<Image>());
    }

    // Update is called once per frame
    void Update()
    {
        if (spell == null) return;
        if (Time.time > last_text_update + UPDATE_DELAY)
        {
            manacost.text = spell.GetManaCost().ToString();
            damage.text = spell.GetDamage().ToString();
            last_text_update = Time.time;
        }
        
        float since_last = Time.time - spell.LastCast;
        float perc;
        if (since_last > spell.GetCooldown())
        {
            perc = 0;
        }
        else
        {
            perc = 1-since_last / spell.GetCooldown();
        }
        cooldown.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48 * perc);
    }
}
