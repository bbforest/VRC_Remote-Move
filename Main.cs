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
                Ver.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if (Ver.Text != new_ver)
                {
                    DialogResult result = MessageBox.Show("업데이트가 있습니다! 업데이트 할까요?", "파란대나무숲 VRM", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        string path = $"{Application.StartupPath}\\VRC_Remote-Move {new_ver}.exe";
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
            try
            {
                Map.Text = wc.DownloadString(Remote.Text);
            }
            catch (Exception)
            {
                List("초기 맵 로딩 실패");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //새로운 맵 확인
            try
            {
                WebClient wc = new WebClient();
                string new_map = wc.DownloadString(Remote.Text);
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
