using System;
using System.Collections.Generic;

/// <summary>
/// 任务
/// </summary>
public class Mission {
    public Mission(Dictionary<String, object> dict) {
        ID = dict["gID"].ToString();
        Name = dict["gName"].ToString();
        Description = dict["mDescribe"].ToString();
    }

    public String ID { get; }
    public String Name { get; }
    public String Description { get; }
    public MissionType Type { get; protected set; }
}

public class WOWMission: Mission {
    public WOWMission(Dictionary<String, object> dict): base(dict) {
        Type = MissionType.WOWAchievement;
        Achievement = new Achievement((Dictionary<string, object>)dict["mProperty"]);
    }

    public Achievement Achievement { get; protected set; }
}

/// <summary>
/// 任务类型
/// </summary>
public enum MissionType {
    /// <summary>
    /// WOW成就
    /// </summary>
    WOWAchievement
}
