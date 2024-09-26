using System;
using System.Collections.Generic;

/// <summary>
/// 魔兽世界成就
/// </summary>
public class WOWAchievement {
    public WOWAchievement(Dictionary<String, object> dict) {
        ID = dict["aID"].ToString();
        Name = dict["aName"].ToString();
        Description = "";

        if (dict["aDescribe"] != null) {
            Description = dict["aDescribe"].ToString();
        }
    }

    /// <summary>
    /// 成就ID
    /// </summary>
    public String ID { get; }
    /// <summary>
    /// 成就名称
    /// </summary>
    public String Name { get; }
    /// <summary>
    /// 成就描述
    /// </summary>
    public String Description { get; protected set; }

    /// <summary>
    /// 成就类型
    /// </summary>
    public WOWAchievementType Type { get; protected set; }

    /// <summary>
    /// 完成状态 1完成 0未完成
    /// </summary>
    public Int32 Status { get; protected set; }
}

/// <summary>
/// 集合类成就，由子成就组成
/// </summary>
public class WOWAchievementSet: WOWAchievement {
    public WOWAchievementSet(Dictionary<String, object> dict): base(dict) {
        Description = "完成下列成就";
        Type = WOWAchievementType.Set;
        Status = 1;

        Dictionary<String, object>[] arr = (Dictionary<String, object>[])dict["aAchievementList"];
        Set = new List<WOWAchievement>(arr.Length);

        for (int i = 0; i < arr.Length; i++) {
            Dictionary<String, object> tempDict = arr[i];
            String typeStr = (String)tempDict["aType"];

            if (typeStr == "set") {
                Set[i] = new WOWAchievementSet(tempDict);
            } else if (typeStr == "single") {
                Set[i] = new WOWAchievementSingle(tempDict);
            } else if (typeStr == "list") {
                Set[i] = new WOWAchievementList(tempDict);
            } else if (typeStr == "partiallyList") {
                Set[i] = new WOWAchievementPartiallyList(tempDict);
            } else if (typeStr == "count") {
                Set[i] = new WOWAchievementCount(tempDict);
            } else if (typeStr == "multiCount") {
                Set[i] = new WOWAchievementMultiCount(tempDict);
            }

            Status = Status * Set[i].Status;
        }
    }

    /// <summary>
    /// 子成就列表
    /// </summary>
    public List<WOWAchievement> Set { get; }
}

/// <summary>
/// 单一成就，由一个单独的目标构成
/// </summary>
public class WOWAchievementSingle: WOWAchievement {
    public WOWAchievementSingle(Dictionary<String, object> dict): base(dict) {
        Type = WOWAchievementType.Single;
        Status = (Int32)dict["aStatus"];
    }
}

/// <summary>
/// 列表成就，由一组子目标构成
/// </summary>
public class WOWAchievementList: WOWAchievement {
    public WOWAchievementList(Dictionary<String, object> dict): base(dict) {
        Type = WOWAchievementType.List;
        Status = 1;

        Dictionary<String, object>[] arr = (Dictionary<String, object>[])dict["aObjectList"];
        List = new List<Target>(arr.Length);

        for (int i = 0; i < arr.Length; i++) {
            Dictionary<String, object> tempDict = arr[i];

            Target target = new Target();
            target.Name = (String)tempDict["oName"];
            target.Status = (Int32)tempDict["oStatus"];

            List[i] = target;
            Status = Status * target.Status;
        }
    }

    /// <summary>
    /// 子目标列表
    /// </summary>
    public List<Target> List { get; protected set; }
}

/// <summary>
/// 部分列表成就，达成列表中的一部分即可
/// </summary>
public class WOWAchievementPartiallyList: WOWAchievementList {
    public WOWAchievementPartiallyList(Dictionary<String, object> dict): base(dict) {
        Type = WOWAchievementType.PartiallyList;
        TargetValue = (Int32)dict["aTargetValue"];
        Description = ((String)dict["aDescribe"]).Replace("$aTargetValue$", TargetValue.ToString());

        Int32 count = 0;

        foreach (Target target in List) {
            count += target.Status;
        }

        Status = (count >= TargetValue) ? 1 : 0;
    }

    /// <summary>
    /// 目标值
    /// </summary>
    public int TargetValue { get; }
}

/// <summary>
/// 单计数成就，单一目标计数
/// </summary>
public class WOWAchievementCount: WOWAchievement {
    public WOWAchievementCount(Dictionary<String, object> dict): base(dict) {
        Type = WOWAchievementType.Count;
        TargetValue = (Int32)dict["aTargetValue"];
        Value = (Int32)dict["aValue"];
        Status = (Value >= TargetValue) ? 1 : 0;
    }

    /// <summary>
    /// 目标值
    /// </summary>
    public Int32 TargetValue { get; }
    /// <summary>
    /// 已完成值
    /// </summary>
    public Int32 Value { get; }
}

/// <summary>
/// 多计数成就，多个独立的目标计数
/// </summary>
public class WOWAchievementMultiCount: WOWAchievement {
    public WOWAchievementMultiCount(Dictionary<String, object> dict): base(dict) {
        Type = WOWAchievementType.MultiCount;
        Status = 1;

        Dictionary<String, object>[] arr = (Dictionary<String, object>[])dict["aObjectList"];
        List = new List<CountTarget>(arr.Length);

        for (int i = 0; i < arr.Length; i++) {
            Dictionary<String, object> tempDict = arr[i];

            CountTarget target = new CountTarget();
            target.Name = (String)tempDict["oName"];
            target.TargetValue = (Int32)tempDict["oTargetValue"];
            target.Value = (Int32)tempDict["oValue"];
            target.Status = (target.Value >= target.TargetValue) ? 1 : 0;

            List[i] = target;
            Status = Status * target.Status;
        }
    }

    /// <summary>
    /// 计数目标列表
    /// </summary>
    public List<CountTarget> List { get; }
}


/// <summary>
/// 成就类型
/// </summary>
public enum WOWAchievementType {
    /// <summary>
    /// 集合，由子成就组成
    /// </summary>
    Set,
    /// <summary>
    /// 单一，由一个单独的目标构成
    /// </summary>
    Single,
    /// <summary>
    /// 列表，由一组子目标构成
    /// </summary>
    List,
    /// <summary>
    /// 部分列表，达成列表中的一部分即可
    /// </summary>
    PartiallyList,
    /// <summary>
    /// 单计数，单一目标计数
    /// </summary>
    Count,
    /// <summary>
    /// 多计数，多个独立的目标计数
    /// </summary>
    MultiCount,
}

/// <summary>
/// 目标,仅包含完成状态
/// </summary>
public struct Target {
    public String Name;
    public Int32 Status;
}

/// <summary>
/// 计数目标,包含目标计数和当前计数
/// </summary>
public struct CountTarget {
    public String Name;
    public Int32 Status;
    public Int32 TargetValue;
    public Int32 Value;
}
