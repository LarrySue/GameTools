using System;
using System.Collections.Generic;

namespace Model {
    /// <summary>
    /// 游戏
    /// </summary>
    public class Game {
        public Game() { }

        public String ID { get; }
        public String Name { get; }
        public String Nickname { get; }
        public List<Mission> MissionList { get; }
    }
}
