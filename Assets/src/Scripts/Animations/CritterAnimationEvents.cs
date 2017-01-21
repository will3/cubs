using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Director;

namespace AssemblyCSharp
{
	public class CritterAnimationEvents : StateMachineBehaviour {
		private bool _exitAttack = false;
		private bool _finishedAttack = false;
		private bool _idle = false;
		private bool _walking = false;
		private bool _attacking = false;

		public AnimationInfo animationInfo;

		// Return true if exiting attack
		public bool exitAttack {
			get { return _exitAttack; }
		}

		// Return true if attacking but attack has finished, see AnimationInfo.attackFinishTime
		public bool finishedAttack {
			get { return _finishedAttack; }
		}

		public bool idle { get { return _idle; } }
		public bool walking { get { return _walking; } }
		public bool attacking { get { return _attacking; } }

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
		{
			_exitAttack = false;
			_finishedAttack = false;

			_idle = stateInfo.IsName("idle");
			_walking = stateInfo.IsName ("walking");
			_attacking = stateInfo.IsName ("attacking");
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