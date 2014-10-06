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
    public SpriteRenderer BackgroundPrefab;
    public SpriteRenderer FramePrefab;
    public TextAsset MonsterPattern;

    private Monster _monster;
    private SpriteRenderer _background;
    private SpriteRenderer _frame;

    private MoveContainer[] _monsterMoves;
    private int _monsterMoveIndex;
    private float _lastMonsterActionTime;

    private readonly Dictionary<Actor, CwActorInfo> _cwInfo = new Dictionary<Actor, CwActorInfo>(2);

    [Serializable]
    private class CwActorInfo
    {
        public MoveType CurrentMove = MoveType.Idle;
        public Vector3 Location;
        public Vector3 DamageLocation;
        public readonly Dictionary<string, SpriteRenderer> StatusEffectIcons = new Dictionary<string, SpriteRenderer>();
    }

    private void Update()
    {
        if (Player.Health <= 0)
        {
            Debug.Log("You die!");
            DestroyWindow();
        }
        if (_monster.Health <= 0)
        {
            Debug.Log("Monster dies!");
            DestroyWindow();
        }

        ShowStatusEffects(Player);
        ShowStatusEffects(_monster);
        PlayerInput();
        MonsterInput();
    }

        if (_cwInfo[Player].CurrentMove == MoveType.Idle)
        { // We only want the player to be able to perform moves from the idle position

#if UNITY_STANDALONE
            if (Input.GetKey("up"))
#endif
#if UNITY_ANDROID || UNITY_IPHONE
            if(Input.touchCount > 0 && Input.GetTouch(0).deltaPosition.y > 0 )
#endif
            {
                Attack(Player);
            }
#if UNITY_STANDALONE
            else if (Input.GetKey("down"))
#endif
#if UNITY_ANDROID || UNITY_IPHONE
            else if(Input.touchCount > 0 && Input.GetTouch(0).deltaPosition.y < 0 )
#endif
            {
                Defend(Player);
            }
        }
        else if (Player.CombatAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") && _cwInfo[Player].CurrentMove != MoveType.Idle)
        {
            Idle(Player);
        }
    }

        if (Time.time > _lastMonsterActionTime + _monsterMoves[_monsterMoveIndex].Delay)
        { // The monster performs the next action in its pattern whenever the delay for that move is exceeded
            _lastMonsterActionTime = Time.time;
            switch (_monsterMoves[_monsterMoveIndex].MoveType)
            {
                case MoveType.Attack:
                    Attack(_monster);
                    break;
                case MoveType.Defend:
                    Defend(_monster);
                    break;
            }
            _monsterMoveIndex = (_monsterMoveIndex + 1)%_monsterMoves.Length;
        }
        else if (_monster.CombatAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") && _cwInfo[_monster].CurrentMove != MoveType.Idle)
        { // We check for _monsterState != MoveType.Idle because this should only happen exactly when the monster becomes idle
            Idle(_monster);
        }
    }

    private void Idle(Actor actor)
    {
        _cwInfo[actor].CurrentMove = MoveType.Idle;
        actor.CombatAnimator.SetInteger("State", 0);
    }

    private void Defend(Actor actor)
    {
        actor.CombatAnimator.SetInteger("State", -1);
    }

    private void Attack(Actor actor)
    {
        _cwInfo[actor].CurrentMove = MoveType.Attack;
        actor.CombatAnimator.SetInteger("State", 1);
    }

    public void OnAnimAttack(Actor attacker)
    {
        if (attacker == Player)
            HandleDamage(attacker, _monster);
        else if (attacker == _monster)
            HandleDamage(attacker, Player);
    }

    public void OnAnimDefenseBegin(Actor defender)
    {
        _cwInfo[defender].CurrentMove = MoveType.Defend;
    }

    public void OnAnimDefenseEnd(Actor defender)
    {
        _cwInfo[defender].CurrentMove = MoveType.Defend;
    }

    public void OnAnimEndHit(Actor actor)
    {
    }

    private void HandleDamage(Actor attacker, Actor defender) 
    {
        if (_cwInfo[defender].CurrentMove != MoveType.Defend)
        {
            float roll = Random.Range(0f, 1f);

            if (attacker.Accuracy / defender.Evasion > roll)
            {
                int damage = 0;
                if (!defender.CwInfo.Defending)
                {
                    Debug.Log("Stump Armor Value = " + defender.GetArmorValue());
                    Debug.Log("Player ATtack Value = " + attacker.GetAttackValue());
                    damage = attacker.GetAttackValue() - defender.GetArmorValue();
                    defender.CwInfo.Animator.SetInteger("State", -2);
                }
                else
                {
                    damage = attacker.GetAttackValue() - defender.GetDefenseValue();
                }

                defender.Health -= damage;
                defender.CombatAnimator.SetInteger("State", -2);

                Debug.Log(string.Format("{1} is hit for {0}! {1} Health: {2}", damage, defender.DisplayName, _monster.Health));
                DisplayMessage(damage.ToString(), Color.red, _cwInfo[defender].DamageLocation);

                // Poison Chance
                //
                // Each attack, the actor has a chance to poison IF the actor IsPoisonous. 
                // If the other actor is already poisoned and it procs, the fade damage value is refreshed.
                roll = Random.Range(0f, 1f);
                if (attacker.IsPoisonous && attacker.PoisonChance > roll)
                {
                    Debug.Log(string.Format("{0} is poisoned for {1} damage.", defender.DisplayName,
                        attacker.PoisonDamageValue));
                    defender.AddStatusEffect(new PoisonStatusEffect(attacker.PoisonDamageValue, attacker.PoisonTickSpeed));
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
                    defender.AddStatusEffect(new BlindStatusEffect(attacker.BlindAttackLength));
                }
            }
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
        InitActor(Player, -(halfCamWidth * 0.6f), -(halfCamHeight * 0.75f));
       // Player.CwInfo.Animator.speed = 2;
        //MonsterSprite
        _monster = (Monster)Instantiate(MonsterPrefab, transform.position, Quaternion.identity);
        _monster.transform.parent = this.transform;
        InitActor(_monster, +(halfCamWidth * 0.6f), -(halfCamHeight * 0.75f));
        //_monster.CwInfo.Animator.speed = 2;

        string[] lines = MonsterPattern.text.Split('\n');
        var parsedMoves = new List<MoveContainer>(lines.Length);
        foreach (string line in lines)
        {
            if (line == "" || line[0] == '#') continue;
            string[] tokens = line.Split(';');
            float delay = float.Parse(tokens[0]);
            MoveType moveType = (MoveType)Enum.Parse(typeof(MoveType), tokens[1], true);
            parsedMoves.Add(new MoveContainer {Delay = delay, MoveType = moveType});
            Debug.Log(moveType + " " + delay);
        }

        _monsterMoves = parsedMoves.ToArray();

        // Player / Monster Buffs
        Debug.Log("Player Accuracy / Evasion: " + Player.Accuracy + " " + Player.Evasion);
        Debug.Log("Monster Accuracy / Evasion: " + _monster.Accuracy + " " + _monster.Evasion);

        //Player.CwInfo.Animator.speed = 2;
    }

    private void InitActor(Actor actor, float x, float y)
    {
        float xPos = actor.transform.position.x + x;
        float yPos = actor.transform.position.y + y;
        
        _cwInfo.Add(actor, new CwActorInfo());
        actor.CombatSprite.transform.parent = this.transform;
        actor.CombatSprite.transform.position = new Vector3(xPos, yPos, 0);
        actor.CombatSprite.sortingLayerName = "Midground";
        actor.CombatSprite.enabled = true;
        _cwInfo[actor].Location = actor.CombatSprite.transform.position;
        _cwInfo[actor].DamageLocation = new Vector3(xPos, yPos + 0.5f, 0);
        actor.CombatAnimator = actor.CombatSprite.GetComponent<Animator>();

        var animController = actor.CombatSprite.GetComponent<ActorAnimationController>();
        animController.TargetActor = actor;
        animController.TargetCombatWindow = this;
        
        Sprite[] seTextures = Resources.LoadAll<Sprite>("StatusEffects");
        foreach (var sprite in seTextures)
        {
            var g = new GameObject(sprite.name);
            g.transform.parent = transform;
            var s = g.AddComponent<SpriteRenderer>();
            s.sprite = sprite;
            _cwInfo[actor].StatusEffectIcons.Add(sprite.name, s);
        }
    }

    public void DestroyWindow()
    {
        Player.CombatSprite.gameObject.SetActive(false);
        Destroy(_monster);
        _monster.CombatSprite.gameObject.SetActive(false);
        Destroy(_background);
        Destroy(_frame);

        foreach (var info in _cwInfo.Values)
        {
            foreach (var b in info.StatusEffectIcons.Values)
                Destroy(b);
        }

        Destroy(gameObject);
    }

    private void ShowStatusEffects(Actor actor)
    {
        if (actor.StatusEffects.Count == 0 || _cwInfo[actor].StatusEffectIcons.Count == 0) return;

        Vector3 buffLocations = new Vector3(_cwInfo[actor].Location.x - 0.3f, _cwInfo[actor].Location.y + 3.5f, 0);

        foreach (var icon in _cwInfo[actor].StatusEffectIcons.Values)
            icon.enabled = false;

        foreach (IActorStatusEffect effect in actor.StatusEffects)
        {
            var icon = _cwInfo[actor].StatusEffectIcons[effect.Name];
            icon.enabled = true;
            icon.transform.position = buffLocations;
            icon.sortingLayerName = "Foreground";
            buffLocations.x += 0.5f;
        }
    }

    protected void DisplayMessage(string text, Color color, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f)
    {
        Vector3 offset = new Vector3(xOffset, yOffset, 2);

        DisplayMessage(text, color, offset);
    }

    protected void DisplayMessage(string text, Color color, Vector3 location)
    {
        TextMesh message = (TextMesh)Instantiate(EventNotifier, location, Quaternion.identity);
        message.color = color;
        message.text = text;
    }
}