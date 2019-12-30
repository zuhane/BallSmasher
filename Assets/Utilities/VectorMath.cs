using System;
using UnityEngine;

public static class VectorMath
{
    public static Vector2 Abs(Vector2 vector)
    {
        return new Vector2(Math.Abs(vector.x), Math.Abs(vector.y));
    }
    public static Vector3 Abs(Vector3 vector)
    {
        return new Vector3(Math.Abs(vector.x), Math.Abs(vector.y), Math.Abs(vector.z));
    }

    public static Vector2 Random()
    {
        return new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
    }
    public static bool Accelerating(Vector2 oldVector, Vector2 newVector)
    {
        oldVector = Abs(oldVector);
        newVector = Abs(newVector);
        return newVector.x > oldVector.x || newVector.y > oldVector.y;
    }
}
