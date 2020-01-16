using UnityEngine;
using System.Collections.Generic;

enum Layer
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Water = 4,
    UI = 5,
    PostProcessing = 8,
    Wall = 9,
    Ball = 10,
    HitBallsOnly = 11,
    Player = 12,
    SafeZone = 13,
    AttackOrb = 14
}

public static class PhysicsLayers
{
    private static Dictionary<int, int> _collisionMatrix;


    public static void PopulateCollisionMatrix()
    {
        _collisionMatrix = new Dictionary<int, int>();
        for (int i = 0; i < 32; i++)
        {
            int mask = 0;
            for (int j = 0; j < 32; j++)
            {
                if (!Physics2D.GetIgnoreLayerCollision(i, j))
                {
                    mask |= 1 << j;
                }
            }
            _collisionMatrix.Add(i, mask);
        }
    }

    public static int LayerMask(int layer)
    {
        if (_collisionMatrix == null)
        {
            PopulateCollisionMatrix();
        }
        return _collisionMatrix[layer];
    }
}

