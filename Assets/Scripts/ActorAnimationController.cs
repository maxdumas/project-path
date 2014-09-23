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
}
