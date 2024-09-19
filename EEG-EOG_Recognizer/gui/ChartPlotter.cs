using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gui
{
    public partial class ChartPlotter : Form, SignalListener
    {
        private double lastCorData = 0;
        private Correlator correlator;
        private SignalSource source;
        private Thread analyzer;

        public ChartPlotter()
        {
            InitializeComponent();
            timer1.Stop();
           // comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (correlator.Record)
                correlator.Record = false;
            else
            {
                correlator.Record = true;
                this.chart2.Series["srchPattern"].Points.Clear();

                for (int i = 0; i < 140; i++)
                {
                    this.chart2.Series["srchPattern"].Points.AddXY(eegIndex, lastEeg);
                    ++pattIndex;
                }
            }
        }

        private void ChartDrawer_Load(object sender, EventArgs e)
        {       
            //this.chart1.ChartAreas[0].AxisY.Interval = 10;
            this.chart1.ChartAreas[0].AxisY.Minimum = -1;
            this.chart1.ChartAreas[0].AxisY.Maximum = 1;

            this.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

            this.chart2.ChartAreas[0].AxisY.Minimum = -2048;
            this.chart2.ChartAreas[0].AxisY.Maximum = 2048;
            this.chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

            this.eegChart.ChartAreas[0].AxisX.Interval = 5;
            this.eegChart.ChartAreas[0].AxisY.Minimum = -2048;
            this.eegChart.ChartAreas[0].AxisY.Maximum = 2048;
            this.eegChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

            for (int i = 0; i < 140; i++)
            {
                this.eegChart.Series["RawEeg"].Points.AddXY(eegIndex, lastEeg);
                ++eegIndex;
                this.chart1.Series["Similarity"].Points.AddXY(index, lastCorData);
                index++;
                this.chart2.Series["srchPattern"].Points.AddXY(eegIndex, lastEeg);
                ++pattIndex;
            }
        }

        //Recieves raw eeg signal data
        void SignalListener.recieveData(short dotum)
        {
            lastEeg = dotum;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            drawSimilarity();
            drawEeg();
            if (correlator.Record)
                drawSPattern();
        }

        private int liveChartSize = 5 * 25;//Display last 5 seconds.


        private int index = 0;
        private void drawSimilarity()
        {
            this.chart1.Series["Similarity"].Points.AddXY(index, lastCorData);
            index++;

            //when frame is full remove from left
            if (index >= liveChartSize)
            {
                this.chart1.Series["Similarity"].Points.RemoveAt(0);
            }
            // Redraw chart
            chart1.Invalidate();
            // Adjust Y & X axis scale
            chart1.ResetAutoValues();
        }

        private int eegIndex = 0;
        private short lastEeg = 0;

        private void drawEeg()
        {
            this.eegChart.Series["RawEeg"].Points.AddXY(eegIndex, lastEeg);
            ++eegIndex;

            //when frame is full remove from left
            if (eegIndex >= liveChartSize)
            {
                this.eegChart.Series["RawEeg"].Points.RemoveAt(0);
            }
            // Redraw chart
            eegChart.Invalidate();
            // Adjust Y & X axis scale
            eegChart.ResetAutoValues();
        }

        private ushort pattIndex = 0;
        private void drawSPattern()
        {
            this.chart2.Series["srchPattern"].Points.AddXY(eegIndex, lastEeg);
            ++pattIndex;

            //when frame is full remove from left
            if (pattIndex >= liveChartSize)
            {
                this.chart2.Series["srchPattern"].Points.RemoveAt(0);
            }
            // Redraw chart
            this.chart2.Invalidate();
            // Adjust Y & X axis scale
            this.chart2.ResetAutoValues();
        }

        
        SignalSource SignalListener.Source
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double LastCorData
        {
            set { lastCorData = value; }
        }

        private String textFilePath = "rawEeg.txt";
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            correlator = new Correlator(5);
            if (this.comboBox1.SelectedIndex == 0)
            {
                source = new SourceText(textFilePath);
            }
            else 
            {
                source = new TGConnection();
            }
            source.Attach(this);
            source.Attach(correlator);
            correlator.Plotter = this;
            analyzer = new Thread(new ThreadStart(source.start));
            
            //TODO: Reset gui
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (analyzer.ThreadState == System.Threading.ThreadState.Unstarted)
                    analyzer.Start();
                else if (analyzer.ThreadState == System.Threading.ThreadState.Suspended)
                    analyzer.Resume();

                timer1.Start();
            }
            catch (NullReferenceException ne) { }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                source.stop();
                timer1.Stop();
            }
            catch (NullReferenceException ne) { }
        }

    }

}
