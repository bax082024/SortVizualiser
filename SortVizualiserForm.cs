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
            // Populate the ComboBox with sorting algorithm options
            comboAlgorithms.Items.AddRange(new string[]
            {
                "Bubble Sort",
                "Selection Sort",
                "Insertion Sort",
                "Merge Sort",
                "Quick Sort"
            });

            comboAlgorithms.SelectedIndex = 0; // Set default selection
            GenerateRandomData(50); // Generate initial data automatically
        }



        private async void btnStart_Click(object sender, EventArgs e)
        {
            // Ensure data exists before starting
            if (data == null || data.Count == 0)
            {
                GenerateRandomData(50); // Generate a default data set
            }

            btnStart.Enabled = false; // Disable the Start button during sorting
            btnReset.Enabled = false; // Disable Reset during sorting

            string selectedAlgorithm = comboAlgorithms.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedAlgorithm))
            {
                switch (selectedAlgorithm)
                {
                    case "Bubble Sort":
                        await BubbleSort();
                        break;

                        // Add more cases for other algorithms
                }
            }

            btnStart.Enabled = true; // Re-enable Start button after sorting
            btnReset.Enabled = true; // Re-enable Reset button after sorting
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pause clicked!");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            GenerateRandomData(50);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trackSpeed_Scroll(object sender, EventArgs e)
        {
            lblSpeed.Text = $"Speed: {trackBarSpeed.Value}";
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
            if (data.Count == 0) return; // Exit early if no data to visualize

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

        private async Task BubbleSort()
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                for (int j = 0; j < data.Count - i - 1; j++)
                {
                    if (data[j] > data[j + 1])
                    {
                        // Swap elements
                        int temp = data[j];
                        data[j] = data[j + 1];
                        data[j + 1] = temp;

                        // Redraw the visualization
                        panelVisualizer.Invalidate();

                        // Delay for visualization based on the speed slider
                        await Task.Delay(trackBarSpeed.Value * 10);
                    }
                }
            }
        }

        private async Task SelectionSort()
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < data.Count; j++)
                {
                    if (data[j] < data[minIndex])
                    {
                        minIndex = j;
                    }
                }

                // Swap the minimum element with the first element of the unsorted part
                if (minIndex != i)
                {
                    int temp = data[i];
                    data[i] = data[minIndex];
                    data[minIndex] = temp;

                    // Redraw the visualization
                    panelVisualizer.Invalidate();

                    // Delay for visualization based on speed slider
                    await Task.Delay(trackBarSpeed.Value * 10);
                }
            }
        }


        private void comboAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {           
            comboAlgorithms.Items.AddRange(new string[]
            {
                "Bubble Sort",
                "Selection Sort" // Add Selection Sort
            });
        }
    }
}
