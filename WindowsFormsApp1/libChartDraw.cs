using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;



namespace libChartDraw
{
    public class BaseDraw
    {
        public string inputFile { set; get; }

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
            chart.SaveImage(String.Format("{3}/{0}_{1}{2}.png", Path.GetFileName(inputFile), DateTime.Now.ToString("hhmmss"), title, Path.GetDirectoryName(inputFile)), System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
        }

    }
    public class RinexDraw : BaseDraw
    {
        // 加载观测数据文件
        public void loadRinexObs()
        {
            List<string> rawData = new List<string>();
            // 卫星数
            List<int> rawSat = new List<int>();
            // 时间
            List<DateTime> rawTime = new List<DateTime>();


            FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs, Encoding.Default);
            string Line;
            int beforeSat = 0;
            while ((Line = read.ReadLine()) != null)
            {
                if (Regex.IsMatch(Line, ">"))
                {
                    rawData.Add(Line);
                    string year = Regex.Split(Line, @"\s{1,}")[0].Replace(">", "").Trim();
                    string month = Regex.Split(Line, @"\s{1,}")[1];
                    string day = Regex.Split(Line, @"\s{1,}")[2];
                    string hour = Regex.Split(Line, @"\s{1,}")[3];
                    string minute = Regex.Split(Line, @"\s{1,}")[4];
                    string second = "";
                    if (Regex.Split(Line, @"\s{1,}")[5].Split('.')[0].Length == 2)
                    {
                        second = Regex.Split(Line, @"\s{1,}")[5].Split('.')[0];
                    }
                    else
                    {
                        second = "0" + Regex.Split(Line, @"\s{1,}")[5].Split('.')[0];
                    }

                    string dateString = year + month + day + hour + minute + second;

                    DateTime dt = DateTime.ParseExact(dateString, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                    rawTime.Add(dt);
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
                    rawSat.Add(sat);
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
        public void DrawRinexSat(List<int> outData, string picInfo)
        {
            chart.Size = new System.Drawing.Size(1920, 1080);
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            ChartArea cArea = new ChartArea();

            chart.ChartAreas.Add(new ChartArea());
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("黑体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chart.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("黑体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chart.ChartAreas[0].AxisX.Title = "采集点数";
            chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("黑体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("黑体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Green;

            List<int> xData = new List<int>() { };
            List<int> yData = new List<int>() { };
            for (int i = 0; i < outData.Count; i++)
            {
                yData.Add(outData[i]);
                xData.Add(i + 1);
            }
            // chart.Series.Add(String.Format("平均值：{0}", getAverage(yData)));
            chart.Titles.Clear();
            chart.Titles.Add(picInfo + Path.GetFileNameWithoutExtension(inputFile));
            chart.Titles[0].Font = new System.Drawing.Font("黑体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chart.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
            chart.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
            chart.Series[0].Points.DataBindXY(xData, yData);
        }
    }
}
