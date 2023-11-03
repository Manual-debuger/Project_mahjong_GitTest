using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitsResponse
{
    public string message = "";
    public string base64voice = "";
}
public class ParsedVitsResponse
{    
    public AudioClip voice ;
    public List<Tuple<string, string>> message;
    public ParsedVitsResponse(VitsResponse vitsResponse)
    {
        message = ChatGPTTool.Parsing(vitsResponse.message);
        // Convert the downloaded base64 string to byte array
        byte[] decodedBytes=Convert.FromBase64String(vitsResponse.base64voice);
        // Create an AudioClip from the mp3 data
        voice = MP3Transform.MP3ToAudioClip(decodedBytes);
    }
}
public class PromptFactors
{   
    public List<Tuple<string, string>> CharacterList;
    public List<string> MoodsList;
    public List<string> DirectionsList;
    public List<int> CurrentScoresList;
    public string SpecialWinPoint;
    public int WinScore;
    public PromptFactors(List<Tuple<string,string>> characterList, List<string> moodsList=null,List<string> directionsList=null,List<int> currentScoresList=null,int winScore=-1)
    {
        this.MoodsList = moodsList;
        this.CharacterList = characterList; 
        this.DirectionsList = directionsList;
        this.CurrentScoresList = currentScoresList;
        this.WinScore = winScore;
    }
 
}
#region Event Args
public class DiscardTileEventArgs:EventArgs
{
    public TileSuits TileSuit;
    public int PlayerIndex;
    public DiscardTileEventArgs(TileSuits tileSuits,int playerIndex)
    {
        TileSuit = tileSuits;
        PlayerIndex = playerIndex;
    }
}
public class TileSuitsLIstEventArgs:EventArgs
{
    public List<TileSuits> TileSuitsList;
    public int PlayerIndex;
    public TileSuitsLIstEventArgs(List<TileSuits> tileSuitsList, int playerIndex)
    {
        TileSuitsList = tileSuitsList;
        PlayerIndex = playerIndex;
    }
}
public class ChowTileEventArgs : TileSuitsLIstEventArgs
{

    public ChowTileEventArgs(List<TileSuits> tileSuitsList, int playerIndex):base(tileSuitsList,playerIndex)
    {
        
    }
}
public class PongTileEventArgs : TileSuitsLIstEventArgs
{
    
    public PongTileEventArgs(List<TileSuits> tileSuitsList, int playerIndex):base (tileSuitsList,playerIndex) { }
}
public class KongTileEventArgs : TileSuitsLIstEventArgs
{
    public bool IsConcealedKong;
   
    public KongTileEventArgs(bool isConcealedKong,List<TileSuits> tileSuitsList, int playerIndex):base(tileSuitsList,playerIndex)
    {       
        IsConcealedKong = isConcealedKong;
    }
}
//public class DiscardTileSuggestArgs:DiscardTileEventArgs
//{
//    public DiscardTileSuggestArgs(TileSuits tileSuits, int playerIndex) : base(tileSuits, playerIndex)
//    {
//    }
//}
public class WinningSuggestArgs : EventArgs
{
    public TileSuits TileSuits;
    public int PlayerIndex;
    public WinningSuggestArgs(TileSuits tileSuits, int playerIndex)
    {
        TileSuits = tileSuits;
        PlayerIndex = playerIndex;
    }
}
public class TileIndexEventArgs : EventArgs
{
    public int TileIndex;
    public TileIndexEventArgs(int tileIndex)
    {
        TileIndex = tileIndex;
    }
}

public class TileSuitEventArgs : EventArgs
{
    public TileSuits TileSuit;
    public TileSuitEventArgs(TileSuits tileSuit)
    {
        TileSuit = tileSuit;
    }
}

#region State Event
public class WaitingEventArgs : EventArgs
{
    public List<SeatInfo> Seats;
    public long Time;
    public int TableID;
    public long? NextStateTime;
    public int Round;
    public int Ante;
    public int ScorePerPoint;
    public WaitingEventArgs(List<SeatInfo> seats, long time, int tableID, long? nextStateTime, int round, int ante, int scorePerPoint)
    {
        Seats = seats;
        Time = time;
        TableID = tableID;
        NextStateTime = nextStateTime;
        Round = round;
        Ante = ante;
        ScorePerPoint = scorePerPoint;
    }
}

public class RandomSeatEventArgs : EventArgs
{
    public int SelfSeatIndex;
    public List<SeatInfo> Seats;
    public RandomSeatEventArgs(int index, List<SeatInfo> seats)
    {
        SelfSeatIndex = index;
        Seats = seats;
    }
}

public class DecideBankerEventArgs : EventArgs
{
    public int BankerIndex;
    public int? RemainingBankerCount;
    public List<SeatInfo> Seats;
    public DecideBankerEventArgs(int bankerIndex, int? remainingBankerCount, List<SeatInfo> seats)
    {
        BankerIndex = bankerIndex;
        RemainingBankerCount = remainingBankerCount;
        Seats = seats;
    }
}

public class OpenDoorEventArgs : EventArgs
{
    public int WallCount;
    public List<TileSuits> Tiles;
    public List<SeatInfo> Seats;
    public OpenDoorEventArgs(int wallCount, List<TileSuits> tiles, List<SeatInfo> seats)
    {
        WallCount = wallCount;
        Tiles = tiles;
        Seats = seats;
    }
}

public class GroundingFlowerEventArgs : EventArgs
{
    public int WallCount;
    public List<TileSuits> Tiles;
    public List<SeatInfo> Seats;
    public GroundingFlowerEventArgs(int wallCount, List<TileSuits> tiles, List<SeatInfo> seats)
    {
        WallCount = wallCount;
        Tiles = tiles;
        Seats = seats;
    }
}

public class PlayingEventArgs : EventArgs
{
    public int PlayingIndex; 
    public long PlayingTimeLeft;
    public int WallCount;
    public List<TileSuits> Tiles;
    public ActionData[] Actions;
    public List<SeatInfo> Seats;
    public PlayingEventArgs(int playingIndex, long playingTimeLeft, int wallCount, List<TileSuits> tiles, ActionData[] actions, List<SeatInfo> seats)
    {
        PlayingIndex = playingIndex;
        PlayingTimeLeft = playingTimeLeft;
        WallCount = wallCount;
        Tiles = tiles;
        Actions = actions;
        Seats = seats;
    }
}

public class WaitingActionEventArgs : EventArgs
{
    public int PlayingIndex;
    public long PlayingTimeLeft;
    public int WallCount;
    public List<TileSuits> Tiles;
    public ActionData[] Actions;
    public List<SeatInfo> Seats;
    public WaitingActionEventArgs(int playingIndex, long playingTimeLeft, int wallCount, List<TileSuits> tiles, ActionData[] actions, List<SeatInfo> seats)
    {
        PlayingIndex = playingIndex;
        PlayingTimeLeft = playingTimeLeft;
        WallCount = wallCount;
        Tiles = tiles;
        Actions = actions;
        Seats = seats;
    }
}

public class HandEndEventArgs : EventArgs
{
    public long PlayingTimeLeft;
    public List<SeatInfo> Seats;
    public HandEndEventArgs(long playingTimeLeft, List<SeatInfo> seats)
    {
        PlayingTimeLeft = playingTimeLeft;
        Seats = seats;
    }
}

public class GameEndEventArgs : EventArgs
{
    public long PlayingTimeLeft;
    public List<SeatInfo> Seats;
    public GameEndEventArgs(long playingTimeLeft, List<SeatInfo> seats)
    {
        PlayingTimeLeft = playingTimeLeft;
        Seats = seats;
    }
}

public class ClosingEventArgs : EventArgs
{
    public List<SeatInfo> Seats;
    public ClosingEventArgs(List<SeatInfo> seats)
    {
        Seats = seats;
    }
}
#endregion
#region Action Event
public class PassActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public PassActionEventArgs(int index, Action action)
    {
        Index = index;
        Action = action;
    }
}

public class DiscardActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public List<TileSuits> Options;
    public DiscardActionEventArgs(int index, Action action, List<TileSuits> options)
    {
        Index = index;
        Action = action;
        Options = options;
    }
}

public class ChowActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public List<List<TileSuits>> Options;
    public ChowActionEventArgs(int index, Action action, List<List<TileSuits>> options)
    {
        Index = index;
        Action = action;
        Options = options;
    }
}

public class PongActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public List<List<TileSuits>> Options;
    public PongActionEventArgs(int index, Action action, List<List<TileSuits>> options)
    {
        Index = index;
        Action = action;
        Options = options;
    }
}

public class KongActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public List<List<TileSuits>> Options;
    public KongActionEventArgs(int index, Action action, List<List<TileSuits>> options)
    {
        Index = index;
        Action = action;
        Options = options;
    }
}

public class ReadyHandActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public List<TileSuits> Option;
    public ReadyHandActionEventArgs(int index, Action action, List<TileSuits> option)
    {
        Index = index;
        Action = action;
        Option = option;
    }
}

public class WinActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public WinActionEventArgs(int index, Action action)
    {
        Index = index;
        Action = action;
    }
}

public class DrawnActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public int DrawnCount;
    public DrawnActionEventArgs(int index, Action action, int drawnCount)
    {
        Index = index;
        Action = action;
        DrawnCount = drawnCount;
    }
}

public class GroundingFlowerActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public int DrawnCount;
    public GroundingFlowerActionEventArgs(int index, Action action, int drawnCount)
    {
        Index = index;
        Action = action;
        DrawnCount = drawnCount;
    }
}

public class DrawnFromDeadWallActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public DrawnFromDeadWallActionEventArgs(int index, Action action)
    {
        Index = index;
        Action = action;
    }
}

public class SelfDrawnWinActionEventArgs : EventArgs
{
    public int Index;
    public Action Action;
    public SelfDrawnWinActionEventArgs(int index, Action action)
    {
        Index = index;
        Action = action;
    }
}
#endregion

public class ResultEventArgs : EventArgs
{
    public List<PlayerResultData> PlayerResult;
    public ResultEventArgs(List<PlayerResultData> playerResult)
    {
        PlayerResult = playerResult;
    }
}

public class FloatEventArgs : EventArgs
{
    public float f;
    public FloatEventArgs(float Float)
    {
        f = Float;
    }
}

public class BoolEventArgs : EventArgs
{
    public bool b;
    public BoolEventArgs(bool Bool)
    {
        b = Bool;
    }
}

#endregion