using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterAnimationEvents : StateMachineBehaviour {
	public interface Listener
	{
		void DidExitAttack ();
		void DidEnterAny ();
	}

	public Listener listener;

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		var finishedAttack = stateInfo.IsName ("attack");
		if (finishedAttack) {
			listener.DidExitAttack ();
		}
	}

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		listener.DidEnterAny ();
	}
}
