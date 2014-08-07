using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ActorInfoUI : MonoBehaviour
{
    public Actor Actor;
    public Texture2D HealthBarBackground;
    public Texture2D HealthBarForeground;

    private int _lastSelection = -1;
    private int _selection = -1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetMouseButtonUp(0))
	    {
	        if (_selection >= 0)
	        {
	            if (_selection == _lastSelection) _lastSelection = -1; // Toggle selection back off
	            else _lastSelection = _selection;
	        }
	        else _lastSelection = -1;
	    }
	}

    void OnGUI()
    {
        DisplayInventory();
        DisplayHealthBar();
    }

    private void DisplayHealthBar()
    {
        GUI.DrawTexture(new Rect(Screen.width - 110f, 10, 100, 10), HealthBarBackground);
        GUI.DrawTexture(new Rect(Screen.width - 110f, 10, 100f*(Mathf.Max(Actor.Health, 0f))/Actor.MaxHealth, 10f),
            HealthBarForeground);
    }

    private void DisplayInventory()
    {
        _selection = -1;
        GUI.skin.box.wordWrap = true;
        var contents = new List<GUIContent>();  
        if (Actor.WeaponSlot != null)
            contents.Add(new GUIContent(Actor.WeaponSlot.GetComponent<SpriteRenderer>().sprite.texture,
                Actor.WeaponSlot.ItemDescription));
        if (Actor.ShieldSlot != null)
            contents.Add(new GUIContent(Actor.ShieldSlot.GetComponent<SpriteRenderer>().sprite.texture,
                Actor.ShieldSlot.ItemDescription));
        if (Actor.MiscSlot != null)
            contents.Add(new GUIContent(Actor.MiscSlot.GetComponent<SpriteRenderer>().sprite.texture,
                Actor.MiscSlot.ItemDescription));

        if (contents.Count > 0)
            _selection = GUI.SelectionGrid(new Rect(10, 10, 64, 64 * contents.Count), _selection, contents.ToArray(), 1);

        if (_lastSelection >= 0)
            GUI.Box(new Rect(80, _lastSelection * 65 + 10, 150f, 100f),
                contents[_lastSelection].tooltip);
    }
}
