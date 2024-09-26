using System;
using System.Collections.Generic;

/// <summary>
/// 待办事项
/// </summary>
public class Todo {
    public Todo(Dictionary<String, object> dict) {
        ID = dict["tID"].ToString();
        Name = dict["tName"].ToString();
        Description = dict["tDescribe"].ToString();
    }

    /// <summary>
    /// 待办ID
    /// </summary>
    public String ID { get; }
    /// <summary>
    /// 待办名称
    /// </summary>
    public String Name { get; }
    /// <summary>
    /// 待办描述
    /// </summary>
    public String Description { get; }
    /// <summary>
    /// 待办类型
    /// </summary>
    public TodoType Type { get; protected set; }
}

/// <summary>
/// WOW成就待办
/// </summary>
public class WOWAchievementTodo: Todo {
    public WOWAchievementTodo(Dictionary<String, object> dict): base(dict) {
        Type = TodoType.WOWAchievementTodo;
        Achievement = new WOWAchievement((Dictionary<string, object>)dict["tProperty"]);
    }

    public WOWAchievement Achievement { get; protected set; }
}

/// <summary>
/// 待办类型
/// </summary>
public enum TodoType {
    /// <summary>
    /// WOW成就待办
    /// </summary>
    WOWAchievementTodo
}
