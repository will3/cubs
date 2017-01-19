using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Director;

namespace AssemblyCSharp
{
	public class CritterAnimationEvents : StateMachineBehaviour {
		private bool _exitAttack = false;
		private bool _finishedAttack = false;

		public AnimationInfo animationInfo;

		// Return true if exiting attack
		public bool exitAttack {
			get { return _exitAttack; }
		}

		// Return true if attacking but attack has finished, see AnimationInfo.attackFinishTime
		public bool finishedAttack {
			get { return _finishedAttack; }
		}

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
		{
			_exitAttack = false;
			_finishedAttack = false;
		}

		override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) {
			_finishedAttack = false;
			if (stateInfo.IsName ("attack") &&
				stateInfo.normalizedTime > animationInfo.attackFinishTime) {
				_finishedAttack = true;
			}
		}

		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.IsName ("attack")) {
				_exitAttack = true;
			}
		}
	}
}