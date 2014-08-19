using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatWindow : MonoBehaviour
{
    public TextMesh EventNotifier;

    public Player Player;
    public Monster MonsterPrefab;
    public SpriteRenderer PlayerSpritePrefab;
    public SpriteRenderer BackgroundPrefab;

    public SpriteRenderer PoisonDebuff;
    public SpriteRenderer BlindDebuff;

    private SpriteRenderer _background;
    private Monster _monster;

    private int _monsterCurrAttacks = 0;

    private readonly CwActorInfo _playerInfo = new CwActorInfo {CombatPeriod = 0.5f, PauseTime = 0.1f};
    private readonly CwActorInfo _monsterInfo = new CwActorInfo {CombatPeriod = 0.5f, PauseTime = 1.0f};

    private class CwActorInfo
    {
        public SpriteRenderer Sprite;
        public Animator Animator;
        public float CombatPeriod;
        public float NextCombatTime; // Probably should not be here
        public float PauseTime;
        public bool Defending;
        public float AttackChance;
        public float NextPoisonTick;
        public float BlindLength;
        public Vector3 Location;
        public Vector3 DamageLocation;
    }

    private float _roll;

    private readonly Dictionary<string, SpriteRenderer> _playerBuffs = new Dictionary<string, SpriteRenderer>();
    private readonly Dictionary<string, SpriteRenderer> _monsterBuffs = new Dictionary<string, SpriteRenderer>();

    private bool BuffUpdate;

    void Update()
    {
        if (Player.Health <= 0)
        {
            Debug.Log("You die!");
            DestroyWindow();
        }
        else if (_monster.Health <= 0)
        {
            Debug.Log("Monster dies!");
            DestroyWindow();
        }

        // End player blinding status effect if it has expired
        if (Time.time > _playerInfo.BlindLength && _playerInfo.BlindLength > 0)
        {
            _playerInfo.AttackChance = Player.Accuracy / _monster.Evasion;
            Player.IsBlinded = false;
            _playerInfo.BlindLength = 0.0f;
            BuffUpdate = true;
            Debug.Log("Player is no longer blinded.");
        }

        // End monster blinding status effect if it has expired
        if (Time.time > _monsterInfo.BlindLength && _monsterInfo.BlindLength > 0)
        {
            _monsterInfo.AttackChance = _monster.Accuracy / Player.Evasion;
            _monster.IsBlinded = false;
            _monsterInfo.BlindLength = 0.0f;
            BuffUpdate = true;
            Debug.Log("Monster is no longer blinded.");
        }

        //Distribute poison damage to Player
        HandlePoison(Player, _playerInfo);

        //Distribute poison damage to Monster
        HandlePoison(_monster, _monsterInfo);

        if (BuffUpdate)
            DisplayBuffs();

        // Player Attack
        if (Input.GetKey("up") && Time.time > _playerInfo.NextCombatTime)
            HandleAttack(Player, _playerInfo, _monster, _monsterInfo);
        // Player Defense
        else if (Input.GetKey("down") && Time.time > _playerInfo.NextCombatTime)
            HandleDefend(Player, _playerInfo);
        // Player Idle
        else if (Time.time > _playerInfo.NextCombatTime - _playerInfo.PauseTime)
            HandleIdle(Player, _playerInfo);

        //Monster Attack
        if (_monsterCurrAttacks < _monster.NumAttacks && Time.time > _monsterInfo.NextCombatTime)
        {
            HandleAttack(_monster, _monsterInfo, Player, _playerInfo);
            ++_monsterCurrAttacks;
        }
        // Monster Defense
        else if (_monsterCurrAttacks >= _monster.NumAttacks && Time.time > _monsterInfo.NextCombatTime)
        {
            HandleDefend(_monster, _monsterInfo);
            _monsterCurrAttacks = 0;
        }
        // Monster Idle
        else if (Time.time > _monsterInfo.NextCombatTime - _monsterInfo.PauseTime)
            HandleIdle(_monster, _monsterInfo);
    }

    // Poison Damage
    //
    // Poison damage works as a "fading" poison. When an actor takes poison damage, the fade damage value is divided in half.
    // Once the fade damage value is below 1, the actor is no longer poisoned.
    //
    // For example: A player becomes poisoned. The first tick does 4 damage, the second does 2, the third does 1.
    //              After that, the player is no longer poisoned.

    private void HandlePoison(Actor actor, CwActorInfo info)
    {
        if (actor.IsPoisoned && Time.time > info.NextPoisonTick)
        {
            info.NextPoisonTick = Time.time + actor.TakingPoisonTickSpeed;

            Debug.Log(actor.DisplayName + " takes " + actor.TakingPoisonFadeValue + " poison damage!");
            actor.Health -= actor.TakingPoisonFadeValue;

            DisplayMessage(actor.TakingPoisonFadeValue.ToString(), new Color(170f / 255f, 0, 250f / 255f, 1), info.DamageLocation);

            if (actor.TakingPoisonFadeValue <= 1)
            {
                actor.IsPoisoned = false;
                BuffUpdate = true;
                actor.TakingPoisonFadeValue = 0;
                actor.TakingPoisonTickSpeed = 0;
                info.Sprite.color = Color.white;
                Debug.Log(actor.DisplayName + " is no longer poisoned.");
            }

            actor.TakingPoisonFadeValue = actor.TakingPoisonFadeValue / 2;
        }
    }

    private void HandleIdle(Actor actor, CwActorInfo info)
    {
        info.Animator.SetBool("attacking", false);
        info.Animator.SetBool("defending", false);
        info.Defending = false;
    }

    private void HandleDefend(Actor actor, CwActorInfo info)
    {
        info.NextCombatTime = Time.time + info.CombatPeriod + info.PauseTime;
        info.Animator.SetBool("defending", true);
        info.Defending = true;
    }

    private void HandleAttack(Actor attacker, CwActorInfo attackerInfo, Actor defender, CwActorInfo defenderInfo)
    {
        attackerInfo.NextCombatTime = Time.time + attackerInfo.CombatPeriod + attackerInfo.PauseTime;
        attackerInfo.Animator.SetBool("attacking", true);
        attackerInfo.Defending = false;

        if (!defenderInfo.Defending)
        {
            _roll = Random.Range(0f, 1f);

            if (attackerInfo.AttackChance > _roll)
            {
                int damage = attacker.GetAttackValue();
                _monster.Health -= damage;

                Debug.Log(string.Format("{1} is hit for {0}! {1} Health: {2}", damage, defender.DisplayName, _monster.Health));
                DisplayMessage(damage.ToString(), Color.red, defenderInfo.DamageLocation);

                // Poison Chance
                //
                // Each attack, the actor has a chance to poison IF the actor IsPoisonous. 
                // If the other actor is already poisoned and it procs, the fade damage value is refreshed.

                _roll = Random.Range(0f, 1f);
                if (attacker.IsPoisonous && attacker.PoisonChance > _roll)
                {
                    BuffUpdate = true;
                    Debug.Log(string.Format("{0} is poisoned for {1} damage.", defender.DisplayName, attacker.PoisonDamageValue));
                    defender.TakingPoisonFadeValue = attacker.PoisonDamageValue;
                    defender.TakingPoisonTickSpeed = attacker.PoisonTickSpeed;
                    defender.IsPoisoned = true;

                    defenderInfo.Sprite.color = new Color(230f/255f, 0, 250f/255f, 1);
                }

                // Blind Chance
                //
                // Each attack, the actor has a chance to poison IF the actor IsBlinding.
                // If the other actor is already blinded and it procs, the blind length is refreshed.
                _roll = Random.Range(0f, 1f);
                if (attacker.IsBlinding && attacker.BlindChance > _roll)
                {
                    if (!defender.IsBlinded)
                    {
                        BuffUpdate = true;
                        Debug.Log(string.Format("{0} is blinded! Accuracy cut in half for {1} seconds.", defender.DisplayName, attacker.BlindAttackLength));
                        defenderInfo.AttackChance = defenderInfo.AttackChance/2.0f;
                        defenderInfo.BlindLength = Time.time + attacker.BlindAttackLength;
                        defender.IsBlinded = true;
                    }
                    else
                    {
                        BuffUpdate = true;
                        Debug.Log(string.Format("{0} is blinded! Accuracy cut in half for {1} seconds.", defender.DisplayName, attacker.BlindAttackLength));
                        defenderInfo.BlindLength = Time.time + attacker.BlindAttackLength;
                    }
                }
            }
            else Debug.Log(attacker.DisplayName + " misses.");
        }
        else Debug.Log(defender.DisplayName + " blocks " + attacker.DisplayName + "'s attack!");
    }

    public void Enable()
    {
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float halfCamWidth = camWidth / 2f;
        float halfCamHeight = cam.orthographicSize;

        //Background
        _background = (SpriteRenderer)Instantiate(BackgroundPrefab, transform.position, Quaternion.identity);

        _background.enabled = false;

        float bgWidth = _background.transform.renderer.bounds.max.x - _background.transform.renderer.bounds.min.x;
        float bgHeight = _background.transform.renderer.bounds.max.y - _background.transform.renderer.bounds.min.y;

        float bgWidthScale = (camWidth / bgWidth);
        float bgHeightScale = (camHeight / bgHeight);

        _background.transform.localScale = new Vector3(bgWidthScale, bgHeightScale, 0);
        _background.transform.position = new Vector3(transform.position.x, Player.transform.position.y, Player.transform.position.z);
        _background.sortingLayerName = "Background";
        _background.enabled = true;

        #region Debug Logging
        Debug.Log("Background Local Scale = " + _background.transform.localScale);
        Debug.Log("Screen Width = " + Screen.width);
        Debug.Log("Screen Height = " + Screen.height);

        Debug.Log("Camera Local Scale = " + cam.transform.localScale);
        Debug.Log("Camera Width = " + camWidth);
        Debug.Log("Camera Height = " + camHeight);
        Debug.Log("bgWidthScale = " + bgWidthScale);
        #endregion

        //PlayerSprite
        InitActor(Player, _playerInfo, -(halfCamWidth*0.68f), -(halfCamHeight*0.57f), PlayerSpritePrefab, _playerBuffs);

        //MonsterSprite
        _monster = (Monster)Instantiate(MonsterPrefab, transform.position, Quaternion.identity);

        InitActor(_monster, _monsterInfo, +(halfCamWidth*0.68f), -(halfCamHeight*0.57f),
            MonsterPrefab.GetComponent<SpriteRenderer>(), _monsterBuffs);

        // Player / Monster Buffs
        Debug.Log("Player Accuracy / Evasion: " + Player.Accuracy + " " + Player.Evasion);
        Debug.Log("Monster Accuracy / Evasion: " + _monster.Accuracy + " " + _monster.Evasion);

        //Initial Attack Chances
        _playerInfo.AttackChance = Player.Accuracy / _monster.Evasion;
        Debug.Log("Player Attack Chance = " + _playerInfo.AttackChance);
        _monsterInfo.AttackChance = _monster.Accuracy / Player.Evasion;
        Debug.Log("Monster Attack Chance = " + _monsterInfo.AttackChance);

        _playerInfo.Animator.speed = 2;
    }

    private void InitActor(Actor actor, CwActorInfo info, float x, float y, SpriteRenderer prefab,
        Dictionary<string, SpriteRenderer> buffs)
    {
        info.Sprite = (SpriteRenderer)Instantiate(prefab, transform.position, Quaternion.identity);
        //_monsterInfo.Sprite = _monster.GetComponent<SpriteRenderer>();
        info.Sprite.enabled = false;

        float xPos = Player.transform.position.x + x;
        float yPos = Player.transform.position.y + y;

        info.Sprite.transform.position = new Vector3(xPos, yPos, 0);
        info.Location = info.Sprite.transform.position;
        info.DamageLocation = new Vector3(xPos, yPos + 0.5f, 0);
        info.Sprite.sortingLayerName = "Midground";
        info.Sprite.enabled = true;
        info.Animator = info.Sprite.GetComponent<Animator>();

        buffs.Clear();
        buffs.Add("Poison", (SpriteRenderer) Instantiate(PoisonDebuff, transform.position, Quaternion.identity));
        buffs.Add("Blind", (SpriteRenderer) Instantiate(BlindDebuff, transform.position, Quaternion.identity));
    }

    public void DestroyWindow()
    {
        Destroy(_monster);
        Destroy(_playerInfo.Sprite);
        Destroy(_monsterInfo.Sprite);
        Destroy(_background);
        Destroy(this);
    }

    public void DisplayBuffs()
    {

        Debug.Log("Displaying Debuffs");

        //Player Debuffs
        ShowBuffs(Player, _playerInfo, _playerBuffs);
        ShowBuffs(_monster, _monsterInfo, _monsterBuffs);

        BuffUpdate = false;
    }

    private void ShowBuffs(Actor actor, CwActorInfo info, Dictionary<string, SpriteRenderer> buffs)
    {
        if (actor.IsPoisoned)
            buffs["Poison"].enabled = true;
        else
        {
            buffs["Poison"].enabled = false;
            buffs["Poison"].sortingLayerName = "Idle";
        }

        if (Player.IsBlinded)
            buffs["Blind"].enabled = true;
        else
        {
            buffs["Blind"].enabled = false;
            buffs["Blind"].sortingLayerName = "Idle";
        }

        Vector3 buffLocations = new Vector3(info.Location.x - 0.3f, info.Location.y + 1.5f, 0);

        foreach (SpriteRenderer buff in buffs.Values)
            if (buff.enabled)
            {
                buff.transform.position = buffLocations;
                buff.sortingLayerName = "Foreground";
                buffLocations.x += 0.5f;
            }
    }

    protected void DisplayMessage(string text, Color color, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f)
    {
        Vector3 offset = new Vector3(xOffset, yOffset, zOffset);

        DisplayMessage(text, color, offset);
    }

    protected void DisplayMessage(string text, Color color, Vector3 location)
    {
        TextMesh message = (TextMesh)Instantiate(EventNotifier, location, Quaternion.identity);
        message.color = color;
        message.text = text;
    }
}