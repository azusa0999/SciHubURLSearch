using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sci_hubAutoLink.Popup
{
    public partial class Help : Form
    {
        Global glb = new Global();
        public Help()
        {
            InitializeComponent();
            StringBuilder stb = new StringBuilder();
            stb.AppendLine("사이허브를 사용하다가 URL이 변경되어 해맸었던 기억이 있습니다.");
            stb.AppendLine("");
            stb.Append("사이허브 제작자가 최상위 도메인을 두 자릿수 내에서 알파뱃만 변경하더라구요. ");
            stb.AppendLine("이 프로그램을 만들 아이디어를 거기서 얻었습니다.");
            stb.AppendLine("");
            stb.Append("단순히 Ping의 반환에 따라 유효한 URL의 목록을 얻을 수 있도록 만들어봤습니다.");
            stb.AppendLine("그래서 혹시 부족한 점이 있거나 조건이 변경된 것이 있다면 블로그에 말씀해주세요.");
            stb.Append("또한 직접 수정해주실 수 있으시다면 GitHub 레파지토리에서 브런치를 만들어주세요. 검토 후 반영하겠습니다.");
            stb.AppendLine("");
            stb.AppendLine("감사합니다.");
            tbxText.Text = stb.ToString();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(glb.MyBlogLink);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(glb.MyGitHubLink);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
