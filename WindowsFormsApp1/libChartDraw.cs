using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;



namespace libChartDraw
{
    public class BaseDraw
    {
        public string InputFile { set; get; }

        public Chart chart = new Chart();

        double getAverage(List<int> arr)
        {
            //声明变量
            double a;
            double sum = 0;
            foreach (double member in arr)
            {
                sum += member;
            }
            a = sum / arr.Count;
            return Math.Round(a, 4);
        }

        public void SaveImage(string title)
        {
            chart.SaveImage(String.Format("{3}/{0}_{1}{2}.png", Path.GetFileName(InputFile), DateTime.Now.ToString("hhmmss"), title, Path.GetDirectoryName(InputFile)), System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
        }

    }
    public class RinexDraw : BaseDraw
    {
        private List<string> rawData = new List<string>();
        private List<int> rawSatData = new List<int>(); 
        private List<DateTime> rawTimeData = new List<DateTime>();
 

        // 加载观测数据文件
        public void loadRinexObs()
        {
            FileStream fs = new FileStream(InputFile, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs, Encoding.Default);
            string Line;
            int beforeSat = 0;
            while ((Line = read.ReadLine()) != null)
            {
                if (Regex.IsMatch(Line, ">"))
                {
                    Console.WriteLine(Line);
                    rawData.Add(Line);
                    string year = Regex.Split(Line, @"\s{1,}")[1];
                    string month = Regex.Split(Line, @"\s{1,}")[2];
                    string day = Regex.Split(Line, @"\s{1,}")[3];
                    string hour = Regex.Split(Line, @"\s{1,}")[4];
                    string minute = Regex.Split(Line, @"\s{1,}")[5];
                    string second = "";

                    if (Regex.Split(Line, @"\s{1,}")[6].Split('.')[0].Length == 2)
                    {
                        second = Regex.Split(Line, @"\s{1,}")[6].Split('.')[0];
                    }
                    else
                    {
                        second = "0" + Regex.Split(Line, @"\s{1,}")[6].Split('.')[0];
                    }

                    string dateString = year + month + day + hour + minute + second;
                    // MessageBox.Show(dateString);
                    DateTime dt = DateTime.ParseExact(dateString, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    rawTimeData.Add(dt);
                    int sat = Convert.ToInt32(Regex.Split(Line, @"\s{1,}")[8]);
                    if (beforeSat - sat > 5)
                    {
                        // textbox.AppendText(rawData[rawData.Count-2]+"\r\n");
                        // textbox.AppendText(rawData[rawData.Count-1]+"\r\n");
                        // textbox.AppendText("**************");
                        // textbox.AppendText(String.Format("Sat Lost:{0}颗卫星", beforeSat-sat));
                        // textbox.AppendText("**************\r\n");

                    }
                    beforeSat = sat;
                    rawSatData.Add(sat);
                    // MessageBox.Show(sat.ToString());

                }
            }
            fs.Close();
            read.Close();
            // textBox1.AppendText(String.Format(">>>绘制卫星数{0}\r\n",Path.GetFileNameWithoutExtension(filePath)));
            DrawOne(rawSat, "卫星数");
            // textbox.AppendText(String.Format("Finished At {0}\r\n", DateTime.Now.ToString()));
            // MessageBox.Show(filePath +"\n"+ getAverage(rawSat));
            GC.Collect();
            // GC.WaitForPendingFinalizers();
            // dataGridView1.Rows.Add(dr);
            // Form1.DrawOne(rawSat);
        }
        public void DrawRinexSat()
        {
            chart.Size = new System.Drawing.Size(1920, 1080);
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            ChartArea cArea = new ChartArea();
            chart.ChartAreas.Add(new ChartArea());
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            // chart.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Milliseconds;


            chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
            chart.ChartAreas[0].AxisX.Interval = 1;
            
            

            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash; //设置网格类型为虚线
            chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash; //设置网格类型为虚线

            chart.ChartAreas[0].AxisX.MajorTickMark.Enabled = true; 
            // chart.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Lines;
            // chart.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Triangle;

            // chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chart.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // chart.ChartAreas[0].AxisX.Title = "采集点数";
            chart.ChartAreas[0].AxisY.Title = "卫星数";
            chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Near;
            chart.Series.Add(String.Format("平均值"));

            // chart.ChartAreas[0].AxisX.Interval = DateTime.Parse("00:00:01").Second;
            chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Near;


            chart.Series[0].ChartType = SeriesChartType.Line;
            chart.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            chart.Titles.Clear();
            chart.Titles.Add(Path.GetFileName(InputFile));
            chart.Titles[0].Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            List<DateTime> listX = new List<DateTime>();
            List<int> listY = new List<int>();
            for (int i = 0; i < rawTimeData.Count; i++)
            {
                listX.Add(rawTimeData[i]);
                listY.Add(rawSatData[i]);
            }
            chart.Series[0].MarkerColor = Color.Green;
            chart.Series[0].MarkerSize = 10;
            // chart.Series[0].MarkerBorderWidth = 20;
            chart.Series[0].MarkerStyle = MarkerStyle.Circle; 
            chart.Series[0].BorderWidth = 3;
            // chart.Series[0].IsValueShownAsLabel = true;//设置显示示数
            // 颜色
            chart.Series[0].Color = Color.Gray;
            chart.Series[0].Points.DataBindXY(listX, listY);
            chart.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
            // chart.ChartAreas[0].AxisX.Minimum = listX[0].ToOADate();
            // chart.ChartAreas[0].AxisX.Maximum = listX[rawTimeData.Count-1].AddSeconds(1).ToOADate();
            chart.ChartAreas[0].AxisY.ScaleView.Size = listY.Max() + listY.Min() + 1;



        }
    }
}
