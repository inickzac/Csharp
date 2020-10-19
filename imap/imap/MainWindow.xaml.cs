using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;
using System.ComponentModel;

namespace imap
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CoreImap CoreMail;
        MenuItem root;
        MenuItem newItemMenu;
        public MainWindow()
        {
            InitializeComponent();
            Action<string> WriteTerminal = new Action<string>((str) => {
                terminal.Dispatcher.BeginInvoke(new Action(() => {
                    terminal.Document.Blocks.Add(new Paragraph(new Run(str)));
                }));
            });

            connect.IsEnabled = true;
            Disconect.IsEnabled = false;
            CoreImap.AnswerFromServer += WriteTerminal;
            CoreImap.SendInServer += WriteTerminal;
            MenuItem newItemMenu;



            //CoreMail = new CoreImap("smtp.mail.ru", 993);
            //CoreMail.AddReaquest("LOGIN test45555555@mail.ru 12345678bnm", new Action<List<string>>((listSt) =>
            //{
            //    string str = string.Empty;
            //    foreach (string nstr in listSt)
            //    { str += nstr + " "; }
            //   // terminal.Dispatcher.BeginInvoke(new Action(() => terminal.Text = str));
            //}));

            //CoreMail = new CoreImap("imap.gmail.com", 993);
            //CoreMail.AddReaquest("login testmail9999899999@gmail.com 12345678test", new Action<List<string>>((listst) =>
            //{
            //    string str = string.Empty;
            //    foreach (string nstr in listst)
            //    { str += nstr + " "; }
            //    terminal.Dispatcher.BeginInvoke(new Action(() => terminal.Text = str));
            //}))           

        }
        
     
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }      

        void showPathTreeMain(List<string> PathList)
        {
            root = new MenuItem() { Title = "Список Папок" };           
            Regex regex = new Regex(@"(""\S+"")$");
           
            
                foreach (string str in PathList)
                {
                    string regStr = regex.Match(str).Value;
                    regStr = regStr.Replace(@"""", string.Empty);
                    newItemMenu = new MenuItem() { Title = regStr };                                
                    if (regStr != string.Empty) root.Items.Add(newItemMenu);
                    ShowExistAndRecentInPathTree(CoreMail.AddRequestAndWaitAnswer("SELECT " + regStr), newItemMenu);
                 }
            
                ShowCompleateTree();
        }

        void ShowExistAndRecentInPathTree(List<string> PathList ,MenuItem menuItem)
        {

            Regex regex1 = new Regex(@"EXISTS$");
            Regex regex3 = new Regex(@"\d+");

            foreach (string str in PathList)
            {
                if(regex1.IsMatch(str))
                {
                    string count = regex3.Match(str).Value;                    
                    MenuItem menuItemExist = new MenuItem() { Title = "Все " +  count , AnswersHeader =PathList};
                    menuItem.Items.Add(menuItemExist);
                    for (int i=1; i<=Int32.Parse(count); i++ )
                    {
                        MenuItem ActMenu=null;
                        ActMenu = ShowHeaderMail(CoreMail.AddRequestAndWaitAnswer(@"FETCH " + i.ToString() + " (FLAGS BODY[HEADER.FIELDS (FROM)])"), menuItemExist);

                        if (ActMenu != null) parseTextMessage(CoreMail.AddRequestAndWaitAnswer(@"FETCH " + i.ToString() + " BODY[TEXT]"),ActMenu, 
                            SearchBondary(CoreMail.AddRequestAndWaitAnswer(@"FETCH " + i.ToString() + " BODY[HEADER]")));
                    }

                }
             
            }

        }

        MenuItem ShowHeaderMail(List<string> MessageList , MenuItem ListWithMesseges)
        {
            Regex regex1 = new Regex(@"<\S+>");
            MenuItem itemMessage=null;
            for (int i=0; i<MessageList.Count; i++)
            {
                if (regex1.IsMatch(MessageList[i]))
                {
                    itemMessage = new MenuItem() { Title = regex1.Match(MessageList[i]).Value.Replace("<", "").Replace(">", ""), AnswersHeader = MessageList };
                    ListWithMesseges.Items.Add(itemMessage);
                }                              
            }
            return itemMessage;
        }

        public void parseTextMessage(List<string> answerList, MenuItem MessageItem, string boundary)
        {
            List<string> TExtBlockBody = new List<string>();
            string base64Str=string.Empty;
            bool openTag = false;
            foreach (string str in answerList)
            {
                if (str.Contains(boundary) && !str.Contains("boundary") && !openTag)
                {
                    openTag = true;
                    continue;
                }
                if(!str.Contains(boundary) && !str.Contains("boundary") && openTag)
                {
                    TExtBlockBody.Add(str);
                }
                if (str.Contains(boundary) && !str.Contains("boundary") && openTag)
                {
                    openTag = false;
                    string answer = GetEncodeString(TExtBlockBody);
                    if (answer != string.Empty)
                    {
                        MessageItem.MessageText = answer;
                        TExtBlockBody = new List<string>();
                    }
                }
            }
        }

        string GetEncodeString(List<string> listStr)
        {
            bool plainText = false;
            string base64 = string.Empty;
            string str8bit = string.Empty;
            for (int i=0; i<listStr.Count; i++)
            {
                if (listStr[i].Contains("text/plain"))
                {
                    plainText = true;
                }

                    if (listStr[i].Contains("base64") && plainText)
                {
                    for(int j=i+1; j<listStr.Count; j++ )
                    {
                        base64 += listStr[j];
                    }
                    break;
                }

                if (listStr[i].Contains("Encoding: 8bit") && plainText)
                {
                    for (int j = i + 1; j < listStr.Count; j++)
                    {
                        str8bit += listStr[j]+"\n";
                    }
                    break;
                }


            }
            if(base64!=string.Empty)
            {
                var base64Byte = System.Convert.FromBase64String(base64);
                return System.Text.Encoding.UTF8.GetString(base64Byte);
            }

            if(str8bit!=string.Empty)
            {
                return str8bit;
            }

            return string.Empty;
            //else { throw new Exception("Не найдена строка base64"); }
        }

        void ShowCompleateTree()
        {
            treePath.Dispatcher.BeginInvoke(new Action(() => { treePath.Items.Add(root); })) ;
        }

        private void TreePath_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MenuItem NowItem = e.NewValue as MenuItem;
            if (NowItem!=null)
            {                             
                MessageText.Text = NowItem.MessageText;                            
            }
           
        }

        string  SearchBondary(List<string> listBody)
        {
            Regex regex1 = new Regex(@"(?<=boundary=)\S+");
            Regex regex2 = new Regex(@"\w+");
            foreach (string str in listBody)
            {
                if(regex1.IsMatch(str))
                {
                    return regex2.Match(regex1.Match(str).Value).Value;
                   
                }              
            }
            return string.Empty;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if(Domain.Text!=string.Empty && Login.Text!=string.Empty && Password.Text!=string.Empty)
            {
                try { CoreMail = new CoreImap(Domain.Text, 993);
                    CoreMail.AddRequestAndWaitAnswer("login "+ Login.Text + " " + Password.Text);                   
                    connect.IsEnabled = false;
                    Domain.IsEnabled = false;
                    Password.IsEnabled = false;
                    Disconect.IsEnabled = true;
                    if(CoreMail.connectStatus==CoreImap.connectStatusEnum.connect)
                    {
                        StatusConn.Foreground = Brushes.Green;
                        StatusConn.Content = "Подключенно";
                        showPathTreeMain(CoreMail.AddRequestAndWaitAnswer(@"LIST """" ""*"""));
                    }
                }
                catch(Exception exc) { MessageBox.Show(exc.Message); }

            }

            else { MessageBox.Show("Введены не все данные"); }

        }

        private void Disconect_Click(object sender, RoutedEventArgs e)
        {
            connect.IsEnabled = true;
            Domain.IsEnabled = true;
            Password.IsEnabled = true;
            Disconect.IsEnabled = false;
            
             CoreMail.Disconnect(); 
            
            StatusConn.Foreground = Brushes.Red;
            StatusConn.Content = "Состояние подключения: Не подключенно";
            ClearFld();
        }

        void ClearFld()
        {
            newItemMenu = null;
            //treePath.ClearValue();
            terminal.Document.Blocks.Clear();
        }


    }

   

}
