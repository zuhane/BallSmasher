
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


enum LayerMask
{
    Default = 1 << 0,
    TransparentFX = 1 << 1,
    IgnoreRaycast = 1 << 2,
    Water = 1 << 4,
    UI = 1 << 5,
    PostProcessing = 1 << 8,
    Wall = 1 << 9,
    Ball = 1 << 10,
    HitBallsOnly = 1 << 11,
    Player = 1 << 12,
    SafeZone = 1 << 13,
    AttackOrb = 1 << 14
}