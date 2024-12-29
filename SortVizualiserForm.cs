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
                    case "Insertion Sort":
                        await InsertionSort();
                        break;
                    case "Merge Sort":
                        await MergeSortWrapper();
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

        private async Task MergeSort(int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;

                // Recursively sort the first half
                await MergeSort(left, mid);
                if (cancelRequested) return;

                // Recursively sort the second half
                await MergeSort(mid + 1, right);
                if (cancelRequested) return;

                // Merge the sorted halves
                await Merge(left, mid, right);
            }
        }

        private async Task Merge(int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            int[] leftArray = new int[n1];
            int[] rightArray = new int[n2];

            // Copy data to temporary arrays
            for (int i = 0; i < n1; i++)
                leftArray[i] = data[left + i];
            for (int i = 0; i < n2; i++)
                rightArray[i] = data[mid + 1 + i];

            int iLeft = 0, iRight = 0, k = left;

            // Merge the temporary arrays back into the original
            while (iLeft < n1 && iRight < n2)
            {
                currentIndex = k; // Highlight the current merge position
                comparingIndex = left + iLeft; // Highlight left array element being compared
                panelVisualizer.Invalidate();

                if (leftArray[iLeft] <= rightArray[iRight])
                {
                    data[k] = leftArray[iLeft];
                    iLeft++;
                }
                else
                {
                    data[k] = rightArray[iRight];
                    iRight++;
                }
                k++;

                // Delay for visualization
                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return;
            }

            // Copy remaining elements of leftArray
            while (iLeft < n1)
            {
                currentIndex = k;
                comparingIndex = left + iLeft;
                data[k] = leftArray[iLeft];
                iLeft++;
                k++;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return;
            }

            // Copy remaining elements of rightArray
            while (iRight < n2)
            {
                currentIndex = k;
                comparingIndex = mid + 1 + iRight;
                data[k] = rightArray[iRight];
                iRight++;
                k++;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return;
            }

            // Reset indices after merging
            currentIndex = -1;
            comparingIndex = -1;
        }

        private async Task MergeSortWrapper()
        {
            await MergeSort(0, data.Count - 1);
            MessageBox.Show("Merge Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task QuickSort(int left, int right)
        {
            if (left < right)
            {
                int pivotIndex = await Partition(left, right);

                // Recursively sort elements before and after the pivot
                await QuickSort(left, pivotIndex - 1);
                await QuickSort(pivotIndex + 1, right);
            }
        }

        private async Task<int> Partition(int left, int right)
        {
            int pivot = data[right]; // Choose the rightmost element as the pivot
            int i = left - 1;

            // Highlight the pivot
            currentIndex = right;

            for (int j = left; j < right; j++)
            {
                comparingIndex = j; // Highlight the current element being compared

                if (data[j] < pivot)
                {
                    i++;
                    // Swap data[i] and data[j]
                    int temp = data[i];
                    data[i] = data[j];
                    data[j] = temp;

                    panelVisualizer.Invalidate(); // Redraw visualization
                    await Task.Delay(trackBarSpeed.Value * 10);
                    if (cancelRequested) return -1;
                }
            }

            // Swap the pivot into its correct position
            int tempPivot = data[i + 1];
            data[i + 1] = data[right];
            data[right] = tempPivot;

            panelVisualizer.Invalidate();
            await Task.Delay(trackBarSpeed.Value * 10);
            if (cancelRequested) return -1;

            // Return the partition index
            return i + 1;
        }

        private async Task QuickSortWrapper()
        {
            cancelRequested = false; // Ensure cancelRequested is reset
            await QuickSort(0, data.Count - 1);

            // Reset indices after sorting
            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Quick Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
