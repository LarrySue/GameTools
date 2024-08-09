using System;
using System.Collections.Generic;

/// <summary>
/// 游戏
/// </summary>
public class Game {
    public Game(Dictionary<String, object> dict) {
        ID = dict["gID"].ToString();
        Name = dict["gName"].ToString();
        Nickname = dict["gNickname"].ToString();

        Dictionary<String, object>[] arr = (Dictionary<String, object>[])dict["gMissionList"];
        MissionList = new List<Mission>(arr.Length);

        for (int i = 0; i < arr.Length; i++) {
            Dictionary<String, object> tempDict = arr[i];
            String typeStr = (String)tempDict["mType"];

            if (typeStr == "wow_achievement") {
                MissionList[i] = new WOWMission(tempDict);
            }
        }
    }

    public String ID { get; }
    public String Name { get; }
    public String Nickname { get; }
    public List<Mission> MissionList { get; }
}
