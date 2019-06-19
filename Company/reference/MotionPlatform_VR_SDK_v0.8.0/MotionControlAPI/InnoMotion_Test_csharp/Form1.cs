using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Inno_MotionController;

namespace InnoMotion_Test_csharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CInnoMotion_API.SetEquipNumber(11);
            int nOpen;
            nOpen = CInnoMotion_API.OpenDevice();
        }
    }
}
