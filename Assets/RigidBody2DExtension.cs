using UnityEngine;
using System.Collections.Generic;

public static class RigidBody2DExtensions
{
    public static void setX(this Rigidbody2D rigid, float x)
    {
        rigid.velocity = new Vector2(x, rigid.velocity.y);
    }
    public static void addX(this Rigidbody2D rigid, float x)
    {
        rigid.velocity = new Vector2(rigid.velocity.x + x, rigid.velocity.y);
    }

    public static void setY(this Rigidbody2D rigid, float y)
    {
        rigid.velocity = new Vector2(rigid.velocity.x, y);
    }
    public static void addY(this Rigidbody2D rigid, float y)
    {
        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y + y);
    }
}
