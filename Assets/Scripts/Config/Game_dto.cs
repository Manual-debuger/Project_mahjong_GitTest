using System;
using System.Collections.Generic;

public class Seat<T>
{
    public string Name; // 玩家ID
    public string Nickname; // 玩家名
    public string Avatar; // 頭像
    public string AvatarBackground; // 頭像背景
    public int Gender; //  0 男生 1 女生 
    public string VoiceLanguage; // 0 國語 1 台語
    public int? TileCount; // 玩家手牌數量
    public List<string[]> Door; // 玩家有碰槓吃等資訊
    public string[] Flowers; // 玩家花牌(後端字串格式)
    public string[] Sea; // 玩家海底(後端字串格式)
    public List<List<TileSuits>> DoorTile; // 玩家花牌(遊戲內的數值格式)
    public List<TileSuits> FlowerTile; // 玩家花牌(遊戲內的數值格式)
    public List<TileSuits> SeaTile; // 玩家海底(遊戲內的數值格式)
    public string DoorWind; // 玩家此局風向
    public int WinScores; // 該場遊戲輸贏分


    public int Scores; // 玩家金錢分數
    public int Index; // 玩家在陣列中的index
    public int? WinCount; // 玩家贏的場次
    public int? LoseCount; // 玩家輸的場次
    
    public bool? Ready; // 是否準備
    public bool? ReadyHand; // 是否聽
    public int[] Location; // 玩家座標位置
    public bool? AutoPlaying; // 是否託管
    
}

public class SeatInfo : Seat<List<string[]>>
{
}

public static class SeatExtensions
{
    public static SeatInfo CloneWithTiles(this SeatInfo seat, List<List<TileSuits>> doorList, List<TileSuits> flowerList, List<TileSuits> seaList)
    {
        return new SeatInfo
        {
            Name = seat.Name,
            Nickname = seat.Nickname,
            Avatar = seat.Avatar,
            AvatarBackground = seat.AvatarBackground,
            Gender = seat.Gender,
            VoiceLanguage = seat.VoiceLanguage,
            TileCount = seat.TileCount,
            DoorTile = doorList,
            FlowerTile = flowerList,
            SeaTile = seaList,
            DoorWind = seat.DoorWind,
            WinScores = seat.WinScores,
            Scores = seat.Scores,
            Index = seat.Index,
            WinCount = seat.WinCount,
            LoseCount = seat.LoseCount,
            Ready = seat.Ready,
            ReadyHand = seat.ReadyHand,
            Location = seat.Location,
            AutoPlaying = seat.AutoPlaying
        };
    }
}

[System.Serializable]
public class MessageData
{
    #region Event data
    public long Time;
    public int ClubID;
    public int TableID;
    public string State;
    public int? PlayingIndex;
    public long? PlayingDeadline;
    public long? NextStateTime;
    public int Index;
    public int Round;
    public int MaxHand;
    public int Hand;
    public int? WallCount;
    public int? BankerIndex;
    public int? Dice;
    public string[] Tiles;
    public ActionData[] Actions;
    public SeatInfo[] Seats;
    public int Ante;
    public int ScorePerPoint;
    public string[] Doras;
    #endregion

    #region Play data
    // public int SelfSeatIndex; Have it already
    public Action Action;
    public int? DrawnCount;
    public string[] Option;
    public List<string[]> Options;
    #endregion

    public int? RemainingBankerCount;
    public List<string[]> Door;
    public List<string[]> AllTiles;
    public ListeningTilesType ListeningTiles;
    // 遊戲回放給予
    public int? PrePlayingIndex;
    public string RoundID;
    public bool? PassingWin;


    public Action ID;
    public ReadyInfoType ReadyInfo;

    

    public string NickName;
    public int PlayerID;
    public int WinScores;
    public int Scores;
    public bool Winner;
    public bool Loser;
    public bool Banker;
    public bool SelfDrawn;
    public int CardinalDirection;
    public int Points;
    public PointType[] PointList;
    public string LastTile;
}

// Define the MessageObject structure to match the incoming JSON data
[System.Serializable]
public class MessageObject
{
    public string Path;
    public MessageData Data;
}

[System.Serializable]
public class ActionData
{
    public Action ID;
    public List<string[]> Options;
    public ReadyInfoType ReadyInfo;
}

[System.Serializable]
public class PointType
{
    public string Describe;
    public int Point;
}

[System.Serializable]
public class PlayerResultData<T>
{
    public T[] Tiles;
    public int Points;
    public PointType[] PointList;
    public int? WinScores;
    public bool? SelfDrawn;
    public bool? Winner;
    public bool? Loser;
    public bool? Banker;
    public bool? Bankruptcy;
    public bool? Disconnected;
    public bool? InsufficientBalance;
    public int? TableFee;
    public bool? HandWin;
}

[System.Serializable]
public class PlayerGameResultData
{
    public string Avatar;
    public string AvatarBackground; // 頭像背景
    public int Gender;
    public int VoiceLanguage;
    public string Name;
    public string Nickname;
    public int Scores;
    public int Index;
    public int WinCount;
    public int LoseCount;
    public int WinScores;
    public int? Compensation;
    public int? TableFee;
    public bool? Bankruptcy;
    public bool? Disconnected;
    public bool? TableOwner;
    public bool? InsufficientBalance;
}
/**
 * ReadyInfo => {
 * 
	丟的牌: {

		 聽的牌: 幾台,
		 聽的牌: 幾台   
	}
  }
 */
[System.Serializable]
public class ReadyInfoType
{
    public Dictionary<string, ListeningTilesType> key;
}

/**
 * {
 * 聽的牌: 幾台
 * }
 */
[System.Serializable]
public class ListeningTilesType
{
    public Dictionary<string, int> Mahjong;
}

public static class Path
{
    public const string Ack = "auth.ack";
    public const string Login = "auth.login";
    public const string TableEnter = "game.table.enter";
    public const string TableEvent = "game.table.event";
    public const string TablePlay = "game.table.play";
    public const string TableResult = "game.table.result";
}

[System.Serializable]
public class LoginData
{
    public bool IsGuest;
    public string Token;
}

[System.Serializable]
public class LoginObject
{
    public string Path;
    public LoginData Data;
}

[System.Serializable]
public class TableEnterObject
{
    public string Path;
    public object Data;
}



[Serializable]
public class TableEventData
{
    public int Ante;
    public int ScorePerPoint;
    public int ClubID;
    public int TableID;
    public int MaxHand;
    public int Hand;
    public int Round;
    public int Index;
    public SeatInfo[] Seats;
    public string State;
    public long Time;
    public ActionData[] Actions;
    public long? NextStateTime;
    public int? Dice;
    public string[] Doras;
    public int? BankerIndex;
    public int? RemainingBankerCount;
    public int? WallCount;
    public int? PlayingIndex;
    public string[] Tiles;
    public List<string[]> Door;
    public long? PlayingDeadline;
    public List<string[]> AllTiles;
    public ListeningTilesType ListeningTiles;
    // 遊戲回放給予
    public int? PrePlayingIndex;
    public string RoundID;
    public bool? PassingWin;
}

[System.Serializable]
public class TablePlayData
{
    //public int SelfSeatIndex;
    public Action Action;
    public string[] Option;
    public int? DrawnCount;
}

[System.Serializable]
public class ResultData
{
    public int Index;
    public string NickName;
    public int PlayerID;
    public int WinScores;
    public int Scores;
    public bool Winner;
    public bool Loser;
    public bool Banker;
    public bool SelfDrawn;
    public int CardinalDirection;
    public int Points;
    public PointType[] PointList;
    public string[] Door;
    public string[] Tiles;
    public string LastTile;
}