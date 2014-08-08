//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Text;
using UnityEngine;
using System.Collections;
public class CombatWindow : MonoBehaviour
{
    public Player player;
    public Monster MonsterPrefab;
	public SpriteRenderer PlayerSpritePrefab;
	public SpriteRenderer BackgroundPrefab;

    private SpriteRenderer MonsterSprite;
	private SpriteRenderer PlayerSprite;
    private SpriteRenderer Background;
	private Monster enemy;

	private Animator PlayerAnimator;
	private Animator MonsterAnimator;

	private float PlayerCombatSpeed = 1.0f;
	private float NextPlayerCombat = 0.0f;
	private float PlayerPause = 0.1f;

	private float MonsterCombatSpeed = 1.0f;
	private float NextMonsterCombat = 0.0f;
	private float MonsterPause = 1.0f;

	private int MonsterNumAttacks = 1;
	private int MonsterCurrAttacks = 0;

	private bool IsPlayerDefending = false;
	private bool IsMonsterDefending = false;


	void Update()
	{

		if (player.Health <= 0) {
			Debug.Log("You die!");
			DestroyWindow();

		} else if (enemy.Health <= 0) {
			Debug.Log("Enemy dies!");
			DestroyWindow();
		}


		if(Input.GetKey("up") && Time.time > NextPlayerCombat) 
		{
			NextPlayerCombat = Time.time + PlayerCombatSpeed + PlayerPause;
			PlayerAnimator.SetBool("attacking",true);
			IsPlayerDefending = false;

			if (!IsMonsterDefending)
			{
				Debug.Log("Hit for 1! Enemy Health: " + (enemy.Health-1));
				enemy.Health -= player.GetAttackValue();
			} 
			else
			{
				Debug.Log("Monster blocks your attack!");
			}
		}
		else if (Input.GetKey("down") && Time.time > NextPlayerCombat)
		{
			NextPlayerCombat = Time.time + PlayerCombatSpeed + PlayerPause;
			PlayerAnimator.SetBool("defending",true);
			IsPlayerDefending = true;
		} 
		else if (Time.time > NextPlayerCombat - PlayerPause) 
		{
			PlayerAnimator.SetBool("attacking",false);
			PlayerAnimator.SetBool("defending",false);
			IsPlayerDefending = false;
		}

		if (MonsterCurrAttacks < MonsterNumAttacks && Time.time > NextMonsterCombat)
		{
			NextMonsterCombat = Time.time + MonsterCombatSpeed + MonsterPause;
			MonsterAnimator.SetBool("attacking",true);
			IsMonsterDefending = false;
			MonsterCurrAttacks += 1;

			if (!IsPlayerDefending) 
			{
				Debug.Log("Take damage for 1! Your Health: " + player.Health);
				player.Health -= enemy.GetAttackValue();
			}
			else
			{
				Debug.Log("You block monster's attack!");
			}
		}
		else if (MonsterCurrAttacks >= MonsterNumAttacks && Time.time > NextMonsterCombat)
		{
			NextMonsterCombat = Time.time + MonsterCombatSpeed + MonsterPause;
			MonsterAnimator.SetBool("defending",true);
			MonsterCurrAttacks = 0;
			IsMonsterDefending = true;
		}
		else if (Time.time > NextMonsterCombat - MonsterPause)
		{
			MonsterAnimator.SetBool("attacking",false);
			MonsterAnimator.SetBool("defending",false);
			IsMonsterDefending = false;
		}

	}

    public void Enable()
    {
		enemy = (Monster)Instantiate(MonsterPrefab, transform.position, Quaternion.identity);
		Background = (SpriteRenderer)Instantiate(BackgroundPrefab, transform.position, Quaternion.identity);
		PlayerSprite = (SpriteRenderer)Instantiate(PlayerSpritePrefab, transform.position, Quaternion.identity);
		MonsterSprite = (SpriteRenderer)Instantiate(MonsterPrefab.GetComponent<SpriteRenderer>(),transform.position,Quaternion.identity);
		
		//PlayerCombatRate = player.CombatSpeed;
		//MonsterCombatRate = MonsterPrefab.CombatSpeed;
		
		Background.enabled = false;
		PlayerSprite.enabled = false;
		MonsterSprite.enabled = false;
		
		MonsterNumAttacks = enemy.NumAttacks;
		
		PlayerAnimator = PlayerSprite.GetComponent<Animator>();
		MonsterAnimator = MonsterSprite.GetComponent<Animator>();

		
		Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        Background.transform.position = pos;
        Background.enabled = true;
        Background.sortingLayerName = "Background";

        //Foreground.transform.position = pos;
        //Foreground.enabled = true;
        //Foreground.sortingLayerName = "Foreground";

        PlayerSprite.transform.position = new Vector3(player.transform.position.x - 2.6f, player.transform.position.y-1.1f, player.transform.position.z);
        PlayerSprite.enabled = true; 
        PlayerSprite.sortingLayerName = "Midground";

        MonsterSprite.transform.position = new Vector3(player.transform.position.x + 2.6f, player.transform.position.y - 1.1f, player.transform.position.z);
        MonsterSprite.enabled = true;
        MonsterSprite.sortingLayerName = "Midground";
    }

	public void DestroyWindow()
	{

		Destroy(enemy);
		Destroy(PlayerSprite);
		Destroy(MonsterSprite);
		Destroy(Background);
		Destroy(this);
	}

    //public void 
}
