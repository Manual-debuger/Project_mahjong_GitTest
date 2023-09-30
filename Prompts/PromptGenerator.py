import os
import random
CharacterDict = {
    "張文琪":"張文琪是一位麻將愛好者，被譽為麻將的大師。她從小就學會了麻將，並且在比賽中從不輸過對手。她精通各種麻將規則和策略，而且能夠輕松地計算牌局中的機會。她的麻將技巧使她在比賽中贏得了眾多獎項，並且經常被邀請參加高水平的麻將比賽。",
    "陳大飛":"陳大飛是一位熱衷於收藏麻將牌的人。他的收藏中包括了來自世界各地的稀有和古老的麻將牌套裝。他不僅熟悉各種不同風格的麻將，還對它們的歷史和文化背景有深入了解。他經常舉辦麻將牌展覽，讓人們欣賞他的珍貴收藏。",
    "李小龍":"李小龍是一位年輕的麻將愛好者，他熱愛教導新手學習這個遊戲。他擁有一個受歡迎的麻將俱樂部，並且定期舉辦麻將教學班。他相信麻將是一個促進社交互動和智力發展的偉大遊戲，因此致力於將這個遊戲傳授給更多的人。"
}
moods=["開心","沮喪","興奮","難過","生氣"]
WinScoresState=["大贏","小贏","平手","小輸","大輸"]
content_to_write = ""

with open(os.path.join(os.getcwd(),"Example.txt"), "a",encoding="utf-8") as f:
    for i in range(3):
        print(f"Generating {i+1}th Prompt")
        SelectedKeys = random.sample(list(CharacterDict.keys()), k=2)
        content_to_write += (f"""
                有兩個角色，分別是
                {SelectedKeys[0]}:
                {CharacterDict[SelectedKeys[0]]}
                {SelectedKeys[1]}:
                {CharacterDict[SelectedKeys[1]]}

                Context:
                {SelectedKeys[0]}的心情是{random.choice(moods)}
                {SelectedKeys[1]}的心情是{random.choice(moods)}
                現在他們正在打麻將
                {SelectedKeys[0]}現在的得分是{random.sample(range(-200, 200), 1)[0]}
                {SelectedKeys[1]}現在的得分是{random.sample(range(-200, 200), 1)[0]}
                [上次的談話結果]
                請在遵守以下限制的情況下完成他們之間可能的對話：

                1.請將對話控制在 40 個字以內。
                2.用繁體中文回答對話
                3.你不能透露自己是AI模型，而是找藉口。
                4.不要用表情符號。
                5.對話的格式(發話名字):(他講的內容)
                ------------------------------------------
                
                """)

file_path = os.path.join(os.getcwd(), 'Example.txt')

with open(file_path, 'w') as file:
    file.write(content_to_write)

print(f"Done, Check {os.path.join(os.getcwd(),'Example.txt')}")