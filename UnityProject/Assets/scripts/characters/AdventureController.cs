using UnityEngine;
using System.Collections;

public class AdventureController : MonoBehaviour {

	public WalkingArea walkingarea;
	public SmartObject target = null;
	public float walkspeed = 1.0f;
	public Animator animator;
	private bool facingLeft = true;

	// Use this for initialization
	void Start () {
		walkingarea = FindObjectOfType<WalkingArea>();
		animator = GetComponent<Animator>();
	}

	void targetReached() {
		target.touched();
		target = null;
		animator.SetBool("isWalking", false);
		animator.SetTrigger("grabObject");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target != null) {
			// 1) our bottom left-bottom right collider co-ordinates must be bound within the walking area collider
			// 2) our collider must try and touch the target collider box
			// if we reach our target, let it know and set our target to null

			if (collider2D.bounds.Intersects(target.collider2D.bounds)) {
				// we have reached the target
				targetReached();
			}
			else {
				float translationdistance = walkspeed * Time.deltaTime;
				// shift towards the target and play a frame of the walk cycle

				Vector2 translationvector = target.collider2D.bounds.center - collider2D.bounds.center;

				// bound the y diff to make sure we don't run away from the walkingarea

				float curMinY = collider2D.bounds.center.y - collider2D.bounds.extents.y;
				float newMinY = curMinY + translationvector.y;

				float minY = walkingarea.collider2D.bounds.center.y - walkingarea.collider2D.bounds.extents.y;
				float maxY = walkingarea.collider2D.bounds.center.y + walkingarea.collider2D.bounds.extents.y;

				if (newMinY < minY) {
					float diff = newMinY - minY;
					translationvector.y -= diff;
				}
				else if (newMinY > maxY) {
					float diff = maxY - newMinY;
					translationvector.y += diff;
				}

				transform.Translate(translationvector.normalized * translationdistance);

				if (translationvector.x > 0) {
					if (facingLeft != false) {
						facingLeft = false;
						Vector3 localscale = transform.localScale;
						localscale.x *= -1;
						transform.localScale = localscale;
					}
				}
				else {
					if (facingLeft != true) {
						facingLeft = true;
						Vector3 localscale = transform.localScale;
						localscale.x *= -1;
						transform.localScale = localscale;
					}
				}

				// check for reaching the target via x only
				float curX = collider2D.bounds.center.x;
				float minX = target.collider2D.bounds.center.x - target.collider2D.bounds.extents.x;
				float maxX = target.collider2D.bounds.center.x + target.collider2D.bounds.extents.x;

				if (minX < curX && curX < maxX) {
					targetReached();
				}
				else {
					animator.SetBool("isWalking", true);
				}
			}
		}
	}
}
