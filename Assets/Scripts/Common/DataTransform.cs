using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataTransformNamespace
{
    public class DataTransform : MonoBehaviour
    {
        public static List<TileSuits> ReturnTileToIndex(string[] data)
        {
            if (data != null && data.Length > 0)
            {
                List<TileSuits> tileSuitsList = new List<TileSuits>();

                foreach (string tile in data)
                {
                    if (tile[0] == '_')
                    {
                        string typeString = tile.Substring(1, 2);
                        if (Enum.TryParse(typeString, out TileSuits tileSuit))
                        {
                            tileSuitsList.Add(tileSuit + 100);
                        }
                        else
                        {
                            Console.WriteLine($"Error: Invalid tile value '{tile}'. Possible values are: {string.Join(", ", Enum.GetNames(typeof(TileSuits)))}");
                        }
                    }
                    else
                    {
                        if (Enum.TryParse(tile, out TileSuits tileSuit))
                        {
                            tileSuitsList.Add(tileSuit);
                        }
                        // Handle error case if enum parsing fails
                    }
                }

                return tileSuitsList;
            }
            else
            {
                return new List<TileSuits>();
            }
        }

        public static List<SeatInfo> MapAllSeats(IEnumerable<SeatInfo> seats)
        {
            List<SeatInfo> processedSeats = new();
            foreach (SeatInfo seat in seats)
            {
                SeatInfo processedSeat = MapSeat(seat);
                processedSeats.Add(processedSeat);
            }
            return processedSeats;
        }

        public static SeatInfo MapSeat(SeatInfo seat)
        {
            List<List<TileSuits>> doorList = MapStringListsToTileSuitsLists(seat.Door);
            List<TileSuits> flowerList = ReturnTileToIndex(seat.Flowers);
            List<TileSuits> seaList = ReturnTileToIndex(seat.Sea);
            return seat.CloneWithTiles(doorList, flowerList, seaList);
        }

        public static List<List<TileSuits>> MapStringListsToTileSuitsLists(List<string[]> options)
        {
            //List<List<TileSuits>> Options = options != null ? options.ConvertAll(singleData => ReturnTileToIndex(singleData)) : new List<List<TileSuits>>();
            List<List<TileSuits>> option = new();
            if (options != null)
            {
                foreach (string[] singleData in options)
                {
                    List<TileSuits> convertedList = ReturnTileToIndex(singleData);
                    option.Add(convertedList);
                }
            }
            return option;
        }
    }
}