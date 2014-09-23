using System;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CombatWindow : MonoBehaviour
{
    public TextMesh EventNotifier;

    public Player Player;
    public Monster MonsterPrefab;
    public SpriteRenderer PlayerSpritePrefab;
    public SpriteRenderer MonsterSpritePrefab;
    public SpriteRenderer BackgroundPrefab;
    public SpriteRenderer FramePrefab;

    public TextAsset MonsterPattern;

    private SpriteRenderer _background;
    private SpriteRenderer _frame;
    private Monster _monster;

    private List<MoveContainer> _monsterMoves;
    private int _moveIndex = 0;
    private MoveType _currentMonsterMove = MoveType.Idle;
    private MoveType _currentPlayerMove = MoveType.Idle;
    private float _lastMonsterActionTime;

    private readonly Dictionary<string, SpriteRenderer> _playerBuffs = new Dictionary<string, SpriteRenderer>();
    private readonly Dictionary<string, SpriteRenderer> _monsterBuffs = new Dictionary<string, SpriteRenderer>();

    private void Update()
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

        ShowStatusEffects(Player, _playerBuffs);
        ShowStatusEffects(_monster, _monsterBuffs);

        //Debug.Log("Player Attack Val: " + Player.CwInfo.Animator.GetFloat("Attack"));

        if (_currentPlayerMove == MoveType.Attack)
        {
            HandleAttackAnim(Player,_monster);
        }

        if (_currentMonsterMove == MoveType.Attack)
        {
            HandleAttackAnim(_monster,Player);
        }

        if (_currentPlayerMove == MoveType.Idle)
        { // We only want the player to be able to perform moves from the idle position
            if (Input.GetKey("up"))
            {
                _currentPlayerMove = MoveType.Attack;
                HandleAttack(Player);
            }
            else if (Input.GetKey("down"))
            {
                _currentPlayerMove = MoveType.Defend;
                HandleDefend(Player);
            }
        }
        else if (Player.CwInfo.Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle") && _currentPlayerMove != MoveType.Idle)
        {
            _currentPlayerMove = MoveType.Idle;
            HandleIdle(Player);
        }



        if (Time.time > _lastMonsterActionTime + _monsterMoves[_moveIndex].Delay)
        { // The monster performs the next action in its pattern whenever the delay for that move is exceeded
            _lastMonsterActionTime = Time.time;
            _currentMonsterMove = _monsterMoves[_moveIndex].MoveType;
            switch (_currentMonsterMove)
            {
                case MoveType.Attack:
                    HandleAttack(_monster);
                    break;
                case MoveType.Defend:
                    HandleDefend(_monster);
                    break;
            }
            _moveIndex = (_moveIndex + 1)%_monsterMoves.Count;
        }
        else if (_monster.CwInfo.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") && _currentMonsterMove != MoveType.Idle)
        { // We check for _monsterState != MoveType.Idle because this should only happen exactly when the monster becomes idle
            _currentMonsterMove = MoveType.Idle;
            HandleIdle(_monster);
        }
    }

    private void HandleIdle(Actor actor)
    {
        actor.CwInfo.Animator.SetInteger("State", 0);
        actor.CwInfo.Defending = false;
        actor.CwInfo.AttackLock = false;
    }

    private void HandleDefend(Actor actor)
    {
        actor.CwInfo.Animator.SetInteger("State", -1);
        actor.CwInfo.Defending = true;
        actor.CwInfo.AttackLock = false;
    }

    private void HandleAttack(Actor actor)
    {
        actor.CwInfo.Animator.SetInteger("State", 1);
        actor.CwInfo.Defending = false;
        actor.CwInfo.AttackLock = false;
    }

    private void HandleAttackAnim(Actor attacker, Actor defender)
    {
        if (attacker.CwInfo.Animator.GetFloat("Attack") > 10.0f && attacker.CwInfo.AttackLock == false)
        {
            HandleDamage(attacker, defender);
            attacker.CwInfo.AttackLock = true;
        }

        if (attacker.CwInfo.AttackLock && attacker.CwInfo.Animator.GetFloat("Attack") < 10.0f && attacker.CwInfo.Animator.GetFloat("FreeAnim") > 1.0f)
        {
            attacker.CwInfo.AttackLock = false;
        }
    }

    private void HandleDamage(Actor attacker, Actor defender) 
    {
        if (!(defender.CwInfo.Animator.GetFloat("Defend") > 1.0f))
        {
            float roll = Random.Range(0f, 1f);

            if (attacker.CwInfo.AttackChance > roll)
            {
                int damage = attacker.GetAttackValue();
                defender.Health -= damage;

                Debug.Log(string.Format("{1} is hit for {0}! {1} Health: {2}", damage, defender.DisplayName, _monster.Health));
                DisplayMessage(damage.ToString(), Color.red, defender.CwInfo.DamageLocation);

                // Poison Chance
                //
                // Each attack, the actor has a chance to poison IF the actor IsPoisonous. 
                // If the other actor is already poisoned and it procs, the fade damage value is refreshed.
                roll = Random.Range(0f, 1f);
                if (attacker.IsPoisonous && attacker.PoisonChance > roll)
                {
                    Debug.Log(string.Format("{0} is poisoned for {1} damage.", defender.DisplayName,
                        attacker.PoisonDamageValue));
                    defender.AddStatusEffect("PoisonStatusEffect",
                        new PoisonStatusEffect(attacker.PoisonDamageValue, attacker.PoisonTickSpeed));
                }

                // Blind Chance
                //
                // Each attack, the actor has a chance to poison IF the actor IsBlinding.
                // If the other actor is already blinded and it procs, the blind length is refreshed.
                roll = Random.Range(0f, 1f);
                if (attacker.IsBlinding && attacker.BlindChance > roll)
                {
                    Debug.Log(string.Format("{0} is blinded! Accuracy cut in half for {1} seconds.",
                        defender.DisplayName, attacker.BlindAttackLength));
                    defender.AddStatusEffect("BlindStatusEffect", new BlindStatusEffect(attacker.BlindAttackLength));
                }
            }
            //else Debug.Log(attacker.DisplayName + " misses.");
        }
        else Debug.Log(defender.DisplayName + " blocks " + attacker.DisplayName + "'s attack!");
    }

    public void Enable()
    {
        Camera cam = Camera.main;
        //float camHeight = 2f * cam.orthographicSize;
        //float camWidth = camHeight * cam.aspect

        //Background
        _background = (SpriteRenderer)Instantiate(BackgroundPrefab);
        _background.transform.parent = this.transform;

        //Frame
        _frame = (SpriteRenderer)Instantiate(FramePrefab);
        _frame.transform.parent = this.transform;

        float bgWidth = _background.transform.renderer.bounds.max.x - _background.transform.renderer.bounds.min.x;
        float bgHeight = _background.transform.renderer.bounds.max.y - _background.transform.renderer.bounds.min.y;

        float frameWidth = _frame.transform.renderer.bounds.max.x - _frame.transform.renderer.bounds.min.x;
        float frameHeight = _frame.transform.renderer.bounds.max.y - _frame.transform.renderer.bounds.min.y;

        Vector3 bl = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 ur = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        float camHeight = ur.y - bl.y;
        float camWidth = ur.x - bl.x;

        float halfCamWidth = camWidth / 2f;
        float halfCamHeight = cam.orthographicSize;

        float bgWidthScale = (camWidth / bgWidth);
        float bgHeightScale = (camHeight / bgHeight);

        float frameWidthScale = (camWidth / frameWidth);
        float frameHeightScale = (camHeight / frameHeight);

        _background.transform.localScale = new Vector3(bgWidthScale, bgHeightScale, 0);
        _background.transform.position = new Vector3((bl.x + ur.x) * 0.5f, (bl.y + ur.y) * 0.5f, 5f); //new Vector3(transform.position.x, Player.transform.position.y, Player.transform.position.z);
        _background.sortingLayerName = "Background";
        _background.enabled = true;

        _frame.transform.localScale = new Vector3(frameWidthScale, frameHeightScale, 0);
        _frame.transform.position = new Vector3((bl.x + ur.x) * 0.5f, (bl.y + ur.y) * 0.5f, 5f);
        _frame.sortingLayerName = "Front";
        _frame.enabled = true;

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
        InitActor(Player, -(halfCamWidth * 0.6f), -(halfCamHeight * 0.75f), PlayerSpritePrefab, _playerBuffs);

        //MonsterSprite
        _monster = (Monster)Instantiate(MonsterPrefab, transform.position, Quaternion.identity);
        _monster.transform.parent = this.transform;
        InitActor(_monster, +(halfCamWidth * 0.6f), -(halfCamHeight * 0.75f), MonsterSpritePrefab, _monsterBuffs);


        string[] lines = MonsterPattern.text.Split('\n');
        _monsterMoves = new List<MoveContainer>(lines.Length);
        for(int i = 0; i < lines.Length; ++i)
        {
            if (lines[i] == "" || lines[i][0] == '#') continue;
            string[] tokens = lines[i].Split(';');
            float delay = float.Parse(tokens[0]);
            MoveType moveType = (MoveType)Enum.Parse(typeof(MoveType), tokens[1], true);
            _monsterMoves.Add(new MoveContainer {Delay = delay, MoveType = moveType});
            Debug.Log(moveType + " " + delay);
        }

        // Player / Monster Buffs
        Debug.Log("Player Accuracy / Evasion: " + Player.Accuracy + " " + Player.Evasion);
        Debug.Log("Monster Accuracy / Evasion: " + _monster.Accuracy + " " + _monster.Evasion);

        //Initial Attack Chances
        Player.CwInfo.AttackChance = Player.Accuracy / _monster.Evasion;
        Debug.Log("Player Attack Chance = " + Player.CwInfo.AttackChance);
        _monster.CwInfo.AttackChance = _monster.Accuracy / Player.Evasion;
        Debug.Log("Monster Attack Chance = " + _monster.CwInfo.AttackChance);

        //Player.CwInfo.Animator.speed = 2;
    }

    private void InitActor(Actor actor, float x, float y, SpriteRenderer prefab, Dictionary<string, SpriteRenderer> buffs)
    {
        actor.CwInfo.Sprite = (SpriteRenderer)Instantiate(prefab, transform.position, Quaternion.identity);
        //_monsterInfo.Sprite = _monster.GetComponent<SpriteRenderer>();
        actor.CwInfo.Sprite.transform.parent = this.transform;
        actor.CwInfo.Sprite.enabled = false;

        float xPos = Player.transform.position.x + x;
        float yPos = Player.transform.position.y + y;

        actor.CwInfo.Sprite.transform.position = new Vector3(xPos, yPos, 0);
        actor.CwInfo.Location = actor.CwInfo.Sprite.transform.position;
        actor.CwInfo.DamageLocation = new Vector3(xPos, yPos + 0.5f, 0);
        actor.CwInfo.Sprite.sortingLayerName = "Midground";
        actor.CwInfo.Sprite.enabled = true;
        actor.CwInfo.Animator = actor.CwInfo.Sprite.GetComponent<Animator>();

        buffs.Clear();
        Sprite[] seTextures = Resources.LoadAll<Sprite>("StatusEffects");
        foreach (var sprite in seTextures)
        {
            Debug.Log(sprite.name);
            var g = new GameObject(sprite.name);
            g.transform.parent = transform;
            var s = g.AddComponent<SpriteRenderer>();
            s.sprite = sprite;
            buffs.Add(sprite.name, s);
        }
    }

    public void DestroyWindow()
    {
        Destroy(_monster);
        Destroy(_background);
        Destroy(Player.CwInfo.Sprite);
        Destroy(_monster.CwInfo.Sprite);
        foreach(var b in _monsterBuffs.Values)
            Destroy(b);
        foreach(var b in _playerBuffs.Values)
            Destroy(b);
        Destroy(this);
    }

    private void ShowStatusEffects(Actor actor, Dictionary<string, SpriteRenderer> buffs)
    {
        if (actor.StatusEffects.Count == 0 || buffs.Count == 0) return;

        Vector3 buffLocations = new Vector3(actor.CwInfo.Location.x - 0.3f, actor.CwInfo.Location.y + 3.5f, 0);

        foreach (var kvp in buffs)
            if (actor.StatusEffects.ContainsKey(kvp.Key))
            {
                kvp.Value.enabled = true;
                kvp.Value.transform.position = buffLocations;
                kvp.Value.sortingLayerName = "Foreground";
                buffLocations.x += 0.5f;
            }
            else
            {
                kvp.Value.enabled = false;
                kvp.Value.sortingLayerName = "Idle";
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