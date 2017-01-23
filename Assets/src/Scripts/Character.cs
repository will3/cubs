using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyCSharp
{
	[Serializable]
	public class AnimationInfo {
		public float attackSpeed = 1.0f;

		public float walkSpeed = 1.0f;

		public float idleSpeed = 1.0f;

		public float attackFinishTime = 0.5f;

		public Vector2 attackExitLocation = new Vector2(0.0f, 1.0f);

		public Vector2 centerLocation = new Vector2(0.0f, 0.5f);
	}

	public class Character : MonoBehaviour, IBlock, IDamagable {

		public string characterName = "<unnamed>";

		public float idleLength = 0.1f;

		public int patrolDis = 10;

		public float speed = 0.04f;

		public float hitPoints = 100.0f;

		public bool isMonster = false;

		public int maxPathFindingSteps = 32;

		public Damage damage = new Damage();

		public float vision = 5.0f;

		public BehaviourType behaviourType;

		public Vector3 moveDirection;

		public AnimationInfo animationInfo = new AnimationInfo();

		private Billboard billboard;
	
		#region IBlock implementation

		private BlockCoord _blockCoord = new BlockCoord();

		public BlockCoord blockCoord {
			get {
				return _blockCoord;
			}
		}

		#endregion

		public void Start() {
			damage.sourceName = characterName;

			var animator = GetComponentInChildren<Animator> ();
			animator.SetAttackSpeed (animationInfo.attackSpeed);
			animator.SetWalkSpeed (animationInfo.walkSpeed);
			animator.SetIdleSpeed (animationInfo.idleSpeed);

			billboard = GetComponentInChildren<Billboard> ();

			Characters.Instance.Add (this);
		}

		public void OnDestroy() {
			Characters.Instance.Remove (this);
		}

		public void Update() {
			if (hitPoints <= 0.0f) {
				Destroy (gameObject);
			}
		}

		public bool Dead {
			get {
				return hitPoints <= 0.0f;
			}
		}

		public Vector3 CalcAttackExitPoint() {
			var attackExitLocation = animationInfo.attackExitLocation;
			return gameObject.transform.position + billboard.transform.TransformDirection (attackExitLocation);
		}

		public Vector3 CalcCenterPoint() {
			var location = animationInfo.centerLocation;
			return gameObject.transform.position + billboard.transform.TransformDirection (location);
		}

		#region IDamagable implementation

		public void ApplyDamage (Damage damage)
		{
			hitPoints -= damage.amount;

			Debug.LogFormat ("{0} attacked {1} for {2} damage", this.characterName, damage.sourceName, damage.amount);
		}

		#endregion
	}

	public class CharacterSearch {
		public float maxDistance = 0;
		public Character character;
	}

	public class Characters {
		private Characters() {}

		private static Characters _instance;
		public static Characters Instance {
			get {
				if (_instance == null) {
					_instance = new Characters ();
				}
				return _instance;
			}
		}

		private Dictionary<int, Character> characters = new Dictionary<int, Character>();

		public void Add(Character character) {
			characters[character.GetInstanceID()] = character;
		}

		public void Remove(Character character) {
			characters.Remove (character.GetInstanceID ());
		}

		// Search for characters, returns in order of distance
		public IEnumerable<Character> GetTargets(CharacterSearch search) {
			var maxDisSq = search.maxDistance * search.maxDistance;

			return characters.Values.Where (u => {
				if (u == search.character) {
					return false;
				}

				var disSq = (u.gameObject.transform.position - search.character.transform.position).sqrMagnitude;
				if (disSq > maxDisSq) {
					return false;
				}

				return u.isMonster != search.character.isMonster;
			}).OrderBy (u => (u.gameObject.transform.position - search.character.transform.position).sqrMagnitude);
		}
	}
}
