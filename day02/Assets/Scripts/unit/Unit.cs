using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public enum unitDirection
	{
		UP,
		UPLEFT,
		LEFT,
		DOWNLEFT,
		DOWN,
		DOWNRIGHT,
		RIGHT,
		UPRIGHT,
		STAY
	};

	public unitDirection direction;

	public float speed;

	public bool selected { get; set; }
	public GameObject selectPref;
	private GameObject selectGo;

	public unitSound sound;

	public GameObject target;
	public float range;
	public float damage;

	private Vector3 destination;
	private Vector3 nextPos;
	private Animator anim;
	private bool attackState;

	private float deltaX;
	private float deltaY;
	private float time;
	[HideInInspector]
	public float attackSpeed;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		direction = unitDirection.STAY;
		destination = transform.position;
		nextPos = transform.position;
		selectGo = null;
		speed /= 500;
		attackState = false;
		time = 0;
		sound = GetComponent<unitSound> ();
	}

	public void moveTo(Vector3 mousePos)
	{
		mousePos.z = 10.0f;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		destination = mousePos;
	}

	public void setDirection()
	{
		deltaX = nextPos.x - transform.localPosition.x;
		deltaY = nextPos.y - transform.localPosition.y;
		deltaX *= 300;
		deltaY *= 300;
		if (deltaX > 1 && deltaY > 1)
			direction = unitDirection.UPRIGHT;
		else if (deltaX > 1 && deltaY < -1)
			direction = unitDirection.DOWNRIGHT;
		else if (deltaX < -1 && deltaY > 1)
			direction = unitDirection.UPLEFT;
		else if (deltaX < -1 && deltaY < -1)
			direction = unitDirection.DOWNLEFT;
		else if (deltaX > 1)
			direction = unitDirection.RIGHT;
		else if (deltaX  < -1)
			direction = unitDirection.LEFT;
		else if (deltaY > 1)
			direction = unitDirection.UP;
		else if (deltaY < -1)
			direction = unitDirection.DOWN;
	}

	public void setAtckDirection()
	{
		deltaX = nextPos.x - transform.localPosition.x;
		deltaY = nextPos.y - transform.localPosition.y;
		deltaX *= 500;
		deltaY *= 500;
		if (deltaX > 1 && deltaY > 1)
			direction = unitDirection.UPRIGHT;
		else if (deltaX > 1 && deltaY < -1)
			direction = unitDirection.DOWNRIGHT;
		else if (deltaX < -1 && deltaY > 1)
			direction = unitDirection.UPLEFT;
		else if (deltaX < -1 && deltaY < -1)
			direction = unitDirection.DOWNLEFT;
		else if (deltaX > 1)
			direction = unitDirection.RIGHT;
		else if (deltaX  < -1)
			direction = unitDirection.LEFT;
		else if (deltaY > 1)
			direction = unitDirection.UP;
		else if (deltaY < -1)
			direction = unitDirection.DOWN;
	}

	public void playAtkanim()
	{
		if (direction == unitDirection.UP) {
			anim.SetInteger ("attack", 1);
		} else if (direction == unitDirection.UPRIGHT) {
			anim.SetInteger ("attack", 2);
		} else if (direction == unitDirection.RIGHT) {
			anim.SetInteger ("attack", 3);
		} else if (direction == unitDirection.DOWNRIGHT) {
			anim.SetInteger ("attack", 4);
		} else if (direction == unitDirection.DOWN) {
			anim.SetInteger ("attack", 5);
		} else if (direction == unitDirection.UPLEFT) {
			anim.SetInteger ("attack", 2);
			transform.localRotation = new Quaternion (0, 180, 0, 0);
		} else if (direction == unitDirection.LEFT) {
			anim.SetInteger ("attack", 3);
			transform.localRotation = new Quaternion (0, 180, 0, 0);
		} else if (direction == unitDirection.DOWNLEFT) {
			anim.SetInteger ("attack", 4);
			transform.localRotation = new Quaternion (0, 180, 0, 0);
		}
	}

	public void playMoveanim()
	{
		if (direction == unitDirection.UP) {
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			anim.SetInteger ("walking", 1);
		} else if (direction == unitDirection.UPRIGHT) {
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			anim.SetInteger ("walking", 2);
		} else if (direction == unitDirection.RIGHT) {
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			anim.SetInteger ("walking", 3);
		} else if (direction == unitDirection.DOWNRIGHT) {
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			anim.SetInteger ("walking", 4);
		} else if (direction == unitDirection.DOWN) {
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			anim.SetInteger ("walking", 5);
		} else if (direction == unitDirection.UPLEFT) {
			transform.localRotation = new Quaternion (0, 0, 0, 0);
			anim.SetInteger ("walking", 2);
			transform.localRotation = new Quaternion (0, 180, 0, 0);
		} else if (direction == unitDirection.LEFT) {
			anim.SetInteger ("walking", 3);
			transform.localRotation = new Quaternion (0, 180, 0, 0);
		} else if (direction == unitDirection.DOWNLEFT) {
			anim.SetInteger ("walking", 4);
			transform.localRotation = new Quaternion (0, 180, 0, 0);
		} else if (direction == unitDirection.STAY) {
			anim.SetInteger ("walking", 0);
		}
	}
	// Update is called once per frame
	void Update () {
		if (selected && selectGo == null) {
			selectGo = GameObject.Instantiate (selectPref, transform.localPosition, Quaternion.identity) as GameObject;
			selectGo.transform.parent = this.transform;
		} else if (!selected) {
			if (selectGo != null) {
				GameObject.Destroy (selectGo);
				selectGo = null;
			}
		}
		if (target == null && attackState) {
			attackState = false;
			anim.SetInteger ("attack", 0);
		}
		if (target != null && Vector3.Distance (transform.localPosition, target.transform.localPosition) > range) {
			destination = target.transform.localPosition;
		} else if (target != null) {
			destination = transform.position;
			setAtckDirection ();
			playAtkanim ();
			attackState = true;
		}

		if (Vector3.Distance (transform.localPosition, destination) > 0.1f) {
			nextPos = Vector3.MoveTowards (transform.localPosition, destination, speed);
		} else
			direction = unitDirection.STAY;
		if (target == null || Vector3.Distance (transform.localPosition, target.transform.localPosition) > range)
			setDirection ();
		playMoveanim ();
		transform.localPosition = nextPos;
		if (attackSpeed > 0 && target != null)
			hitTarget();
	}
	public void hitTarget()
	{
		if (time == 0)
			time = Time.time;
		if (time + attackSpeed < Time.time) {
			sound.playSong ("attackSound");
			target.GetComponent<Health> ().inflige (this.damage);
			if (target.GetComponent<Health> ().isDead ()) {
				target.GetComponent<Health> ().kill ();
				target = null;
			}
			time = Time.time;
		}
	}
}
