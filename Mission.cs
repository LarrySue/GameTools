using System;
using System.Collections.Generic;

public class Mission {
    public Mission() { }

    public String ID { get; set; }
    public String Name { get; set; }
    public MissionType Type { get; set; }
    public Dictionary<String, object> Property { get; set; }


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
