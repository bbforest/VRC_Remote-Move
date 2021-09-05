
namespace VRC_Remote_Move
{
    partial class Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Remote = new System.Windows.Forms.TextBox();
            this.Map = new System.Windows.Forms.TextBox();
            this.Ver = new System.Windows.Forms.Label();
            this.Run_btn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(537, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "대상 주소에서 주기적으로 월드정보를 불러오고 일치하지 않으면 VRChat을 종료 후 실행합니다. v.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "대상주소 : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "이동주소 : ";
            // 
            // Remote
            // 
            this.Remote.Location = new System.Drawing.Point(70, 24);
            this.Remote.Name = "Remote";
            this.Remote.Size = new System.Drawing.Size(677, 21);
            this.Remote.TabIndex = 2;
            this.Remote.Text = "http://pgm.bbforest.net/vrm/map.txt";
            // 
            // Map
            // 
            this.Map.Location = new System.Drawing.Point(70, 44);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size(677, 21);
            this.Map.TabIndex = 2;
            this.Map.Text = "https://vrchat.com/home/launch?worldId=wrld_1cff3558-c6e3-43b1-83ab-d551b48d6d50&" +
    "instanceId=15670~region(jp)";
            // 
            // Ver
            // 
            this.Ver.AutoSize = true;
            this.Ver.Location = new System.Drawing.Point(542, 9);
            this.Ver.Name = "Ver";
            this.Ver.Size = new System.Drawing.Size(41, 12);
            this.Ver.TabIndex = 1;
            this.Ver.Text = "0.0.0.0";
            // 
            // Run_btn
            // 
            this.Run_btn.Location = new System.Drawing.Point(499, 69);
            this.Run_btn.Name = "Run_btn";
            this.Run_btn.Size = new System.Drawing.Size(248, 84);
            this.Run_btn.TabIndex = 3;
            this.Run_btn.Text = "작동시작!";
            this.Run_btn.UseVisualStyleBackColor = true;
            this.Run_btn.Click += new System.EventHandler(this.Run_btn_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 71);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(481, 364);
            this.listBox1.TabIndex = 4;
            // 
            // Tray
            // 
            this.Tray.Icon = ((System.Drawing.Icon)(resources.GetObject("Tray.Icon")));
            this.Tray.Text = "notifyIcon1";
            this.Tray.Visible = true;
            this.Tray.Click += new System.EventHandler(this.Tray_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 450);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.Run_btn);
            this.Controls.Add(this.Map);
            this.Controls.Add(this.Remote);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Ver);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Move += new System.EventHandler(this.Main_Move);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Remote;
        private System.Windows.Forms.TextBox Map;
        private System.Windows.Forms.Label Ver;
        private System.Windows.Forms.Button Run_btn;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.NotifyIcon Tray;
    }
}

