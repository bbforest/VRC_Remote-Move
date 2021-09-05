using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRC_Remote_Move
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            //vrchat://launch?ref=vrchat.com&id=wrld_59963f47-59fb-4797-9453-863f783e5007&instanceId=70606~region(jp)
            //https://vrchat.com/home/launch?worldId=wrld_b02e2bbe-c0c4-46f9-aca2-1d0133eb374f&instanceId=63500~region(jp)
            //vrchat://launch?ref=vrchat.com&id=wrld_b02e2bbe-c0c4-46f9-aca2-1d0133eb374f:63500~region(jp)
            //Process.Start("vrchat://launch?ref=vrchat.com&id=wrld_b02e2bbe-c0c4-46f9-aca2-1d0133eb374f:63500~region(jp)");

            //중복실행방지
            System.Diagnostics.Process[] processes = null;
            string CurrentProcess = System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper();
            processes = System.Diagnostics.Process.GetProcessesByName(CurrentProcess);
            if (processes.Length > 1)
            {
                MessageBox.Show("이미 VRM이 실행중입니다.\n작업표시줄 오른쪽 아이콘을 확인해보세요!", "파란대나무숲 VRM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit();
                Environment.Exit(0);
            }

            //업데이트 확인
            try
            {
                WebClient wc = new WebClient();
                string new_ver = wc.DownloadString("http://pgm.bbforest.net/vrm/ver.txt");
                Ver.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if (Ver.Text != new_ver)
                {
                    DialogResult result = MessageBox.Show("업데이트가 있습니다! 업데이트 할까요?", "파란대나무숲 VRM", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        wc.DownloadFile("http://pgm.bbforest.net/vrm/vrm.msi", Environment.GetEnvironmentVariable("temp") + "\\vrm.msi");
                        Process.Start(Environment.GetEnvironmentVariable("temp") + "\\vrm.msi");
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception)
            {
                Ver.Text = "버전 확인 실패!";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                WebClient wc = new WebClient();
                Map.Text = wc.DownloadString(Remote.Text);
            }
            catch (Exception)
            {
                Run_Set();
                List("오류! 이동주소를 찾을 수 없습니다.");
                MessageBox.Show("이동주소를 찾을 수 없습니다.\n대상주소를 확인하세요!", "파란대나무숲 VRM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool Run = false;

        private void Run_btn_Click(object sender, EventArgs e)
        {
            Run_Set();
        }

        private async void Run_Set()
        {
            Run = !Run;
            if (Run)
            {
                Run_btn.Enabled = false;
                Remote.ReadOnly = Run;
                Map.ReadOnly = Run;
                List($"작동상태 : {Run}");
                timer1.Enabled = true;
                Run_btn.Text = "작동중지!\n3초 후 가능";
                await Task.Delay(1000);
                Run_btn.Text = "작동중지!\n2초 후 가능";
                await Task.Delay(1000);
                Run_btn.Text = "작동중지!\n1초 후 가능";
                await Task.Delay(1000);
                Run_btn.Text = "작동중지!";
                Run_btn.Enabled = true;
            }
            else
            {
                List($"작동상태 : {Run}");
                timer1.Enabled = false;
                Remote.ReadOnly = Run;
                Map.ReadOnly = Run;
                Run_btn.Text = "작동시작!";
            }
        }

        private void List(string msg)
        {
            this.Invoke(new Action(delegate () {
                listBox1.Items.Add(msg);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
            }));
        }

        private void Main_Move(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Opacity = 0;
                this.ShowInTaskbar = false;
                Tray.Visible = true;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("프로그램을 종료할까요?\n최소화를 누르면 트레이에서 동작해요!", "파란대나무숲 VRM", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes) Environment.Exit(0);
            else e.Cancel = true;
        }

        private void Tray_Click(object sender, EventArgs e)
        {
            this.Opacity = 1;
            this.ShowInTaskbar = true;
            Tray.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }
    }
}
