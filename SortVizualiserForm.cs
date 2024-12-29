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
        private int currentIndex = -1; // Current comparison index
        private int comparingIndex = -1; // Second bar being compared
        private bool cancelRequested = false;
        private bool isAlgorithmManuallySelected = false;


        public SortVizualiserForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

             // Set default selection
            GenerateRandomData(50); // Generate initial data with 50 bars

            isAlgorithmManuallySelected = true;
        }






        private async void btnStart_Click(object sender, EventArgs e)
        {
            cancelRequested = false;

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
                    case "Selection Sort":
                        await SelectionSort();
                        break;

                        // Add more cases for other algorithms
                }
            }

            btnStart.Enabled = true; // Re-enable Start button after sorting
            btnReset.Enabled = true; // Re-enable Reset button after sorting
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelRequested = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cancelRequested = false;
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
            int barWidth = panelVisualizer.Width / data.Count;

            for (int i = 0; i < data.Count; i++)
            {
                Brush brush = Brushes.Blue;

                if (i == currentIndex) brush = Brushes.Red; // Highlight the current bar
                else if (i == comparingIndex) brush = Brushes.Green; // Highlight the comparing bar

                int barHeight = data[i];
                int x = i * barWidth;
                int y = panelVisualizer.Height - barHeight;

                g.FillRectangle(brush, x, y, barWidth - 2, barHeight);
            }
        }

        private async Task BubbleSort()
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                for (int j = 0; j < data.Count - i - 1; j++)
                {
                    currentIndex = j; // Update the current index
                    comparingIndex = j + 1; // Update the comparing index

                    if (data[j] > data[j + 1])
                    {
                        // Swap elements
                        int temp = data[j];
                        data[j] = data[j + 1];
                        data[j + 1] = temp;
                    }

                    // Redraw the visualization
                    panelVisualizer.Invalidate();

                    // Delay for visualization based on the speed slider
                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested) return; // Stop sorting if canceled
                }
            }
        }

        private async Task SelectionSort()
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                int minIndex = i;
                currentIndex = i; // Update the index for coloring

                for (int j = i + 1; j < data.Count; j++)
                {
                    comparingIndex = j; // Update the second index for coloring
                    if (data[j] < data[minIndex])
                    {
                        minIndex = j;
                    }

                    // Redraw the visualization
                    panelVisualizer.Invalidate();

                    // Delay for visualization based on the speed slider
                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested) return; // Check if sorting was canceled
                }

                // Swap the elements
                if (minIndex != i)
                {
                    int temp = data[i];
                    data[i] = data[minIndex];
                    data[minIndex] = temp;
                }

                // Redraw the visualization
                panelVisualizer.Invalidate();
            }

            // Reset indices after sorting
            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Sorting Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task InsertionSort()
        {
            for (int i = 1; i < data.Count; i++)
            {
                int key = data[i];
                int j = i - 1;

                currentIndex = i; // Mark the current element
                comparingIndex = j; // Mark the element being compared

                while (j >= 0 && data[j] > key)
                {
                    data[j + 1] = data[j];
                    j--;

                    // Update visualization
                    comparingIndex = j; // Update the comparison index
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested) return; // Stop if cancel is requested
                }

                data[j + 1] = key;

                // Update visualization after placing the key
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);

                if (cancelRequested) return; // Stop if cancel is requested
            }

            // Reset indices
            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Insertion Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void comboAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isAlgorithmManuallySelected)
                return; // Ignore event if triggered during initialization

            string selectedAlgorithm = comboAlgorithms.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedAlgorithm))
            {
                MessageBox.Show("No algorithm selected yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show($"You selected: {selectedAlgorithm}", "Algorithm Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
