using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	
    public float moveSpeed;
	public LayerMask collideablesLayer;
	
	private bool isMoving;
	private Vector2 input;
	
	private Animator animator;
	
	private float fixedDeltaTime;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		this.fixedDeltaTime = Time.fixedDeltaTime;
	}
	
	private void Update()
	{
		if (!isMoving)
		{
			input.x = Input.GetAxisRaw("Horizontal");
			input.y = Input.GetAxisRaw("Vertical");
			
			//yeetus diagonalus; no diagonals cause diagonals are big gae
			if (input.x != 0) input.y = 0;
			
			//makes player move by x or y amount
			if (input != Vector2.zero)
			{
				animator.SetFloat("moveX", input.x);
				animator.SetFloat("moveY", input.y);
		
				var targetPos = transform.position;
				targetPos.x += input.x;
				targetPos.y += input.y;
				
				if (IsWalkable(targetPos))
				{
					StartCoroutine(Move(targetPos));
				}
			}
		}
		animator.SetBool("isMoving", isMoving);

		if (Input.GetKeyDown("space"))
        {
            if (Time.timeScale == 1.0f)
                Time.timeScale = 5.0f;
            else
                Time.timeScale = 1.0f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
	}
	
	//Coroutine
	IEnumerator Move(Vector3 targetPos)
	{
		isMoving = true;
		
		//basically checks for difference from "future" / target position from current position
		while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
		{
			//moves to x / y position with this move speed in a short amount of time
			transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
			yield return null;
		}
		transform.position = targetPos;
		
		isMoving = false;
	}
	
	private bool IsWalkable(Vector3 targetPos)
	{
		if (Physics2D.OverlapCircle(targetPos, 0.3f, collideablesLayer) != null)
		{
			return false;
		}
		
		return true;
	}
}
