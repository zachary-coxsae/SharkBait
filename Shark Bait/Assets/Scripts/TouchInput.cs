using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handle screen touches
// Mouse clicks emulate touches

public class TouchInput : MonoBehaviour
{
	private Vector2 startPosition;      // screen
	private Vector2 lastPosition;       // screen - last frame
	private Vector2 moveDirection;      // screen
	private Vector2 endPosition;        // screen

	private long touchStartTicks;
	private long touchEndTicks;
	private float TickSpeedFactor = 100000f;        // to reduce swipe time

	private float swipeThreshold = 20f;     // anything less not considered a valid swipe (ie. a tap or accidental swipe)


	private void Update()
	{
		
		// track a single touch
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			// handle finger movements based on TouchPhase
			switch (touch.phase)
			{
				case TouchPhase.Began:
					StartTouch(touch.position, Input.touchCount);
					break;

				case TouchPhase.Moved:
					MoveTouch(touch.position, Input.touchCount);
					break;

				case TouchPhase.Ended:
					EndTouch(touch.position, Input.touchCount);
					break;

				case TouchPhase.Stationary:         // touching but hasn't moved
					break;

				case TouchPhase.Canceled:           // system cancelled touch
					break;
			}
		}

		// emulate touch with left mouse
		else if (Input.GetMouseButtonDown(0))
		{
			StartTouch(Input.mousePosition, 1);
		}
		else if (Input.GetMouseButton(0))
		{
			MoveTouch(Input.mousePosition, 1);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			EndTouch(Input.mousePosition, 1);
		}

		// right mouse == 2 finger swipe
		else if (Input.GetMouseButtonDown(1))
		{
			StartTouch(Input.mousePosition, 2);
		}
		else if (Input.GetMouseButton(1))
		{
			MoveTouch(Input.mousePosition, 2);
		}
		else if (Input.GetMouseButtonUp(1))
		{
			EndTouch(Input.mousePosition, 2);
		}
	}

	private void StartTouch(Vector2 screenPosition, int touchCount)
	{
		startPosition = screenPosition;
		touchStartTicks = DateTime.Now.Ticks;

		lastPosition = startPosition;

		GameEvents.OnSwipeStart?.Invoke(startPosition, touchCount);
	}

	private void MoveTouch(Vector2 screenPosition, int touchCount)
	{

		Vector2 movePosition = screenPosition;

		if (movePosition != lastPosition)
		{
			moveDirection = (movePosition - lastPosition).normalized;

			float moveDistance = Mathf.Abs(Vector2.Distance(lastPosition, movePosition));
			float moveSpeed = moveDistance / Time.deltaTime;     // speed = distance / time

			GameEvents.OnSwipeMove?.Invoke(movePosition, moveDirection, moveSpeed, touchCount);

			lastPosition = movePosition;
		}
		else
			GameEvents.OnSwipeMove?.Invoke(movePosition, Vector2.zero, 0f, touchCount);
	}

	private void EndTouch(Vector2 screenPosition, int touchCount)
	{

		endPosition = screenPosition;
		touchEndTicks = DateTime.Now.Ticks;
		Vector2 moveDirection = (endPosition - startPosition).normalized;

		float moveDistance = Mathf.Abs(Vector2.Distance(startPosition, endPosition));
		float moveSpeed = 0;     // distance / time
		float touchTime = touchEndTicks - touchStartTicks;

		if (touchTime > 0)            // should be!!
		{
			float swipeTime = touchTime / TickSpeedFactor;
			moveSpeed = moveDistance / swipeTime;     // speed = distance / time
		}

		if (moveSpeed > 0)
		{
			GameEvents.OnSwipeEnd?.Invoke(endPosition, moveDirection, moveSpeed, touchCount);
		}
	}
}