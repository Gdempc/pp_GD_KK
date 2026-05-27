using pp_GD_KK.Properties;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace pp_GD_KK
{
    public partial class UserControl4 : UserControl
    {

        

        public UserControl4()
        {
            InitializeComponent();
        }

        private void UserControl4_Load(object sender, EventArgs e)
        {

            string server = "localhost";
            string database = "wydarzeniastudenckie"; 
            string uid = "root";         
            string password = "";        

            string connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};";
            string query = "SELECT ID, Title, Description, Image FROM ogloszenia";


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                

                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string title = reader.GetString("Title");
                        string description = reader.GetString("Description");

                        Image image = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                        {
                            byte[] imageBytes = (byte[])reader["Image"];
                            image = Image.FromStream(new MemoryStream(imageBytes));
                        }

                        FlowLayoutPanel p = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 180, BorderStyle = BorderStyle.FixedSingle };
                        p.Width = flowLayoutPanel1.Width - 30;
                        PictureBox pictureBox = new PictureBox { Height = p.Height - 10, Width = p.Height - 10, Image = image, SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.White };
                        RichTextBox txt = new RichTextBox { Text = description, Width = p.Width - pictureBox.Width - 15, Height = p.Height - 10, Enabled = false, BorderStyle = BorderStyle.FixedSingle };
                        p.Controls.Add(pictureBox);
                        p.Controls.Add(txt);
                        flowLayoutPanel1.Controls.Add(p);

                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Błąd połączenia: " + ex.Message);
                }
            }
        }
    }
}
