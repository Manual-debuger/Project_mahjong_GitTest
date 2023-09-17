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
    public bool? AutoPlaying; // 是否託管
    public int? TileCount; // 玩家手牌數量
    public string[] Flowers; // 玩家花牌(後端字串格式)
    public string[] Sea; // 玩家海底(後端字串格式)
    public List<string[]> Door; // 玩家有碰槓吃等資訊
    public List<TileSuits> FlowerTile; // 玩家花牌(遊戲內的數值格式)
    public List<TileSuits> SeaTile; // 玩家海底(遊戲內的數值格式)
    public List<List<TileSuits>> DoorTile; // 玩家花牌(遊戲內的數值格式)
    public bool? ReadyHand; // 是否聽
    public string DoorWind; // 玩家此局風向
    public int WinScores; // 該場遊戲輸贏分

    // never seen
    public int Scores; // 玩家金錢分數
    public int Index; // 玩家在陣列中的index
    public int? WinCount; // 玩家贏的場次
    public int? LoseCount; // 玩家輸的場次
    public bool? Ready; // 是否準備
    public int[] Location; // 玩家座標位置
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
    // Event Data
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

    // Play Data
    public Action Action;
    public int? DrawnCount;
    public string[] Option;
    public List<string[]> Options;

    // 遊戲回放給予
    public int? PrePlayingIndex;
    public string RoundID;
    public bool? PassingWin;

    // Result Data
    public bool? InterceptWin;
    public int? RemainingBankerCount;
    public PlayerResultData[] Results;
    public string[] Wall;
}

// Define the MessageObject structure to match the incoming JSON data
[System.Serializable]
public class MessageObject
{
    public string Path;
    public MessageData Data;
}

/*
 * ReadyInfo: {
	丟的牌1: {
		 聽的牌1: 幾台,
		 聽的牌2: 幾台   
	},
    丟的牌2: {
		 聽的牌3: 幾台,
		 聽的牌4: 幾台
	}
   }

    "ReadyInfo": {
        "c5": {"c8": 2},
        "c8": {"c5": 2}
    }
 */
[System.Serializable]
public class ActionData
{
    public Action ID;
    public List<string[]> Options;
    public List<List<TileSuits>> OptionTiles;
    public Dictionary<string, Dictionary<string, int>> ReadyInfo;
    public Dictionary<TileSuits, Dictionary<TileSuits, int>> ReadyInfoTile;
}

[System.Serializable]
public class PointType
{
    public string Describe;
    public int Point;
}

[System.Serializable]
public class PlayerResultData
{
    public string Name;
    public string Nickname;
    public int Scores;
    public string Avatar;
    public string AvatarBackground; // 頭像背景
    public int Gender;
    public string VoiceLanguage;
    public int Score;
    public int WinScores;
    public bool? Winner; // 胡
    public PointType[] PointList; // 贏的台型
    public bool? Loser; // 放槍
    public bool? Banker;
    public int Index;
    public string DoorWind;
    public List<string[]> Door;
    public string[] Tiles;
    public string[] Flowers;
    public List<List<TileSuits>> DoorTile;
    public List<TileSuits> Tile;
    public List<TileSuits> FlowerTile;
    public bool? SelfDrawn;
    public int WinPoint;

    // need add
    public int Points;

    // never seen
    public int? WinCount;
    public int? LoseCount;
    public bool? Bankruptcy;
    public bool? Disconnected;
    public bool? InsufficientBalance;
    public int? TableFee;
    public bool? HandWin;
}

public static class PlayerResultExtensions
{
    public static PlayerResultData CloneWithTiles(this PlayerResultData playerResult, List<List<TileSuits>> doorList, List<TileSuits> tileList, List<TileSuits> flowerList)
    {
        return new PlayerResultData
        {
            Name = playerResult.Name,
            Nickname = playerResult.Nickname,
            Scores = playerResult.Scores,
            Avatar = playerResult.Avatar,
            AvatarBackground = playerResult.AvatarBackground,
            Gender = playerResult.Gender,
            VoiceLanguage = playerResult.VoiceLanguage,
            Score = playerResult.Score,
            WinScores = playerResult.WinScores,
            Banker = playerResult.Banker,
            Index = playerResult.Index,
            DoorWind = playerResult.DoorWind,
            DoorTile = doorList,
            Tile = tileList,
            FlowerTile = flowerList,
            SelfDrawn = playerResult.SelfDrawn,
            Winner = playerResult.Winner,
            Loser = playerResult.Loser,
            WinPoint = playerResult.WinPoint,

            Points = playerResult.Points,
            PointList = playerResult.PointList,

            WinCount = playerResult.WinCount,
            LoseCount = playerResult.LoseCount,
            Bankruptcy = playerResult.Bankruptcy,
            Disconnected = playerResult.Disconnected,
            InsufficientBalance = playerResult.InsufficientBalance,
            TableFee = playerResult.TableFee,
            HandWin = playerResult.HandWin,
        };
    }
}

// Leave this table's result
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
    public int WinPoint;
    public int? Compensation;
    public int? TableFee;
    public bool? Bankruptcy;
    public bool? Disconnected;
    public bool? TableOwner;
    public bool? InsufficientBalance;
}

public static class Path
{
    public const string Ack = "auth.ack";
    public const string Login = "auth.login";
    public const string TableEnter = "game.table.enter";
    public const string TableEvent = "game.table.event";
    public const string TablePlay = "game.table.play";
    public const string TableResult = "game.table.result";
    public const string TableAutoPlay = "game.table.autoplay";
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

[System.Serializable]
public class TablePlayData
{
    public int Index;
    public Action Action;
    public string[]? Option;
    public int? DrawnCount;
}

[System.Serializable]
public class TablePlayObject
{
    public string Path;
    public TablePlayData Data;
}