using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
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

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static SqLiteHelper sql;
        private Monsters monsterParent;
        private Monsters monsterChild1;
        private Monsters monsterChild2;

        private List<Photo> photos = new List<Photo>();

        private SQLiteDataReader getMonsterData(int id)
        {
            //读取整张表
            return sql.ExecuteQuery("select * from monsters where id = '" + id + "'");
        }

        private SQLiteDataReader getMonsterChildData(int id)
        {
            //读取整张表
            return sql.ExecuteQuery("select c.id, c.monsterChild1, c.monsterChild2 from compose c LEFT OUTER JOIN monsters m ON c.monsterParent = m.id where m.id = '" + id + "'");
        }

        private void setMonsterInfo(int id, Label label)
        {
            SQLiteDataReader reader = this.getMonsterData(id);
            while (reader.Read())
            {
                //读取ID
                Console.WriteLine("" + reader.GetInt32(reader.GetOrdinal("ID")));
                this.monsterParent.id = reader.GetInt32(reader.GetOrdinal("ID"));
                //读取Name
                Console.WriteLine("" + reader.GetString(reader.GetOrdinal("name")));
                this.monsterParent.name = reader.GetString(reader.GetOrdinal("name"));
                label.Content = this.monsterParent.name;
                //读取Age
                Console.WriteLine("" + reader.GetInt32(reader.GetOrdinal("hp")));
                this.monsterParent.hp = reader.GetInt32(reader.GetOrdinal("hp"));
            }
        }

        private void setMonsterChildInfo(int id)
        {
            Compose c = new Compose();
            SQLiteDataReader reader = this.getMonsterChildData(id);
            while (reader.Read())
            {
                //读取ID
                Console.WriteLine("" + reader.GetInt32(reader.GetOrdinal("ID")));
                c.id = reader.GetInt32(reader.GetOrdinal("ID"));
                c.child1 = reader.GetInt32(reader.GetOrdinal("monsterChild1"));
                c.child2 = reader.GetInt32(reader.GetOrdinal("monsterChild2"));
               
                this.setMonsterInfo(c.child1, this.labelChild1);
                this.setMonsterInfo(c.child2, this.labelChild2);
            }

        }

        public MainWindow()
        {
            InitializeComponent();
            
            //this.image.MouseDown += new MouseButtonEventHandler(Border_Title_MouseDown);
            this.monsterParent = new Monsters();
            this.monsterChild1 = new Monsters();
            this.monsterChild2 = new Monsters();
            sql = new SqLiteHelper("data source=new.db");
            this.setMonsterInfo(3, this.labelParent);
            this.setMonsterChildInfo(3);

            this.photos.Add(new Photo()
            {
                FullPath = "Resources/2.png",
                FullPath2 = "Resources/1.png",
                Content = "hello"
            });
            this.photos.Add(new Photo()
            {
                FullPath = "Resources/1.png",
                FullPath2 = "Resources/2.png",
                Content = "hello"
            });

            this.listBox.ItemsSource = this.photos;
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void labelParent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        
        private void saveToFile(BitmapSource bitmapSource, String fileName)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            encoder.Save(stream);
        }
    }
}
