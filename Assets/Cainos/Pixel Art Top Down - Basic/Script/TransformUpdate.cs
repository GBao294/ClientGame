using UnityEngine;

public class TransformUpdate
{
    public ushort Tick { get; private set; }
    public Vector2 Position { get; private set; }

    public TransformUpdate(ushort tick, Vector2 position)
    {
        Tick = tick;
        Position = position;
    }
}
