using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameTools {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow: Window {
        /// <summary>
        /// 数据列表
        /// </summary>
        private List<Game> gameList;

        public MainWindow() {
            Loaded += MainWindow_Loaded;

            InitializeComponent();

            Console.WriteLine("================================================");
            Console.WriteLine("InitializeComponent");
            Console.WriteLine("==================================================");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            gameList = new List<Game>();

            for (int i = 0; i < gameList.Count; i++) {
                Game game = gameList[i];
                TextBlock txb = new TextBlock
                {
                    Text = game.Name,
                    FontSize = 20
                };

                LeftStp.Children.Add(txb);
            }
        }
    }
}
