using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SortVizualizer
{
    public partial class SortVizualiserForm : Form
    {
        private List<int> data = new List<int>();


        public SortVizualiserForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Add sorting algorithms to the dropdown
            comboAlgorithms.Items.AddRange(new string[]
            {
                "Bubble Sort",
                "Selection Sort",
                "Insertion Sort",
                "Merge Sort",
                "Quick Sort"
            });

            comboAlgorithms.SelectedIndex = 0; // Default selection
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Start clicked! Sorting algorithm will start soon.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pause clicked!");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reset clicked! Visualization will reset.");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trackSpeed_Scroll(object sender, EventArgs e)
        {
            lblSpeed.Text = $"Speed: {trackSpeed.Value}";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void GenerateRandomData(int size)
        {
            Random random = new Random();
            data.Clear();

            for (int i = 0; i < size; i++)
            {
                // Generate random values between 10 and 300 (adjust for panel height)
                data.Add(random.Next(10, panelVisualizer.Height - 10));
            }

            panelVisualizer.Invalidate(); // Redraw the panel
        }

        private void panelVisualizer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int barWidth = panelVisualizer.Width / data.Count; // Calculate bar width based on data size

            for (int i = 0; i < data.Count; i++)
            {
                int barHeight = data[i];
                int x = i * barWidth;
                int y = panelVisualizer.Height - barHeight;

                // Draw each bar as a rectangle
                g.FillRectangle(Brushes.Blue, x, y, barWidth - 2, barHeight);
            }
        }







    }
}
