using UnityEngine;
using System.Collections;

public class ActorAnimationController : MonoBehaviour
{
    public Actor TargetActor;
    public CombatWindow TargetCombatWindow;

    public void AnimAttack()
    {
        TargetCombatWindow.OnAnimAttack(TargetActor);
    }

    public void AnimEndAttack()
    {

        TargetCombatWindow.OnAnimAttackEnd(TargetActor);
    }

    public void AnimBeginDefense()
    {
        TargetCombatWindow.OnAnimDefenseBegin(TargetActor);
    }

    public void AnimEndDefense()
    {
        TargetCombatWindow.OnAnimDefenseEnd(TargetActor);
    }

    public void AnimBeginIdle()
    {
        
    }

    public void AnimEndIdle()
    {
        
    }

    public void AnimBeginHit()
    {
        
    }

    public void AnimEndHit()
    {
        TargetCombatWindow.OnAnimEndHit(TargetActor);
    }


    public void AnimEndDeath()
    {
        TargetCombatWindow.OnAnimEndDeath(TargetActor);
    }
}
