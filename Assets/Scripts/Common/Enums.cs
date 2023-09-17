using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum TileSuits
{
    //Character / crak (萬子)
    c1,
    c2,
    c3,
    c4,
    c5,
    c6,
    c7,
    c8,
    c9,
    //Dot (筒子/餅子)
    d1, 
    d2,
    d3,
    d4,
    d5,
    d6,
    d7,
    d8,
    d9,
    //Bamboo / bam (索子/條子)
    b1,
    b2,
    b3,
    b4,
    b5,
    b6,
    b7,
    b8,
    b9,
    //Other 東南西北 中發白
    o1,//東
    o2,//南
    o3,//西
    o4,//北
    o5,//中
    o6,//發
    o7,//白
    //FLower
    f1,//梅
    f2,//蘭
    f3,//竹
    f4,//菊
    f5,//春
    f6,//夏
    f7,//秋
    f8,//冬
    NULL
}
public enum MeldTypes
{
    Sequence,//順子
    Triplet,//刻子
    ConcealedQuadplet,//暗槓子
    ExposedQuadplet,//明槓子 
}
public enum DoorWind
{
    East,
    South,
    West,
    North,
}
public enum Action
{
    Pass,
    Discard,
    Chow,
    Pong,
    Kong,
    AdditionKong,
    ConcealedKong,
    ReadyHand,
    Win,
    Drawn,
    GroundingFlower,
    DrawnFromDeadWall,
    SelfDrawnWin,
}

public enum EffectID
{
    Chow,
    Pong,
    Kong,
    Listen,
    Win,
    None
}

public enum AudioType
{
    Chow,
    Pong,
    Kong,
    ReadyHand,
    Win,
    SelfWin,
    GroundingFlower
}

public enum HighlightBarState
{
    Default=0,
    Highlight=1
}