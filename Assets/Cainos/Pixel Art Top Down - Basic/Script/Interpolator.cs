using System.Collections.Generic;
using UnityEngine;

public class Interpolator : MonoBehaviour
{
    [SerializeField] private float timeElapsed = 0f;
    [SerializeField] private float timeToReachTarget = 0.05f;
    [SerializeField] private float movementThreshold = 0.05f;

    private List<TransformUpdate> futureTransformUpdates = new List<TransformUpdate>();

    private float squareMovementThreshold;
    private TransformUpdate to;
    private TransformUpdate from;
    private TransformUpdate previous;

    private void Start()
    {
        squareMovementThreshold = movementThreshold * movementThreshold;
        to = new TransformUpdate(NetworkManager.Singleton.ServerTick, transform.position);
        from = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, transform.position);
        previous = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, transform.position);
    }

    private void Update()
    {
        for (int i = 0; i < futureTransformUpdates.Count; i++)
        {
           // Debug.Log("ftfu count " + futureTransformUpdates.Count);
            if (NetworkManager.Singleton.ServerTick >= futureTransformUpdates[i].Tick)
            {
                previous = to;
                to = futureTransformUpdates[i];
                Debug.Log(NetworkManager.Singleton.InterpolationTick +"TICKKKKK");
                from = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, transform.position);

                futureTransformUpdates.RemoveAt(i);
                i--;
                timeElapsed = 0f;
                timeToReachTarget = (to.Tick - from.Tick) * Time.fixedDeltaTime;
            }
         

        }

        timeElapsed += Time.deltaTime;
        if (timeToReachTarget != 0)
        {
            float lerpAmount = timeElapsed / timeToReachTarget;
            InterpolatePosition(lerpAmount);
        }
    }

    private void InterpolatePosition(float lerpAmount)
    {
        if (!float.IsNaN(lerpAmount) && !float.IsInfinity(lerpAmount))
        {
            if ((to.Position - previous.Position).sqrMagnitude < squareMovementThreshold)
            {
                if (to.Position != from.Position)
                    transform.position = Vector2.Lerp(from.Position, to.Position, lerpAmount);

                return;
            }

            transform.position = Vector2.LerpUnclamped(from.Position, to.Position, lerpAmount);
        }
    }

    public void NewUpdate(ushort tick, Vector3 position)
    {
        if (tick <= NetworkManager.Singleton.InterpolationTick)
            return;

        foreach (TransformUpdate update in futureTransformUpdates)
        {
            if (tick < update.Tick)
            {
                futureTransformUpdates.Insert(futureTransformUpdates.IndexOf(update), new TransformUpdate(tick, position));
                return;
            }
        }

        futureTransformUpdates.Add(new TransformUpdate(tick, position));
    }
}
