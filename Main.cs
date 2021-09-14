using System;
using System.Diagnostics;
using System.Net;
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

            //중복실행방지
            Process[] processes = null;
            string CurrentProcess = Process.GetCurrentProcess().ProcessName.ToUpper();
            processes = Process.GetProcessesByName(CurrentProcess);
            if (processes.Length > 1)
            {
                MessageBox.Show("이미 VRM이 실행중입니다.\n작업표시줄 오른쪽 아이콘을 확인해보세요!", "파란대나무숲 VRM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit();
                Environment.Exit(0);
            }

            WebClient wc = new WebClient();
            //업데이트 확인
            try
            {
                string new_ver = wc.DownloadString("http://pgm.bbforest.net/vrm/ver.txt");

                string[] v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
                Ver.Text = $"{v[0]}.{v[1]}.{v[2]}";
                if (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() != new_ver)
                {
                    DialogResult result = MessageBox.Show("업데이트가 있습니다! 업데이트 할까요?", "파란대나무숲 VRM", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        v = new_ver.Split('.'); 
                        string path = $"{Application.StartupPath}\\VRC_Remote-Move {v[0]}.{v[1]}.{v[2]}.exe";
                        wc.DownloadFile("http://pgm.bbforest.net/vrm/VRC_Remote-Move.exe", path);
                        Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
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
            //새로운 맵 확인
            try
            {
                WebClient wc = new WebClient();
                string new_map = wc.DownloadString($"http://pgm.bbforest.net/vrm/data/{Remote.Text}");
                if (Map.Text != new_map) //새로운 맵이 지금 맵이랑 다르면
                {
                    Map.Text = new_map; //지금 맵에 새로운 맵 적용
                    List($"{DateTime.Now.ToString("HH:mm")} 새로운 맵 발견 : {new_map}");

                    if (new_map == "stop")
                    {
                        //종료 명령 인식시 10분 뒤 종료
                        Process.Start("shutdown.exe", "-s -t 600");
                        DialogResult result = MessageBox.Show("10분 뒤 컴퓨터가 종료됩니다.\n종료를 취소하시려면 취소버튼을 눌러주세요.", "파란대나무숲 VRM", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result == DialogResult.Cancel) Process.Start("shutdown.exe", "-a");
                    }

                    try
                    {
                        //맵의 ID와 인스턴스, 지역 분석
                        string Map_str = Map.Text.Split('?')[1], id = Map_str.Split('&')[0].Split('=')[1], i_id = Map_str.Split('&')[1].Split('=')[1];
                        //VRChat 실행 가능한 URI 생성
                        string target = $"vrchat://launch?ref=vrchat.com&id={id}:{i_id}";
                        //실행중인 VRChat 종료
                        Process[] processesList = Process.GetProcessesByName("VRChat");
                        foreach (Process process in processesList) process.Kill();
                        List("VRChat 종료됨");
                        //새로운 VRChat 실행
                        Process.Start(target);
                        List($"{DateTime.Now.ToString("HH:mm")} ID : {id} VRChat 실행 완료!");
                    }
                    catch (Exception)
                    {
                        List("이동주소 분석 실패!");
                    }
                }
            }
            //새로운 맵 확인 실패시
            catch (Exception)
            {
                Run_Set();
                List("오류! 이동주소를 찾을 수 없습니다. PC이름을 확인하세요!");
                MessageBox.Show("이동주소를 찾을 수 없습니다.\nPC이름을 확인하세요!", "파란대나무숲 VRM", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (Remote.Text == "")
                {
                    MessageBox.Show("PC 이름이 지정되지 않았습니다.\nPC이름을 지정해주세요!", "파란대나무숲 VRM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List("PC이름 : " + Remote.Text);
                try
                {
                    WebClient wc = new WebClient();
                    Map.Text = wc.DownloadString($"http://pgm.bbforest.net/vrm/data/{Remote.Text}");
                }
                catch (Exception)
                {

                }

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
            if (result == DialogResult.No) e.Cancel = true;
        }

        private void Tray_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Opacity = 1;
            Tray.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Remote.Text = Properties.Settings.Default.PCname;
            WinStart_check.Checked = Properties.Settings.Default.WinStart;
            StartTray_check.Checked = Properties.Settings.Default.StartTray;
            if (StartTray_check.Checked)
            {
                this.Opacity = 0;
                this.ShowInTaskbar = false;
                Tray.Visible = true;
            }

            List("[현재설정]");
            List("PC이름 : " + Remote.Text);
            List("윈도우 시작시 실행 : " + WinStart_check.Checked);
            List("시작시 트레이로 : " + StartTray_check.Checked);

            try
            {
                if (Remote.Text == "")
                {
                    List("PC 이름이 지정되지 않았습니다. PC이름을 지정해주세요!");
                    return;
                }
                WebClient wc = new WebClient();
                Map.Text = wc.DownloadString($"http://pgm.bbforest.net/vrm/data/{Remote.Text}");
            }
            catch (Exception)
            {
                List("초기 맵 로딩 실패! PC이름을 확인해주세요!");
            }
        }

        private void Remote_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PCname = Remote.Text;
            Properties.Settings.Default.Save();
        }

        private void StartTray_check_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.StartTray = StartTray_check.Checked;
            Properties.Settings.Default.Save();
        }

        private void WinStart_check_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.WinStart = WinStart_check.Checked;
            Properties.Settings.Default.Save();
            List($"윈도우 시작시 자동 실행 : {WinStart_check.Checked}");
            if (WinStart_check.Checked) AddStartupProgram("net.bbforest.vrm", Application.ExecutablePath);
            else if (!WinStart_check.Checked) RemoveStartupProgram("net.bbforest.vrm");
        }


        private static readonly string _startupRegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        private Microsoft.Win32.RegistryKey GetRegKey(string regPath, bool writable)
        {
            return Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regPath, writable);
        }

        public void AddStartupProgram(string programName, string executablePath)
        {
            using (var regKey = GetRegKey(_startupRegPath, true))
            {
                try
                {
                    // 키가 이미 등록돼 있지 않을때만 등록
                    if (regKey.GetValue(programName) == null)
                        regKey.SetValue(programName, executablePath);

                    regKey.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        // 등록된 프로그램 제거
        public void RemoveStartupProgram(string programName)
        {
            using (var regKey = GetRegKey(_startupRegPath, true))
            {
                try
                {
                    // 키가 이미 존재할때만 제거
                    if (regKey.GetValue(programName) != null)
                        regKey.DeleteValue(programName, false);

                    regKey.Close();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
