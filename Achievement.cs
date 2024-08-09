using System;
using System.Collections.Generic;

public class Achievement {
    public Achievement(Dictionary<String, object> dict) {
        ID = dict["aID"].ToString();
        Name = dict["aName"].ToString();
        Description = "";

        if (dict["aDescribe"] != null) {
            Description = dict["aDescribe"].ToString();
        }
    }

    public String ID { get; }
    public String Name { get; }
    public String Description { get; protected set; }

    /// <summary>
    /// 成就类型
    /// </summary>
    public AchievementType Type { get; protected set; }

    /// <summary>
    /// 完成状态 1完成 0未完成
    /// </summary>
    public Int32 Status { get; protected set; }
}

/// <summary>
/// 集合类成就，由子成就组成
/// </summary>
public class AchievementSet: Achievement {
    public AchievementSet(Dictionary<String, object> dict): base(dict) {
        Description = "完成下列成就";
        Type = AchievementType.Set;
        Status = 1;

        Dictionary<String, object>[] arr = (Dictionary<String, object>[])dict["aAchievementList"];
        Set = new List<Achievement>(arr.Length);

        for (int i = 0; i < arr.Length; i++) {
            Dictionary<String, object> tempDict = arr[i];
            String typeStr = (String)tempDict["aType"];

            if (typeStr == "set") {
                Set[i] = new AchievementSet(tempDict);
            } else if (typeStr == "single") {
                Set[i] = new AchievementSingle(tempDict);
            } else if (typeStr == "list") {
                Set[i] = new AchievementList(tempDict);
            } else if (typeStr == "partiallyList") {
                Set[i] = new AchievementPartiallyList(tempDict);
            } else if (typeStr == "count") {
                Set[i] = new AchievementCount(tempDict);
            } else if (typeStr == "multiCount") {
                Set[i] = new AchievementMultiCount(tempDict);
            }

            Status = Status * Set[i].Status;
        }
    }

    /// <summary>
    /// 子成就列表
    /// </summary>
    public List<Achievement> Set { get; }
}

/// <summary>
/// 单一成就，由一个单独的目标构成
/// </summary>
public class AchievementSingle: Achievement {
    public AchievementSingle(Dictionary<String, object> dict): base(dict) {
        Type = AchievementType.Single;
        Status = (Int32)dict["aStatus"];
    }
}

/// <summary>
/// 列表成就，由一组子目标构成
/// </summary>
public class AchievementList: Achievement {
    public AchievementList(Dictionary<String, object> dict): base(dict) {
        Type = AchievementType.List;
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
public class AchievementPartiallyList: AchievementList {
    public AchievementPartiallyList(Dictionary<String, object> dict): base(dict) {
        Type = AchievementType.PartiallyList;
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
public class AchievementCount: Achievement {
    public AchievementCount(Dictionary<String, object> dict): base(dict) {
        Type = AchievementType.Count;
        TargetValue = (Int32)dict["aTargetValue"];
        Value = (Int32)dict["aValue"];
        Status = (Value >= TargetValue) ? 1 : 0;
    }

    public Int32 TargetValue { get; }
    public Int32 Value { get; }
}

/// <summary>
/// 多计数成就，多个独立的目标计数
/// </summary>
public class AchievementMultiCount: Achievement {
    public AchievementMultiCount(Dictionary<String, object> dict): base(dict) {
        Type = AchievementType.MultiCount;
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

    public List<CountTarget> List { get; }
}


/// <summary>
/// 成就类型
/// </summary>
public enum AchievementType {
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
    Test
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
