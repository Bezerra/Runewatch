using System;
[Flags]
public enum PieceConcreteType
{
    None = 0,
    BossRoom = 1,
    Corridor = 2,
    LargeCorridor = 4,
    RoomFourExitsBigger = 8,
    RoomFourExits = 16,
    RoomTwoExits = 32,
    Wall = 64,
    RoomTwoExitsDifferentPoints = 128,
    Stairs = 256,
}
