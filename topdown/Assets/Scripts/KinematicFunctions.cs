using System.Collections;
using UnityEngine;

public class KinematicFunctions
{
    //lerps object from one place to another
    public static IEnumerator MoveObject(Transform obj, Vector3 Origin, Vector3 Destination, float totalMovementTime)
    {
        float currentMovementTime = 0f;//The amount of time that has passed
        while (Vector3.Distance(obj.localPosition, Destination) > 0)
        {
            currentMovementTime += Time.deltaTime;
            obj.localPosition = Vector3.Lerp(Origin, Destination, currentMovementTime / totalMovementTime);
            yield return null;
        }
    }
}
