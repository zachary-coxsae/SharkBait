using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static Action<Vector2, int> OnSwipeStart; //startPosition, touchCount
    public static Action<Vector2, Vector2, float ,int> OnSwipeMove; //movePosition, moveDirection, moveSpeed, touchCount
    public static Action<Vector2, Vector2, float, int> OnSwipeEnd; //endPosition, moveSpeed, touchCount
    public static Action<RaycastHit> OnFishHit; //hitinfo
}
