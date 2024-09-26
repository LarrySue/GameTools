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

        Dictionary<String, object>[] arr = (Dictionary<String, object>[])dict["gTodoList"];
        TodoList = new List<Todo>(arr.Length);

        for (int i = 0; i < arr.Length; i++) {
            Dictionary<String, object> tempDict = arr[i];
            String typeStr = (String)tempDict["tType"];

            if (typeStr == "wow_achievement") {
                TodoList[i] = new WOWAchievementTodo(tempDict);
            }
        }
    }

    /// <summary>
    /// 游戏ID
    /// </summary>
    public String ID { get; }
    /// <summary>
    /// 游戏名称
    /// </summary>
    public String Name { get; }
    /// <summary>
    /// 游戏别名
    /// </summary>
    public String Nickname { get; }
    /// <summary>
    /// 游戏待办列表
    /// </summary>
    public List<Todo> TodoList { get; }
}
