using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public partial class ChatGPTTool
{
    public static List<Tuple<string, string>> Parsing(string target)
    {
        Regex regex = new Regex(@"([^\n]+)[：:]([^\n]+)");
        List<Tuple<string, string>> result = new List<Tuple<string, string>>();
        MatchCollection matches = regex.Matches(target);
        foreach (Match match in matches)
        {
            result.Add(new Tuple<string, string>(match.Groups[1].Value, match.Groups[2].Value));
        }
        return result;
    }
    public static string GeneratePrompt(PromptType promptType,PromptFactors promptFactors)
    {
        string prompt = "";
        switch(promptType)
        {
            case PromptType.TwoManChat:
                prompt += $"有兩個角色，分別是\n" +
                    $"{promptFactors.CharacterList[0].Item1}：\n" +
                    $"{promptFactors.CharacterList[0].Item2}\n" +
                    $"{promptFactors.CharacterList[1].Item1}：\n" +
                    $"{promptFactors.CharacterList[1].Item2}\n" +
                    $"目前的情境是:\n" +
                    $"{promptFactors.CharacterList[0].Item1}的心情是{promptFactors.MoodsList[0]}\n" +
                    $"{promptFactors.CharacterList[1].Item1}的心情是{promptFactors.MoodsList[1]}\n" +
                    $"現在他們正在打麻將\n" +
                    $"{promptFactors.CharacterList[0].Item1}是{promptFactors.DirectionsList[0]}風位，現在的得分是{promptFactors.CurrentScoresList[0]}\n" +
                    $"{promptFactors.CharacterList[1].Item1}是{promptFactors.DirectionsList[1]}風位，現在的得分是{promptFactors.CurrentScoresList[1]}\n" +
                    $"";
                break;
            default:
                Debug.LogWarning("ChatGPTTool.GeneratePrompt: Don't support such promptType yet");
                break;
        }        
        prompt+= "\n你需要扮演一個說書人，你的目標是在遵守以下限制的情況下完成他們之間可能的吸引人的對話：\n" +
            "請將對話控制在 80 個字以內。\n" +
            "用繁體中文回答對話\n" +
            "你不能透露自己是AI模型，而是找藉口。\n" +
            "不要用表情符號。\n" +
            "對話的格式(發話名字):(他講的內容)\n" +
            "只顯示對話內容就好，不需要顯示如對話結束，Note等提示\n" +
            "如果要表達人物的動作或語氣等，請在他講的內容中用()顯示，如(開心地講)\n" +
            "以下提供幾點幫助你寫作:\n" +
            "角色的對話應該反應他們的性格或是當下的心情:\n" +
            "對話的內容應該是吸引人且令人信服的";
        return prompt;
    }  
    
}
