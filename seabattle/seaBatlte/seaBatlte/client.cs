using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seaBatlteСlient
{
    public partial class ClientForm : Form
    {
        Bitmap b;
        int[] quantityShipTypeByMass = new int[4];
        string name;
        bool IsPlacementOverFlag = false;
        public string[] messageStringsSplitMass;
        int[] shot = new int[2];
        int[] hitTheTargetPoints = new int[2];
        int[] didNotHitPoints = new int[2];
        bool isBattleFlag = false;
        Field myBtField = new Field(50);
        Field enemyBtField = new Field(400);
        Field TemplateShipField = new Field(400, Cell.TypeOfCell.onlyShip);
        Field fieldOfArrangement = new Field(50,Cell.TypeOfCell.onlyShip);
        bool isConnectingNow = false;
        bool IsEnemyReadyFlag = false;
        bool amImovingFlag = false;
        Ship selectedShip;
        TcpClient client;       
        IPEndPoint receivePoint;
        int ShipSize = 1;
        System.Threading.Timer time;
        List<Cell> prevCell=new List<Cell>();
        List<Label> partsLabels = new List<Label>();
        Stack<Field> fields = new Stack<Field>();
        Ship.Orientation orientation = Ship.Orientation.horizontal;
        Thread thread;
        public ClientForm() 
        {
            init();
        }


        private void init()
        {
            Ship.loadingImageShip();
            quantityShipTypeByMass[0] = 4;
            quantityShipTypeByMass[1] = 3;
            quantityShipTypeByMass[2] = 2;
            quantityShipTypeByMass[3] = 1;
            InitializeComponent();
            receivePoint = new IPEndPoint(IPAddress.Any, 0);
            string addres = "127.0.0.1";
            int port = 8888;
            client = new TcpClient(addres,port);
            b = new Bitmap("fon.jpg");
            DoubleBuffered = true;
            test();
            if (HorisontalradioButton.Checked) orientation = Ship.Orientation.vertical;
            if (verticalradioButton.Checked) orientation = Ship.Orientation.horizontal;
            thread = new Thread(new ThreadStart(WaitForPackets));
            thread.Start();
            TemplateShip();
            isVisibleMainControls(true);
            isVisibleArrangementVisible(false);
            TimerCallback tm = new TimerCallback(updateForm);
            time = new System.Threading.Timer(tm, null, 0, 100);
            isVisibleArrangementVisible(false);
        }
        private void isVisibleMainControls(bool isVisible)
        {
            Invoke(new Action(() =>
            {
                waitlabel.Visible = isVisible;
                registrationToServerbutton.Visible = isVisible;
                letsStartButton.Visible = isVisible;
                nameTextBox.Visible = isVisible;
                shipOrientationComboBox.Visible = isVisible;
                playersListListBox.Visible = isVisible;
                label3.Visible = isVisible;
                label1.Visible = isVisible;
            }));
            
        }
        private void initButtomArrangement()
        {
            backButtom.Visible = false;
            HorisontalradioButton.BackColor = Color.Transparent;
            HorisontalradioButton.Font = new Font(HorisontalradioButton.Font.FontFamily, 10);
            HorisontalradioButton.ForeColor = Color.White;
            verticalradioButton.BackColor = Color.Transparent;
            verticalradioButton.ForeColor = Color.White;
            verticalradioButton.Font = new Font(verticalradioButton.Font.FontFamily, 10);
        }
        private void UpdateQuantityOfParts()
        {
            for(int i=0; i<partsLabels.Count; i++)
            {
                partsLabels[i].Text = quantityShipTypeByMass[i].ToString();
            }
        }
        private void initLabelsQuantityOfParts(Label label,int offsetCell,string text)
        {
            if(!this.IsHandleCreated)
            {
                this.CreateHandle(); 
            }
            Invoke(new Action(() =>
            {              
                label.BackColor = Color.Transparent;
                label.ForeColor = Color.White;
                label.AutoSize = true;
                label.Font = new Font(label.Font.FontFamily, 20);
                Point point = TemplateShipField.battleFiieldCells[0, offsetCell].Location;
                point.X -= 30;
                label.Location = point;
                label.Text = text;
            }));
            
        }
        public void test()
        {
            initButtomArrangement();
            playersListListBox.Visible = true;
            letsStartButton.Visible = true;
            registrationToServerbutton.Visible = true;           
            setButtomInGame();
           
        }
        private void updateForm(object o)
        {
            try
            {
                if (!this.IsHandleCreated)
                {
                    this.CreateHandle();
                }
;                Invoke(new Action(() => Invalidate()));
            }
            catch (Exception)
            {               
                Thread.Sleep(100);
                Invoke(new Action(() => Invalidate()));
            }
        }
        private void sendToServer(string H)
        {

            byte[] data = Encoding.UTF8.GetBytes(H);
            client.GetStream().Write(data,0, data.Length);
        }
        public void WaitForPackets()
        {
            try
            {
                int quantityOfKiledEnumyPartsHips = 0;
                while (true)
                {
                    // Получение массива байтов от сервера
                    byte[] data = new byte[1024];
                   int bytes= client.GetStream().Read(data,0,data.Length);
                    string MessageString = Encoding.UTF8.GetString(data,0,bytes);
                    messageStringsSplitMass = MessageString.Split(',');
                    if (messageStringsSplitMass[0] == "playersList")
                    {
                        playersListListBox.Invoke((MethodInvoker)(() => playersListListBox.Items.Clear()));
                        for(int i=1; i<messageStringsSplitMass.Length; i++)
                        {
                            if (nameTextBox.Text != messageStringsSplitMass[i]) {
                                Invoke(new Action(() => playersListListBox.Items.Add(messageStringsSplitMass[i])));

                            }
                        }
                       
                    }
                    if(MessageString == "disconect")
                    {
                        Invoke(new Action(() => { Restart();
                            disconnectlabel.Visible = true;
                        }
                        )) ;                        
                    }


                    if (MessageString == "connect" && !isConnectingNow)
                    {
                        isConnectingNow = true;
                        string H = "connect";
                        sendToServer(H);
                        MessageBox.Show("Соединение с соперником установлено начинайте расстановку");
                        isVisibleMainControls(false);
                        isVisibleArrangementVisible(true);
                        Invoke(new Action(() => this.Size = (new Size(680, 430)))); 
                    }
                    if (MessageString == "end")
                    {
                        MessageBox.Show("Противник закончил расстановку и готов играть");                     
                        IsEnemyReadyFlag = true;
                        Invoke(new Action(() => this.Size = (new Size(730, 430))));
                    }
                    if (messageStringsSplitMass[0] == "shot")
                    {
                        shot[0] = Convert.ToInt32(messageStringsSplitMass[1]);
                        shot[1] = Convert.ToInt32(messageStringsSplitMass[2]);
                        CheckTheEnemyShot();
                    }
                    if (messageStringsSplitMass[0] == "HitTheTargetCongratulations!")
                    {
                        hitTheTargetPoints[0] = Convert.ToInt32(messageStringsSplitMass[1]);
                        hitTheTargetPoints[1] = Convert.ToInt32(messageStringsSplitMass[2]);
                        enemyBtField.battleFiieldCells[hitTheTargetPoints[0], hitTheTargetPoints[1]].type = Cell.TypeOfCell.explosionEnemy;
                        quantityOfKiledEnumyPartsHips++;
                        if (quantityOfKiledEnumyPartsHips > 19)
                        {
                            MessageBox.Show("Поздравляем, Вы победили");
                            Close();
                        }
                    }
                    if (messageStringsSplitMass[0] == "youDidNotHit")
                    {
                        didNotHitPoints[0] = Convert.ToInt32(messageStringsSplitMass[1]);
                        didNotHitPoints[1] = Convert.ToInt32(messageStringsSplitMass[2]);
                        enemyBtField.battleFiieldCells[didNotHitPoints[0], didNotHitPoints[1]].type = Cell.TypeOfCell.miss;
                        amImovingFlag = false;
                        string H = "move" + "," + amImovingFlag;
                        sendToServer(H);
                    }
                    if (messageStringsSplitMass[0] == "move")
                    {
                        IsEnemyReadyFlag = true;
                        amImovingFlag = !Convert.ToBoolean(messageStringsSplitMass[1]);
                    }

                }
            }
            catch (OutOfMemoryException e)
            {
                MessageBox.Show(e.Message);
            }

        }
        private void setButtomInGame()
        {
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string H = nameTextBox.Text;
            sendToServer(H);
            nameTextBox.Invoke(new Action(() => nameTextBox.Clear()));

        }
        public bool reduceTheNumberOfAvalibleShip(int length)
        {
            if (quantityShipTypeByMass[length - 1] > 0)
            {
                quantityShipTypeByMass[length - 1]--;
                UpdateQuantityOfParts();
                return true;
            }
            else
            {
                MessageBox.Show("Такие корабли закончились");
                return false;
            }
        }
        public bool shipsAreNotPlaced()
        {
            if (quantityShipTypeByMass[0] == quantityShipTypeByMass[1] && quantityShipTypeByMass[0] == quantityShipTypeByMass[2] && quantityShipTypeByMass[0]
                == quantityShipTypeByMass[3] & quantityShipTypeByMass[0] == 0)
            {
                startBattle();
                return false;
            }
            return true;
        }
        private void Client_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!IsPlacementOverFlag)
                {
                    if (shipsAreNotPlaced())
                    {
                        if (myBtField.returnClickPoint(e.X, e.Y)[0] != -1)
                        {
                            Ship ship = new Ship(ShipSize);
                            if (placedInTheField(ShipSize, ship, e))
                            {                                
                                if (orientation==Ship.Orientation.vertical)
                                {
                                    initFieldsAroundTheShipVertical(ShipSize, ship, e);
                                }
                                else
                                {
                                    initFieldsAroundTheShipHorizontal(ShipSize, ship, e);
                                }
                            }
                        }
                        Ship selectedShip = getShipByMouse(e, TemplateShipField);
                        if (selectedShip != null)
                        {
                            if (this.selectedShip != null) this.selectedShip.setSelected(false);
                            this.selectedShip = selectedShip;
                            ShipSize = selectedShip.quantityOfPartShip;
                            selectedShip.setSelected(true);
                        }
                        shipsAreNotPlaced();
                    }

                    else
                    {
                        MessageBox.Show("все корабли расставлены");
                    }

                }
                if (isBattleFlag)
                {
                    myShot(e);
                }
            }
            catch (OutOfMemoryException ex)
            {
                throw;
                //MessageBox.Show(ex.Message);
            }
        }
        private Ship getShipByMouse(MouseEventArgs e, Field field)
        {
            Cell cell = field.returnCellByMouse(e);
            if (cell != null)
            {
                foreach (Ship ship in field.listShips)
                {
                    Ship secectedShip = ship.checkCellInTheShip(cell);
                    if (secectedShip != null)
                    {
                        return secectedShip;
                    }
                }
            }
            return null;
        }
        private void myShot(MouseEventArgs e)
        {
            if (amImovingFlag && IsEnemyReadyFlag)
            {
                if (enemyBtField.returnClickPoint(e.X, e.Y)[0] != -1)
                {
                    string H = "shot" + "," + enemyBtField.returnClickPoint(e.X, e.Y)[0] + "," + enemyBtField.returnClickPoint(e.X, e.Y)[1];
                    sendToServer(H);
                }
            }

        }
        private void checkLose()
        {
            int quantityKiledEnemyShips = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (myBtField.battleFiieldCells[i, j].type == Cell.TypeOfCell.explosion)
                    {
                        quantityKiledEnemyShips++;
                    }
                    if (quantityKiledEnemyShips > 19)
                    {
                        MessageBox.Show("Вы проиграли");
                        Invoke(new Action(() => this.Close()));
                    }
                }
            }
        }
        private void initFieldsAroundTheShipVertical(int ShipSize, Ship ship, MouseEventArgs e)
        {
            int x = 2;
            if (myBtField.returnClickPoint(e.X, e.Y)[1] + ShipSize > 9)
            {
                x = 1;
            }
            if (myBtField.returnClickPoint(e.X, e.Y)[1] + ShipSize < 11)
            {
                if (reduceTheNumberOfAvalibleShip(ShipSize))
                {
                    for (int i = 0; i < ShipSize + x; i++)
                    {
                        if (myBtField.returnClickPoint(e.X, e.Y)[0] - 1 > -1 && myBtField.returnClickPoint(e.X, e.Y)[1] + i - 1 > -1)
                        {
                            ship.setAsAFieldNearShip(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0] - 1, myBtField.returnClickPoint(e.X, e.Y)[1] + i - 1]);
                        }
                        if (myBtField.returnClickPoint(e.X, e.Y)[1] + i - 1 > -1)
                        {
                            ship.setAsAFieldNearShip(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0], myBtField.returnClickPoint(e.X, e.Y)[1] + i - 1]);
                        }
                        if (myBtField.returnClickPoint(e.X, e.Y)[0] + 1 < 10 && myBtField.returnClickPoint(e.X, e.Y)[1] + i - 1 > -1)
                        {
                            ship.setAsAFieldNearShip(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0] + 1, myBtField.returnClickPoint(e.X, e.Y)[1] + i - 1]);
                        }
                    }
                    List<Cell> cells = new List<Cell>();
                    for (int i = 0; i < ShipSize; i++)
                    {
                        cells.Add(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0], myBtField.returnClickPoint(e.X, e.Y)[1] + i]);
                       
                    }
                    ship.createShipInField(cells,Ship.Orientation.vertical,myBtField);
                }
            }
            else
            {
                MessageBox.Show("корабль выходит за границу поля");
            }
        }    
        private void initFieldsAroundTheShipHorizontal(int ShipSize, Ship ship, MouseEventArgs e)
        {
            int x = 2;
            if (myBtField.returnClickPoint(e.X, e.Y)[0] + ShipSize > 9)
            {
                x = 1;
            }
            if (myBtField.returnClickPoint(e.X, e.Y)[0] + ShipSize < 11)
            {
                if (reduceTheNumberOfAvalibleShip(ShipSize))
                {
                    for (int i = 0; i < ShipSize + x; i++)
                    {

                        if (myBtField.returnClickPoint(e.X, e.Y)[0] + i - 1 > -1)
                        {
                            ship.setAsAFieldNearShip(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0] + i - 1, myBtField.returnClickPoint(e.X, e.Y)[1]]);
                        }
                        if (myBtField.returnClickPoint(e.X, e.Y)[0] + i - 1 > -1 && myBtField.returnClickPoint(e.X, e.Y)[1] - 1 > -1)
                        {
                            ship.setAsAFieldNearShip(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0] + i - 1, myBtField.returnClickPoint(e.X, e.Y)[1] - 1]);
                        }
                        if (myBtField.returnClickPoint(e.X, e.Y)[0] + i - 1 > -1 && myBtField.returnClickPoint(e.X, e.Y)[1] + 1 < 10)
                        {
                            ship.setAsAFieldNearShip(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0] + i - 1, myBtField.returnClickPoint(e.X, e.Y)[1] + 1]);
                        }
                    }
                    List<Cell> cells = new List<Cell>();
                    for (int i = 0; i < ShipSize; i++)
                    {
                        cells.Add(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0] + i, myBtField.returnClickPoint(e.X, e.Y)[1]]);                       
                    }
                    ship.createShipInField(cells, Ship.Orientation.horizontal,myBtField);
                }
            }
            else
            {
                MessageBox.Show("корабль выходит за границу поля");
            }
        }
        private bool placedInTheField(int ShipSize, Ship ship, MouseEventArgs e)
        {
            bool ok = true;
            for (int i = 0; i < ShipSize; i++)
            {
                if (myBtField.returnClickPoint(e.X, e.Y)[1] + ShipSize < 11)
                {
                    if (orientation==Ship.Orientation.vertical && !ship.doesNotInterfereWithTheShip(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0],
                        myBtField.returnClickPoint(e.X, e.Y)[1] + i]))
                    {
                        ok = false;
                        break;
                    }
                }
                if (myBtField.returnClickPoint(e.X, e.Y)[0] + ShipSize < 11)
                {
                    if (orientation == Ship.Orientation.horizontal && !ship.doesNotInterfereWithTheShip(myBtField.battleFiieldCells[myBtField.returnClickPoint(e.X, e.Y)[0] + i,
                        myBtField.returnClickPoint(e.X, e.Y)[1]]))
                    {
                        ok = false;
                        break;
                    }
                }
            }
             if(ok) fields.Push((Field)myBtField.Clone());
            return ok;
        }
        private void Client_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(b, new Point(0, 0));
            if (isConnectingNow)
            {
                if (!IsPlacementOverFlag)
                {
                    TemplateShipField.Paint(e.Graphics);
                    myBtField.Paint(e.Graphics);
                    fieldOfArrangement.Paint(e.Graphics);
                }

                if (IsPlacementOverFlag)
                {                  
                    enemyBtField.Paint(e.Graphics);
                   myBtField.Paint(e.Graphics);
                   
                }

            }
            else
            {
                shipOrientationComboBox.Visible = false;
            }
            if (amImovingFlag)
            {
                Text = nameTextBox.Text + " Ваш ход";
            }
            else
            {
                Text = nameTextBox.Text;
            }
        }
        private void startBattle()
        {                    
                isBattleFlag = true;
                IsPlacementOverFlag = true;
                string H = "end";
                sendToServer(H);
            isVisibleArrangementVisible(false);
            if (!amImovingFlag)
                {
                    H = "move" + "," + amImovingFlag;
                    sendToServer(H);
                }
            myBtField.setCellAroundShipAnVisible();
            isVisibleArrangementVisible(false);
        }
        private void isVisibleArrangementVisible(bool isVisible)
        {
            Action setC = new Action(() =>
            {
                foreach (Label l in partsLabels)
                {
                    l.Visible = isVisible;
                }
                verticalradioButton.Visible = isVisible;
                HorisontalradioButton.Visible = isVisible;
            });

            BeginInvoke(setC);
        }      
        private void CheckTheEnemyShot()
        {
            bool didIHit = false;
            for (int i = 0; i < 10; i++)
            {
                if (myBtField.listShips[i].hitCheck(myBtField.battleFiieldCells[shot[0], shot[1]].Location) == 0)
                {
                    didIHit = true;
                }
            }
            if (didIHit)
            {
                string H = "HitTheTargetCongratulations!" + "," + shot[0] + "," + shot[1];

                myBtField.battleFiieldCells[shot[0], shot[1]].type = Cell.TypeOfCell.explosion;
                sendToServer(H);
                checkLose();
            }
            else
            {
                myBtField.battleFiieldCells[shot[0], shot[1]].type = Cell.TypeOfCell.miss;
                string H = "youDidNotHit" + "," + shot[0] + "," + shot[1];
                sendToServer(H);
            }
        }
        private void regToServer_Click(object sender, EventArgs e)
        {
             name = nameTextBox.Text;
            if (nameTextBox.Text != "")
            {
                string H = "name" + "," + nameTextBox.Text;
                sendToServer(H);
                label3.Visible = false;
                nameTextBox.Visible = false;
                registrationToServerbutton.Visible = false;
                Text = nameTextBox.Text;
                Namelabel.Visible = false;
                waitlabel.Visible = true;
            }
            else
            {
                MessageBox.Show("Введите имя");
            }

        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (playersListListBox.SelectedIndex != -1)
            {
                string H = "choice" + "," + playersListListBox.Items[playersListListBox.SelectedIndex];
                sendToServer(H);
                playersListListBox.Visible = false;
                letsStartButton.Visible = false;
            }
        }
        private void TemplateShip()
        {
            int k = 0;
            for (int i = 1; i < 5; i++)
            {               
                Ship ship = new Ship(i);
                Label label = new Label();
                partsLabels.Add(label);
                Controls.Add(label);
                List<Cell> cells = new List<Cell>();
                for (int j = 0; j < i; j++)
                {                  
                    initLabelsQuantityOfParts(label, i + k, quantityShipTypeByMass[i - 1].ToString());
                    cells.Add(TemplateShipField.battleFiieldCells[j, i + k]);                  
                }
                ship.createShipInField(cells, Ship.Orientation.horizontal, TemplateShipField, false);
                k++;
            }       
}
        private void Client_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isBattleFlag)
            {
                foreach (Cell c in prevCell)
                {
                    c.type = Cell.TypeOfCell.water;
                }

                Ship movingShip = new Ship(ShipSize);

                int[] pointsInField = fieldOfArrangement.returnClickPoint(e.X, e.Y);
                if (pointsInField[0] != -1)
                {
                    if (orientation == Ship.Orientation.vertical)
                    {
                        for (int i = 0; i < movingShip.quantityOfPartShip; i++)
                        {
                            if (pointsInField[1] + i < 10)
                            {
                                Cell cell = fieldOfArrangement.battleFiieldCells[pointsInField[0], pointsInField[1] + i];
                                movingShip.createShipInField(cell, Ship.Orientation.vertical,fieldOfArrangement);
                                prevCell.Add(cell);
                            }
                        }
                    }
                    if (orientation == Ship.Orientation.horizontal)
                    {
                        for (int i = 0; i < movingShip.quantityOfPartShip; i++)
                        {
                            if (pointsInField[0] + i < 10)
                            {
                                Cell cell = fieldOfArrangement.battleFiieldCells[pointsInField[0] + i, pointsInField[1]];
                                movingShip.createShipInField(cell, Ship.Orientation.horizontal,fieldOfArrangement);
                                prevCell.Add(cell);
                            }
                        }
                    }
                } 
            }
        }
        private void VerticalradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked) orientation = Ship.Orientation.vertical;
        }
        private void HorizontalradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked) orientation = Ship.Orientation.horizontal;
        }
        private void BackButtom_Click(object sender, EventArgs e)
        {
           if(fields.Count!=0) myBtField = fields.Pop();
        }    
        private void ClientForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            sendToServer("close");
            thread.Abort();
            client.Dispose();          
            Environment.Exit(Environment.ExitCode);
        }
        private void Restart()
        {         
         quantityShipTypeByMass = new int[4];
         IsPlacementOverFlag = false;
         shot = new int[2];
         hitTheTargetPoints = new int[2];
         didNotHitPoints = new int[2];
         isBattleFlag = false;
         myBtField = new Field(50);
         enemyBtField = new Field(400);
         TemplateShipField = new Field(400, Cell.TypeOfCell.onlyShip);
         fieldOfArrangement = new Field(50, Cell.TypeOfCell.onlyShip);
         isConnectingNow = false;
         IsEnemyReadyFlag = false;
         amImovingFlag = false;       
         ShipSize = 1;
         prevCell = new List<Cell>();
         partsLabels = new List<Label>();
         fields = new Stack<Field>();
         orientation = Ship.Orientation.horizontal;
         init();
            nameTextBox.Text = name;
            regToServer_Click(null, null);
            isVisibleArrangementVisible(false);
            this.Size = (new Size(330,310));
        }

    }


    public class Field : ICloneable
    {
        int offset; 
        public Cell[,] battleFiieldCells = new Cell[10, 10];      
        public int storona = 30;
        public int Razm;
        public List<Ship> listShips { get; private set; } = new List<Ship>();

        public Field(int offset)
        {
            this.offset = offset;
            for (int i = 0; i < 10; i++)
            {               
                for (int j = 0; j < 10; j++)
                {
                    battleFiieldCells[i, j] = new Cell();
                    battleFiieldCells[i, j].Location = new Point(i * 30 + offset, j * 30 + 50);
                    battleFiieldCells[i, j].ImageWater = new Bitmap("waterSprites\\" + j.ToString() + i.ToString() + ".png");

                }
            }
         
        }

        public Field(int offset, Cell.TypeOfCell type)
        {
            this.offset = offset;
            for (int i = 0; i < 10; i++)
            {

                for (int j = 0; j < 10; j++)
                {
                    battleFiieldCells[i, j] = new Cell();
                    battleFiieldCells[i, j].type = type;
                    battleFiieldCells[i, j].Location = new Point(i * 30 + offset, j * 30 + 50);

                }
            }
        }

        public void Paint(Graphics gr)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    battleFiieldCells[i, j].Paint(gr);
                }
            }
        }

        public int[] returnClickPoint(int xr, int yr)
        {
            int[] ia = new int[2];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (battleFiieldCells[i, j].Contains(new Point(xr, yr)))
                    {
                        ia[0] = i;
                        ia[1] = j;
                        return ia;
                    }
                }
            }
            ia[0] = -1;
            ia[1] = -1;
            return ia;
        }

        public Cell returnCellByMouse(MouseEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (battleFiieldCells[i, j].Contains(new Point(e.X, e.Y)))
                    {
                        return battleFiieldCells[i, j];
                    }
                }
            }
            return null;
        }

        public object Clone()
        {
            Field field = new Field(offset);
            for(int i=0; i<10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    field.battleFiieldCells[i, j].type = this.battleFiieldCells[i, j].type;
                    field.battleFiieldCells[i, j].ImageShip= this.battleFiieldCells[i, j].ImageShip;
                    field.battleFiieldCells[i, j].ImageWater = this.battleFiieldCells[i, j].ImageWater;
                }
            }
            field.Razm = this.Razm;
            field.storona = this.storona;
            return field;
        }

        public void setCellAroundShipAnVisible()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    battleFiieldCells[i, j].showCellIfCellIsArroundAShip = false;
                }
            }
        }
    }
    public class Ship
    {
      static  List<List<Bitmap>> shipsBitmapsVertical = new List<List<Bitmap>>();
        static List<List<Bitmap>> shipsBitmapsHorizontal = new List<List<Bitmap>>();
        public int quantityOfPartShip;
        int hitCount;
        public bool kill;
        public int ub;
        bool selected = false;
        public enum Orientation { vertical, horizontal };
        Orientation orientation;
        List<Cell> cellsOfTheShipList = new List<Cell>();
        public Ship(int quantityOfPartShip)
        {
            this.quantityOfPartShip = quantityOfPartShip;
            kill = false;

        }   
        private void initSpriteShip()
        {
            for (int i = 0; i <= quantityOfPartShip - 1; i++)
            {
                if(i<cellsOfTheShipList.Count)
                {                   
                    if (orientation == Orientation.vertical) {
                        if (!selected)
                        {
                            Bitmap bm = shipsBitmapsVertical[quantityOfPartShip - 1][i];
                            cellsOfTheShipList[i].ImageShip = bm;
                        }

                        if (selected)
                        {
                            Bitmap bm = shipsBitmapsVertical[quantityOfPartShip - 1][cellsOfTheShipList.Count + i];
                            cellsOfTheShipList[i].ImageShip = bm;
                        }
                    }

                    if (orientation == Orientation.horizontal)
                    {
                        if (!selected)
                        {
                            Bitmap bm = shipsBitmapsHorizontal[quantityOfPartShip - 1][i];
                            cellsOfTheShipList[i].ImageShip = bm;
                        }

                        if (selected)
                        {                        
                         Bitmap bm = shipsBitmapsHorizontal[quantityOfPartShip - 1][cellsOfTheShipList.Count+i];
                         cellsOfTheShipList[i].ImageShip = bm;                            
                        }
                    }
                }
            }
        }
        public static void loadingImageShip()
        {
            for (int i = 1; i < 5; i++)
            {
                List<Bitmap> bitmapsVertical = new List<Bitmap>();
                List<Bitmap> bitmapsHorizontal = new List<Bitmap>();
                for (int j=1; j<=i*2; j++)
                {
                    if (i == 0) {                        
                        Bitmap f = new Bitmap("ship" + i + "health\\" + j + ".png");
                        Bitmap s = new Bitmap("ship" + i + "health\\" + j + 1 + ".png");
                        Bitmap fr = new Bitmap("ship" + i + "health\\" + j + ".png");
                        Bitmap sr = new Bitmap("ship" + i + "health\\" + j + 1 + ".png");
                        bitmapsVertical.Add(f);
                        bitmapsVertical.Add(s);
                        fr.RotateFlip(RotateFlipType.Rotate90FlipXY);
                        sr.RotateFlip(RotateFlipType.Rotate90FlipXY);
                        bitmapsHorizontal.Add(fr);
                        bitmapsHorizontal.Add(sr);
                    }
                    Bitmap bm = new Bitmap("ship" + i + "health\\" + (j - 1) + ".png");
                    Bitmap bmr = new Bitmap("ship" + i + "health\\" + (j - 1) + ".png");
                    bitmapsVertical.Add(bm);
                    bm.RotateFlip(RotateFlipType.Rotate90FlipXY);
                    bitmapsHorizontal.Add(bmr);

                }
                shipsBitmapsVertical.Add(bitmapsHorizontal);
                shipsBitmapsHorizontal.Add(bitmapsVertical);
            }
        }
        public void setSelected(bool isSelect)
        {
            if (isSelect == selected) return;
            if (isSelect)
            {
                selected = true;
                initSpriteShip();
            }

            if (!isSelect)
            {
                selected = false;
                initSpriteShip();
            }
        }
        public Ship checkCellInTheShip(Cell cell)
        {
            if (cellsOfTheShipList.Contains(cell))
            {
                return this;
            }
            return null;
        }
        public void createShipInField(List<Cell>cells, Ship.Orientation orientation,Field field, bool setTypeShip = true)
        {
            this.orientation = orientation;
            cellsOfTheShipList.AddRange(cells);
            if (setTypeShip) {
                foreach(Cell c in cells)
                {
                    c.type = Cell.TypeOfCell.ship;
                }
            }
            initSpriteShip();
           field.listShips.Add(this);
        }

        public void createShipInField(Cell cell, Ship.Orientation orientation, Field field, bool setTypeShip = true)
        {
            this.orientation = orientation;
            cellsOfTheShipList.Add(cell);
            if (setTypeShip)
            {  cell.type = Cell.TypeOfCell.ship;               
            }
            initSpriteShip();
            field.listShips.Add(this);
        }
        public void setAsAFieldNearShip(Cell cell)
        {
            if (cell != null)
            {
                cell.type = Cell.TypeOfCell.fieldNearShip;
            }
        }
        public bool doesNotInterfereWithTheShip(Cell cell)
        {
            if (cell.type != Cell.TypeOfCell.ship && cell.type != Cell.TypeOfCell.fieldNearShip)
            {
                return true;
            }
            else
            {
                MessageBox.Show("сюда нельзя");
                return false;
            }
        }
        public int hitCheck(Point point)
        {

            for (int i = 0; i < cellsOfTheShipList.Count; i++)
            {
                if (cellsOfTheShipList[i].Contains(point))
                {
                    hitCount++;
                    return 0;
                }
            }
            if (hitCount == cellsOfTheShipList.Count)
            {
                for (int j = 0; j < cellsOfTheShipList.Count; j++)
                {
                    kill = true;
                    ub++;                   
                }
            }
            return 1;
        }

      
    }
    public class Cell
    {
        public bool showCellIfCellIsArroundAShip = true;
        bool wasExplosion = false;
        public enum TypeOfCell { ship, water, fieldNearShip, explosion, miss, explosionEnemy, onlyShip }
        public TypeOfCell type { get; set; }
        public Point Location; 
        int size;
        Rectangle rectangle;
        static Bitmap missImage;
        public Bitmap ImageWater { get; set; }
        public Bitmap ImageShip { get; set; }
        static BlockingCollection<Bitmap> explosionAnimationsList;
        static BlockingCollection<Bitmap> fireAnimationsList;
        static BlockingCollection<Bitmap> waterFireAnimationsList;
        int lastIndex;
        static Cell()
        {
            missImage = new Bitmap("crosssprite\\0.png");
            fireAnimationsList = initFireAnimations();
            explosionAnimationsList = initExplosionAnimations();
            waterFireAnimationsList = initFireWaterAnimations();
        }
        public Cell()
        {
            type = TypeOfCell.water;
            Location = new Point(20, 20);
            size = 30;
            rectangle = new Rectangle(Location.X, Location.Y, size, size);
        }
        public void Paint(Graphics graphics)
        {
            rectangle = new Rectangle(Location.X, Location.Y, size, size);
            if (type == TypeOfCell.water)
            {
                if (ImageWater != null) graphics.DrawImage(ImageWater, rectangle);

            }
            if (type == TypeOfCell.ship)
            {
                if (ImageWater != null) graphics.DrawImage(ImageWater, rectangle);
                if (ImageShip != null) graphics.DrawImage(ImageShip, rectangle);

            }
            if (type == TypeOfCell.fieldNearShip)
            {
                if (showCellIfCellIsArroundAShip)
                {
                    graphics.DrawImage(ImageWater, rectangle);
                    graphics.DrawImage(missImage, rectangle);
                }
                else
                {
                    graphics.DrawImage(ImageWater, rectangle);
                }
            }
            if (type == TypeOfCell.explosionEnemy)
            {
                if (ImageWater != null) graphics.DrawImage(ImageWater, rectangle);
                if (!wasExplosion)
                {
                    Bitmap bitmap = makeAnimation(explosionAnimationsList, ref wasExplosion);
                    if (bitmap != null) graphics.DrawImage(bitmap, rectangle);
                }
                if (wasExplosion) graphics.DrawImage(makeAnimationWithReapeat(fireAnimationsList), rectangle);
            }
            if (type == TypeOfCell.explosion)
            {
                if (ImageWater != null) graphics.DrawImage(ImageWater, rectangle);
                if (ImageShip != null) graphics.DrawImage(ImageShip, rectangle);
                if (!wasExplosion)
                {
                    Bitmap bitmap = makeAnimation(explosionAnimationsList, ref wasExplosion);
                    if (bitmap != null) graphics.DrawImage(bitmap, rectangle);
                }
                if (wasExplosion) graphics.DrawImage(makeAnimationWithReapeat(fireAnimationsList), rectangle);
            }
            if (type == TypeOfCell.miss)
            {
                if (ImageWater != null) graphics.DrawImage(ImageWater, rectangle);
                if (ImageShip != null) graphics.DrawImage(ImageShip, rectangle);
                if (!wasExplosion)
                {
                    Bitmap bitmap = makeAnimation(waterFireAnimationsList, ref wasExplosion);
                    if (bitmap != null) graphics.DrawImage(bitmap, rectangle);
                }
                if (wasExplosion) graphics.DrawImage(missImage, rectangle);
            }
            if (type == TypeOfCell.onlyShip)
            {
                if (ImageShip != null) graphics.DrawImage(ImageShip, rectangle);
            }

            if (type != TypeOfCell.onlyShip) graphics.DrawRectangle(Pens.Black, rectangle);


        }
        private Bitmap makeAnimation(BlockingCollection<Bitmap> pathImageList, ref bool animationCompleate)
        {
            Bitmap bm = null;
            if (pathImageList.Count != 0 && lastIndex > pathImageList.Count - 1)
            {
                animationCompleate = true;
            }
            else
            {
                bm = pathImageList.ElementAt(lastIndex);
            }
            lastIndex++;
            return bm;
        }
        private Bitmap makeAnimationWithReapeat(BlockingCollection<Bitmap> pathImageList)
        {
            Bitmap bm = null;
            if (pathImageList.Count != 0 && lastIndex > pathImageList.Count - 1)
            {
                lastIndex = 0;
                bm = pathImageList.ElementAt(lastIndex);
            }
            else
            {
                bm = pathImageList.ElementAt(lastIndex);
            }
            lastIndex++;
            return bm;

        }
        public bool Contains(Point p)
        {
            if (rectangle.Contains(p))
            {
                return true;
            }
            return false;
        }
        private static BlockingCollection<Bitmap> initExplosionAnimations()
        {
            string path = "explosion\\0.png";
            BlockingCollection<Bitmap> cutParts = new BlockingCollection<Bitmap>();
            Bitmap bm = new Bitmap(path);
            for (int i = 0; i < bm.Height - (bm.Height / 4) * 2; i += bm.Height / 4)
            {
                Bitmap cutHeight = bm.Clone(new Rectangle(0, i, bm.Width, bm.Height / 4), bm.PixelFormat);
                for (int j = 0; j < cutHeight.Width; j += cutHeight.Width / 8)
                {
                    Bitmap tBm = cutHeight.Clone(new Rectangle(j, 0, cutHeight.Width / 8, cutHeight.Height), cutHeight.PixelFormat);
                    cutParts.Add(tBm.Clone(new Rectangle(25, 25, 75, 75), tBm.PixelFormat));
                }

            }

            return cutParts;
        }
        private static BlockingCollection<Bitmap> initFireAnimations()
        {
            string path = "fireSprites\\0.png";
            BlockingCollection<Bitmap> cutParts = new BlockingCollection<Bitmap>();
            Bitmap bm = new Bitmap(path);
            for (int i = 0; i < bm.Height; i += bm.Height / 4)
            {
                Bitmap cutHeight = bm.Clone(new Rectangle(0, i, bm.Width, bm.Height / 4), bm.PixelFormat);
                for (int j = 0; j < cutHeight.Width; j += cutHeight.Width / 4)
                {
                    Bitmap tBm = cutHeight.Clone(new Rectangle(j, 0, cutHeight.Width / 4, cutHeight.Height), cutHeight.PixelFormat);
                    cutParts.Add(tBm.Clone(new Rectangle(15, 15, 100, 100), tBm.PixelFormat));
                }

            }

            return cutParts;
        }
        private static BlockingCollection<Bitmap> initFireWaterAnimations()
        {
            string path = "waterFireSprites\\0.png";
            BlockingCollection<Bitmap> cutParts = new BlockingCollection<Bitmap>();
            Bitmap bm = new Bitmap(path);
            for (int i = 0; i < bm.Height; i += bm.Height / 4)
            {
                Bitmap cutHeight = bm.Clone(new Rectangle(0, i, bm.Width, bm.Height / 4), bm.PixelFormat);
                for (int j = 0; j < cutHeight.Width; j += cutHeight.Width / 4)
                {
                    Bitmap tBm = cutHeight.Clone(new Rectangle(j, 0, cutHeight.Width / 4, cutHeight.Height), cutHeight.PixelFormat);
                    cutParts.Add(tBm.Clone(new Rectangle(25, 25, 300, 300), tBm.PixelFormat));
                }

            }

            return cutParts;
        }
    }

  
}
