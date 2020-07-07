using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Keyboard
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);
        //window task barı gizler
        private const int HIDE = 0;
        //window task barı gösterir
        private const int SHOW = 1;
        int simdikiWidth = 1024;
        int simdikiHeight = 768;

        public Form1()
        {
            InitializeComponent();
            taskbargizle();
            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            Rectangle calismaAlan = new Rectangle();
            calismaAlan = Screen.GetBounds(calismaAlan);
            float oranWidth = ((float)calismaAlan.Width / (float)simdikiWidth);
            float oranHeight = ((float)calismaAlan.Height / (float)simdikiHeight);
            this.Scale(new SizeF(oranWidth, oranHeight));
            Keyboard ana = new Keyboard();
            ana.Show();
        }
        public void taskbargizle()
        {
            ////form pencresini bulalım (handle)
            int hwnd = FindWindow("Shell_TrayWnd", "");

            //window task bar gizli olacak
            // ShowWindow(hwnd, HIDE);

            //formun başlığı olmasın
            this.FormBorderStyle = FormBorderStyle.None;

            //pencereyi boyutunu tam ekran olacak şekilde ayarlayalım
            this.Size = new Size(SystemInformation.VirtualScreen.Width,
                                     SystemInformation.VirtualScreen.Height + this.Height - this.ClientSize.Height);

            //form penceresini ekranda ortalayalım
            CenterToScreen();
        }

        private string haberlesme = "SER";
        public void XmlOku()
        {
            XmlTextReader oku = new XmlTextReader("config.xml");
            while (oku.Read())
            {
                if (oku.NodeType == XmlNodeType.Element)
                {
                    switch (oku.Name)
                    {
                        case "haberlesme":
                            haberlesme = oku.ReadString();
                            break;
                    }
                }
            }
            oku.Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F2") { ((Keyboard)Application.OpenForms["Keyboard"]).Show(); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
