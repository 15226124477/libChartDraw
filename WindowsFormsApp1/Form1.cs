﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libChartDraw;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // BaseDraw bDraw = new BaseDraw();
            RinexDraw rinexDraw = new RinexDraw();
            rinexDraw.InputFile = "C:\\Users\\suhang\\Desktop\\HM505\\SEPT0560.23O";
            rinexDraw.loadRinexObs();
            rinexDraw.DrawRinexSat();
            rinexDraw.SaveImage("卫星数");





        }
    }
}
