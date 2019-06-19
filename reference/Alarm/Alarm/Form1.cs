using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.IO;

namespace Alarm
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        object __lockobj = new object();
        public Form1()
        {
            InitializeComponent();

            LoadList();
            timer.Tick += OnTimer;
            timer.Interval = 1000 * 60; // 60초
            timer.Start();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Show();
            this.ShowInTaskbar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void OnTimer( object sender, EventArgs e)
        {
            lock (__lockobj)
            {
                foreach (var url in listBox.Items)
                {
                    if( CompareUrl((string)url) )
                    {
                        MessageBox.Show($"{url} 변경 됨");
                    }
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            lock (__lockobj)
            {
                string url = AddressTextBox.Text;
                string sResponse = string.Empty;
                if (IsExist(url))
                {
                    MessageBox.Show("중복");
                    return;
                }

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.Timeout = 5 * 1000;
                    request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

                    using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                    {
                        HttpStatusCode status = resp.StatusCode;

                        Stream respStream = resp.GetResponseStream();
                        using (StreamReader sr = new StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                        {
                            sResponse = sr.ReadToEnd();
                            string FileUrl = $".\\{url.GetHashCode()}";
                            StreamWriter sw = new StreamWriter(FileUrl);
                            sw.Write(sResponse);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                AddUrl(AddressTextBox.Text);
                AddressTextBox.Text = "";
            }
        }

        public bool IsExist( string sAddress )
        {
            foreach( var str in listBox.Items)
            {
                if( str.Equals(sAddress))
                {
                    return true;
                }
            }

            return false;
        }

        public void LoadList()
        {
        
            if (!File.Exists(@".\UrlList.txt"))
            {
                return;
            }

            string[] AllLine = File.ReadAllLines(@".\UrlList.txt");
            
            foreach( var l in AllLine)
            {
                listBox.Items.Add(l);
            }
        }

        public void AddUrl(string url)
        {
            //string[] r = File.ReadAllLines(@".\UrlList.txt");
            string u = url + "\n";
            File.AppendAllText(@".\UrlList.txt", u);

            listBox.Items.Add( url );
        }

        public bool CompareUrl( string url )
        {
            string sResponse = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 5 * 1000;
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

                using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                {
                    HttpStatusCode status = resp.StatusCode;

                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                    {
                        sResponse = sr.ReadToEnd();
                    }

                    string FileUrl = $".\\{url.GetHashCode()}";
                    using (StreamReader rdr = new StreamReader(FileUrl))
                    {
                        string FileR = rdr.ReadToEnd();

                        if (FileR.Equals(sResponse))
                        {
                            return false;
                        }
                        
                        StreamWriter sw = new StreamWriter(FileUrl);
                        sw.Write(sResponse);
                    }

                }
            }
            catch (Exception ex)
            {                
            }

            return true;
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            //lock (__lockobj)
            //{
            //    if (e.KeyCode == Keys.Delete)
            //    {
            //        int index = listBox.SelectedIndex;
            //        string FileUrl = $".\\{listBox.Items[index].GetHashCode()}";
            //        File.Delete(FileUrl);
            //        listBox.Items.RemoveAt(index);
            //    }
            //}
        }
    }
}
