using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Threading;

namespace sci_hubAutoLink
{
    public partial class ScihubVal : Form
    {
        public ScihubVal()
        {
            InitializeComponent();
            lblGuide.Visible = false;
        }

        static int totalCount = 0;
        Object thisLock = new Object();
        private void Check()
        {
            string alphabat = "abcdefghijklmnopqrstuvwxyz";
            //string alphabat = "ahzk";
            char[] indx1 = alphabat.ToCharArray();
            char[] indx2 = alphabat.ToCharArray();
            string scihublink = "http://www.sci-hub.";
            //모든 경우의 수는 26^2 = 676개의 루프가 필요.
            totalCount = (int)Math.Pow(indx1.Length, 2);
            this.SetText(totalCount.ToString());
            this.lblGuide.Visible = true;
            Application.DoEvents();

            foreach (char chr1 in indx1)
            {
                foreach (char chr2 in indx2)
                {
                    string who = scihublink + chr1 + chr2;

                    //Thread trd = new Thread(new ParameterizedThreadStart(isConnSuccess));
                    Thread trd = new Thread(delegate ()
                    {
                        isConnSuccess(who);
                    }
                    );

                    trd.Start();
                }
            }
        }

        private void isConnSuccess(string url)
        {
            try
            {
                Ping pingSender = new Ping();
                Uri uri = new Uri(url);
                var ip = Dns.GetHostAddresses(uri.Host)[0];
                PingReply reply = pingSender.Send(ip);
                if (reply.Status == IPStatus.Success)
                {
                    //this.cbxLinkItem.Items.Add(url);
                    this.SetAddItem(url);
                }
            }
            catch (Exception ex)
            {

            }

            lock (thisLock)
            {
                totalCount--;
                this.SetText(totalCount.ToString());
                SetVisible(totalCount > 0);
                Application.DoEvents();
            }
        }
        
        private void SetAddItem(string text)
        {
            if (this.cbxLinkItem.InvokeRequired)
            {
                this.cbxLinkItem.BeginInvoke(new Action(() => cbxLinkItem.Items.Add(text)));
            }
            else
            {
                this.cbxLinkItem.Items.Add(text);
            }
        }

        private void SetText(string text)
        {
            if (this.tbxCount.InvokeRequired)
            {
                this.tbxCount.BeginInvoke(new Action(() => tbxCount.Text = text));
            }
            else
            {
                this.tbxCount.Text = text;
            }
        }
        
        private void SetVisible(bool isVisible)
        {
            if (this.lblGuide.InvokeRequired)
            {
                this.lblGuide.BeginInvoke(new Action(() => lblGuide.Visible = isVisible));
            }
            else
            {
                this.lblGuide.Visible = isVisible;
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Check();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(cbxLinkItem.Text);
        }

    }
}
