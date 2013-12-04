using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace JoinFile
{
    public partial class frmMain : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        #region DLL
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        #endregion

        public frmMain()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(1, 1, this.Width, this.Height, 15, 15));
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            txtSourceFile.Text = openFileDialog1.FileName;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            txtDestinationFile.Text = saveFileDialog1.FileName;
        }

        private void btnJoinFile_Click(object sender, EventArgs e)
        {
            //this.Enabled = false;
            loadingImage.Visible = true;
            loadingImage.Enabled = true;

            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            btnBrowse.Visible = false;
            btnJoinFile.Visible = false;
            btnSaveFile.Visible = false;
            btnClose.Visible = false;
            txtDestinationFile.Visible = false;
            txtSourceFile.Visible = false;

            backgroundWorker1.WorkerSupportsCancellation = false;
            backgroundWorker1.WorkerReportsProgress = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                        m.Result = (IntPtr)HTCAPTION;
                    return;
                    break;
            }
            base.WndProc(ref m);
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            int i = 0;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowse_MouseHover(object sender, EventArgs e)
        {
            ((Button)sender).BackgroundImage = Properties.Resources.button_active;
        }

        private void btnBrowse_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ExcelUtility.JoinFiles(txtSourceFile.Text, txtDestinationFile.Text);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.Enabled = true;
            if (e.Error != null)
            {
                MessageBox.Show("There're some error. Please make sure your input file is correct.", "Error", MessageBoxButtons.OK);
            }
            
            loadingImage.Visible = false;
            loadingImage.Enabled = true;

            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            btnBrowse.Visible = true;
            btnJoinFile.Visible = true;
            btnSaveFile.Visible = true;
            btnClose.Visible = true;
            txtDestinationFile.Visible = true;
            txtSourceFile.Visible = true;
        } 

    }
}
