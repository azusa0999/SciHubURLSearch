﻿using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Threading;

namespace sci_hubAutoLink
{
    public partial class ScihubVal : Form
    {
        #region 전역변수 및 생성자

        public ScihubVal()
        {
            InitializeComponent();
            lblGuide.Visible = false;
        }
        
        static int totalCount = 0;
        Object thisLock = new Object();
        Global glb = new Global();//전역변수 객체 할당

        #endregion

        #region 메소드 Set
        /// <summary>
        /// URL 체크 메소드
        /// </summary>
        private void Check()
        {
            char[] indx1 = glb.alphabat.ToCharArray();
            char[] indx2 = glb.alphabat.ToCharArray();
            //모든 경우의 수는 알파뱃이 26개, 두 자릿수의 모든 경우의 수에 대해 확인할 것으로 정의되었다.
            //따라서 26^2 = 676개의 루프가 필요하게 된다.
            totalCount = (int)Math.Pow(indx1.Length, 2);
            this.SetText(totalCount.ToString());
            this.lblGuide.Visible = true;
            Application.DoEvents();

            foreach (char First_chr in indx1)
            {
                foreach (char Last_chr in indx2)
                {
                    string who = glb.scihublink_FirstString + First_chr + Last_chr;
                    //스레드로 유효 URL 검색
                    Thread trd = new Thread(delegate ()
                    {
                        isConnSuccess(who);
                    });

                    trd.Start();
                }
            }
        }

        /// <summary>
        /// 핑 반환에 따른 유효성 검정
        /// </summary>
        /// <param name="url">주소 문자열</param>
        private void isConnSuccess(string url)
        {
            try
            {
                //18.08.16 주석처리. 핑 체크는 보안상 이유로 막아놓는 경우가 많다. 실제로 HTTP로 접속이 되더라도 핑은 안되는 경우도 있음. 다른 방식을 고민.
                ////Ping 확인
                //Ping pingSender = new Ping();
                //Uri uri = new Uri(url);
                //var ip = Dns.GetHostAddresses(uri.Host)[0];
                //PingReply reply = pingSender.Send(ip);
                //if (reply.Status == IPStatus.Success)
                //{
                //    this.SetAddItem(url);
                //}
                
                Uri uri = new Uri(url);
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Timeout = 10000;// 서버측 응답에 대한 클라이언트의 대기시간. 타국의 경우이고 느린 서버라는 점에서 대강 디폴트는 10초.
                request.Method = "HEAD";// 이는 웹사이트여야 한다는 점. 응답받은 HEAD를 가지고 이것이 HTTP프로토콜에 맞게 응답받은 것인지 확인하게 됨.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                // 연결이 되었으면 유효한 주소로 인식하고 콤보박스에 해당 URL을 담는다.
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    this.SetAddItem(url);
                }

            }
            catch (Exception ex)
            {

            }

            // 전역 카운터 처리를 위해 다른 쓰레드에 대한 Lock
            lock (thisLock)
            {
                totalCount--;
                this.SetText(totalCount.ToString());//카운터 정보 업데이트
                SetVisible(totalCount > 0);//확인중.. 메세지 숨길 것인지 확인
                Application.DoEvents();//큐에 대기중인 메세지 처리(안하면 화면이 업데이트되지 않는다)
            }
        }
        #endregion

        #region 컨트롤러들의 스레드 안전성을 위한 메소드 SET
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
        #endregion


        #region Event Set
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Check();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(cbxLinkItem.Text);
        }

        private void cbxLinkItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbxLinkItem.SelectedIndex > 0)
            {
                this.webViewer.Navigate(cbxLinkItem.Text);
            }
        }
        
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Popup.Help help = new Popup.Help();
            help.ShowDialog();
        }
        #endregion

    }
}
