using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K101FootbalAppFinal
{
    public partial class Form1 : Form
    {
        K101FootBallAppDBEntities db = new K101FootBallAppDBEntities();
        Stadion selectedStadion;
        Room selectedRoom;
        Time selectedTime;


        public Form1()
        {
            InitializeComponent();
        }
        private Worker actWork;
        public void UserInformation(Worker worker)
        {
            actWork = worker;

        }
        public void dtgAdminFill()
        {
            dtgAdmin.DataSource = db.Orders.Select(x => new
            {
                x.ID,
                x.CustomerName,
                x.CustomerSurname,
                x.CustomerPhone,
                x.Stadion.StadionName,
                x.Stadion.StadionPrice,
                x.Room.RoomName
            }).ToList();
        }
        public void FillStadionCombo()
        {
            cmbStadion.Items.AddRange(db.Stadions.Select(x => x.StadionName).ToArray());
        }
        public void FillStadionAdmin()
        {
            cmbAdminStadion.Items.AddRange(db.Stadions.Select(x => x.StadionName).ToArray());
        }
        public void FillShiftList()
        {
            cmbTime.Items.AddRange(db.Times.Select(x => x.Shifts).ToArray());
        }
        public void FillDataGrid()
        {
            dtgDataList.DataSource = db.Orders.Select(x => new
            {
                x.CustomerName,
                x.CustomerSurname,
                x.CustomerPhone,
                x.Stadion.StadionName,
                x.Stadion.StadionPrice,
                x.Room.RoomName,
                x.RezervDate
           }).ToList();
        }
        public void FillRooms()
        {
            cmbRooms.Items.AddRange(db.Rooms.Select(x => x.RoomName).ToArray());
        }
        public void FillRoomsAdmin()
        {
            cmbAdminRoom.Items.AddRange(db.Rooms.Select(x => x.RoomName).ToArray());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            Worker newLogin = db.Workers.FirstOrDefault(x => x.WorkerEmail == email);
            if (newLogin != null)
            {
                if (newLogin.WorkerPassword == password)
                {
                    this.WindowState = FormWindowState.Maximized;
                    loginPanel.Visible = false;
                    if (newLogin.Role.RoleName == "Admin")
                    {
                        UserInformation(newLogin);
                        adminName.Visible = true;
                        adminName.Text = newLogin.WorkerName;
                        workerPanel.Visible = false;
                        adminPanel.Visible = true;
                        WindowState = FormWindowState.Maximized;


                    }
                    else if (newLogin.Role.RoleName == "Seller")
                    {
                        UserInformation(newLogin);

                        workerName.Visible = true;
                        workerName.Text = "User";
                        adminPanel.Visible = false;
                        workerPanel.Visible = true;
                        WindowState = FormWindowState.Maximized;

                    }
                }
                else
                {
                    loginError.Visible = true;
                    lblError.Visible = true;
                    lblError.Text = "Email ve ya sifre sehfdi";
                }
            }
            else
            {
                loginError.Visible = true;
                lblError.Visible = true;
                lblError.Text = "Email ve ya sifre sehfdi";

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 300;
            this.Height = 350;
            FillStadionCombo();
            FillRooms();
            FillDataGrid();
            dtgAdminFill();
            FillStadionAdmin();
            FillRoomsAdmin();
            FillShiftList();
            Screen screen = Screen.FromControl(this);

            Rectangle workingArea = screen.WorkingArea;
            this.Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };

            FormBorderStyle = FormBorderStyle.None;
        }

        private void workerPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void cmbStadion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sta = cmbStadion.Text;
            selectedStadion = db.Stadions.First(x => x.StadionName == sta);
            cmbRooms.Enabled = true;
        }
        private void cmbRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            string rooms = cmbRooms.Text;
            selectedRoom = db.Rooms.FirstOrDefault(x => x.RoomName == rooms);
            dtgStadion.Enabled = true;

        }
        private void cmbTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            string times = cmbTime.Text;
            selectedTime = db.Times.FirstOrDefault(x => x.Shifts == times);
            rezervet.Enabled = true;



            /* Check date on DB */

            int stadions = selectedStadion.StadionID;
            int timess = selectedTime.TimesID;

            foreach (var item in db.Orders)
            {


                if (Convert.ToString(item.StadionID) == Convert.ToString(stadions))
                {
                    if (Convert.ToString(item.Time.TimesID) == Convert.ToString(timess))
                    {
                        if (Convert.ToString(item.RezervDate) == Convert.ToString(dtgStadion.Value))
                        {
                        
                            //MessageBox.Show("Bu tarixe meydan doludu");
                            label11.Visible = true;
                            label11.Text = "Bu tarixe meydan doludu";
                            rezervet.Visible = false;
                            return;
                        }
                    }
                    else
                    {
                        rezervet.Visible = true;
                        label11.Visible = false;
                    }

                }
                else
                {
                    rezervet.Visible = true;
                    label11.Visible = false;

                }

            }


        }
        private void button2_Click(object sender, EventArgs e)
        {
        /*  if (custumerName.Text == "" && custumerSurname.Text == "" && custumerPhone.Text == "")
            {
                MessageBox.Show("Butun xanalari doldurun");

            }
        */
            string Name = custumerName.Text;
            string Surname = custumerSurname.Text;
            string Phone = custumerPhone.Text;
            DateTime Rezervation = dtgStadion.Value;
            /*int Stadion = selectedStadion.StadionID;
            int Room = selectedRoom.RoomID;
            int Time = selectedTime.TimesID;*/

            string[] arr = new string[] { Name, Surname, Phone, Convert.ToString(Rezervation) };


            if (selectedStadion == null)
            {
                MessageBox.Show("Reqemler");
                return;
            }

            if (Utilities.IsEmpity(arr,string.Empty))
            {
                
                userError.Visible = false;
                userErrorText.Visible = false;

                Order newOrder = new Order();
                newOrder.WorkerID = actWork.WorkerID;
                newOrder.CustomerName = Name;
                newOrder.CustomerSurname = Surname;
                newOrder.CustomerPhone = Phone;

                newOrder.RezervDate = Rezervation;
                newOrder.StadionID = selectedStadion.StadionID;
                newOrder.RoomID = selectedRoom.RoomID;
                newOrder.TimeID = selectedTime.TimesID;

                db.Orders.Add(newOrder);
                db.SaveChanges();
                FillDataGrid();
                MessageBox.Show("Ugurlu oldu");

            }
            else
            {
                userError.Visible = true;
                userErrorText.Visible = true;
                userErrorText.Text = "Bütün xanaları doldurun";
            }
        }

        private void loginPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        Stadion newStadion;
        private void button3_Click(object sender, EventArgs e)
        {
            newStadion = new Stadion();
            newStadion.StadionName = cmbAdminStadion.Text;
            newStadion.StadionPrice = nmrStadionPrice.Text;
            db.Stadions.Add(newStadion);
            db.SaveChanges();
            MessageBox.Show("Ugurlu oldu");

        }
        string tesas;
        private void cmbAdminStadion_SelectedIndexChanged(object sender, EventArgs e)
        {

            sil.Visible = true;
            nmrStadionPrice.Visible = false;
            AdminMeydanElave.Visible = false;
            pricess.Visible = false;
            tesas = cmbAdminStadion.Text;
        }

        private void cmbAdminStadion_KeyDown(object sender, KeyEventArgs e)
        {
            pricess.Visible = true;
            AdminMeydanElave.Visible = true;
            nmrStadionPrice.Visible = true;
            sil.Visible = false;
        }

        private void cmbAdminRoom_KeyDown(object sender, KeyEventArgs e)
        {
            roomEdit.Visible = false;
            roomDelete.Visible = false;
        }

        private void cmbAdminRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            roomEdit.Visible = true;
            roomDelete.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
           
        }

        private void adminPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtgStadion_ValueChanged(object sender, EventArgs e)
        {
            cmbTime.Enabled = true;
        }

        private void sil_Click(object sender, EventArgs e)
        {
            Stadion asd = db.Stadions.First(x => x.StadionName == tesas);
            asd.Status = true;
            db.SaveChanges();
            FillStadionAdmin();
        }
        Order selecOrder;
        private void dtgAdmin_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            cmbCStadion.Items.AddRange(db.Stadions.Select(x => x.StadionName).ToArray());
            cmbCTime.Items.AddRange(db.Times.Select(x => x.Shifts).ToArray());
            cmbCRooms.Items.AddRange(db.Rooms.Select(x => x.RoomName).ToArray());

            int rezervID = (int)dtgAdmin.Rows[e.RowIndex].Cells[0].Value;
            selecOrder = db.Orders.First(x => x.ID == rezervID);
            txtCName.Text = selecOrder.CustomerName;
            txtCSurname.Text = selecOrder.CustomerSurname;
            txtCPhone.Text = selecOrder.CustomerPhone;
            dtgCRezerv.Value = (DateTime)selecOrder.RezervDate;
            cmbCStadion.Text = selecOrder.Stadion.StadionName;
            cmbCRooms.Text = selecOrder.Room.RoomName;
            cmbCTime.Text = selecOrder.Time.Shifts;
            dtgAdminFill();


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            selecOrder.CustomerName = txtCName.Text;
            selecOrder.CustomerSurname = txtCSurname.Text;
            selecOrder.CustomerPhone = txtCPhone.Text;
            selecOrder.RezervDate = dtgCRezerv.Value;
            int statID = db.Stadions.First(x => x.StadionName == cmbCStadion.Text).StadionID;
            selecOrder.StadionID = statID;
            int roomID = db.Rooms.First(x => x.RoomName == cmbCRooms.Text).RoomID;
            selecOrder.RoomID = roomID;
            int timeID = db.Times.First(x => x.Shifts == cmbCTime.Text).TimesID;
            selecOrder.TimeID = timeID;
            db.SaveChanges();
            MessageBox.Show("Alindiiiii");
            dtgAdminFill();
        }

        private void deleteItems_Click(object sender, EventArgs e)
        {
            db.Orders.Remove(selecOrder);
            db.SaveChanges();

            MessageBox.Show(Convert.ToString(selecOrder.ID));
            dtgAdminFill();

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Worker newSeller = new Worker();
            newSeller.WorkerName = newUserName.Text;
            newSeller.WorkerEmail = newUserEmail.Text;
            newSeller.WorkerPassword = newUserPassword.Text;
            newSeller.RoleID = 2;
            db.Workers.Add(newSeller);
            db.SaveChanges();
            MessageBox.Show("Qeydiyyatdan kecdi");
            newUserName.Text = "";
            newUserEmail.Text = "";
            newUserPassword.Text = "";
        }
    }
}
