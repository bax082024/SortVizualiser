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
                    case "Quick Sort":
                        await QuickSortWrapper();
                        break;
                    case "Heap Sort":
                        await HeapSort();
                        break;
                    case "Counting Sort":
                        await CountingSort();
                        break;
                    case "Radix Sort":
                        await RadixSort();
                        break;
                    case "Shell Sort":
                        await ShellSort();
                        break;
                    case "Bucket Sort":
                        await BucketSort();
                        break;
                    case "Tim Sort":
                        await TimSort();
                        break;
                    case "Pigeonhole Sort":
                        await PigeonholeSort();
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

        private async Task HeapSort()
        {
            int n = data.Count;

            // Build the heap (rearrange array)
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                await Heapify(n, i);
                if (cancelRequested) return; // Check for cancel
            }

            // One by one extract elements from the heap
            for (int i = n - 1; i > 0; i--)
            {
                // Swap the root (largest element) with the last element
                int temp = data[0];
                data[0] = data[i];
                data[i] = temp;

                currentIndex = 0;       // Highlight the root
                comparingIndex = i;     // Highlight the current element being swapped

                panelVisualizer.Invalidate(); // Redraw visualization
                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return; // Check for cancel

                // Call heapify on the reduced heap
                await Heapify(i, 0);
                if (cancelRequested) return; // Check for cancel
            }

            currentIndex = -1;
            comparingIndex = -1;

            panelVisualizer.Invalidate(); // Final refresh
            MessageBox.Show("Sorting Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // To heapify a subtree rooted at index i
        private async Task Heapify(int n, int i)
        {
            int largest = i; // Initialize largest as root
            int left = 2 * i + 1; // Left child
            int right = 2 * i + 2; // Right child

            // If left child is larger than root
            if (left < n && data[left] > data[largest])
            {
                largest = left;
            }

            // If right child is larger than largest so far
            if (right < n && data[right] > data[largest])
            {
                largest = right;
            }

            // If largest is not root
            if (largest != i)
            {
                // Swap root with the largest
                int swap = data[i];
                data[i] = data[largest];
                data[largest] = swap;

                currentIndex = i;       // Highlight the current root
                comparingIndex = largest; // Highlight the largest child

                panelVisualizer.Invalidate(); // Redraw visualization
                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return; // Check for cancel

                // Recursively heapify the affected subtree
                await Heapify(n, largest);
            }
        }

        private async Task CountingSort()
        {
            if (data.Count == 0) return;

            // Find the maximum value in the data
            int maxValue = data.Max();

            // Create the count array
            int[] count = new int[maxValue + 1];

            // Count the occurrences of each element
            foreach (var value in data)
            {
                count[value]++;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 5); // Delay to visualize counting
            }

            // Rebuild the sorted data
            int index = 0;
            for (int i = 0; i < count.Length; i++)
            {
                while (count[i] > 0)
                {
                    data[index] = i;
                    index++;
                    count[i]--;

                    // Redraw the visualization d
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 5); // Delay for visualization

                    if (cancelRequested) return; // Cancel if requested
                }
            }

            // Reset indices for coloring
            currentIndex = -1;
            comparingIndex = -1;

            // Inform the user that sorting is complete
            MessageBox.Show("Counting Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task RadixSort()
        {
            if (data.Count == 0) return;

            // Find the maximum number to determine the number of digits
            int maxValue = data.Max();
            int digitPosition = 1; // Start with the least significant digit

            while (maxValue / digitPosition > 0)
            {
                await CountingSortByDigit(digitPosition);
                digitPosition *= 10; // Move to the next digit position

                if (cancelRequested) return; // Cancel if requested
            }

            // Reset indices for coloring
            currentIndex = -1;
            comparingIndex = -1;

            // Inform the user that sorting is complete
            MessageBox.Show("Radix Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task CountingSortByDigit(int digitPosition)
        {
            int[] count = new int[10]; // For digits 0 to 9
            int[] output = new int[data.Count]; // Sorted output array

            // Count occurrences of each digit
            for (int i = 0; i < data.Count; i++)
            {
                currentIndex = i; // Highlight the current bar being processed
                int digit = (data[i] / digitPosition) % 10; // Extract the digit at the current position
                count[digit]++;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 5); // Delay for visualization

                if (cancelRequested) return; // Cancel if requested
            }

            // Update count[i] to store actual positions in the output array
            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            // Build the output array
            for (int i = data.Count - 1; i >= 0; i--)
            {
                int digit = (data[i] / digitPosition) % 10;
                output[count[digit] - 1] = data[i];
                comparingIndex = count[digit] - 1; // Highlight where the number is being placed
                count[digit]--;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 5); // Delay for visualization

                if (cancelRequested) return; // Cancel if requested
            }

            // Copy the output array back into the data list
            for (int i = 0; i < data.Count; i++)
            {
                data[i] = output[i];
                currentIndex = i; // Highlight the updated bar
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 5); // Delay for visualization

                if (cancelRequested) return; // Cancel if requested
            }
        }

        private async Task ShellSort()
        {
            if (data.Count == 0) return;

            // Start with a large gap and reduce it over time
            for (int gap = data.Count / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < data.Count; i++)
                {
                    int temp = data[i];
                    int j = i;

                    // Perform gapped insertion sort
                    while (j >= gap && data[j - gap] > temp)
                    {
                        currentIndex = j; // Highlight the current index
                        comparingIndex = j - gap; // Highlight the comparison index
                        data[j] = data[j - gap];

                        // Redraw the visualization
                        panelVisualizer.Invalidate();
                        await Task.Delay(trackBarSpeed.Value * 5); // Delay for visualization

                        j -= gap;

                        if (cancelRequested) return; // Cancel if requested
                    }

                    data[j] = temp;

                    // Redraw the visualization
                    currentIndex = j; // Highlight the placement index
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 5); // Delay for visualization

                    if (cancelRequested) return; // Cancel if requested
                }
            }

            // Reset indices for coloring
            currentIndex = -1;
            comparingIndex = -1;

            // Inform the user that sorting is complete
            MessageBox.Show("Shell Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task BucketSort()
        {
            int bucketCount = 10; // Number of buckets
            List<int>[] buckets = new List<int>[bucketCount];

            // Initialize buckets
            for (int i = 0; i < bucketCount; i++)
            {
                buckets[i] = new List<int>();
            }

            // Determine the range of data
            int maxValue = data.Max();
            int minValue = data.Min();
            int range = (maxValue - minValue) / bucketCount + 1;

            // Assign elements to buckets
            for (int i = 0; i < data.Count; i++)
            {
                int bucketIndex = (data[i] - minValue) / range;
                buckets[bucketIndex].Add(data[i]);

                // Visualization: Highlight the assigned bucket
                currentIndex = i;
                comparingIndex = -1; // Reset comparing index
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
            }

            // Sort each bucket and merge
            data.Clear();
            for (int i = 0; i < bucketCount; i++)
            {
                // Sort the individual bucket
                buckets[i].Sort();

                // Add bucket contents back to the data list
                foreach (var value in buckets[i])
                {
                    data.Add(value);

                    // Visualization: Show progress as elements are added
                    currentIndex = data.Count - 1;
                    comparingIndex = -1; // Reset comparing index
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);
                }
            }

            // Reset indices after sorting
            currentIndex = -1;
            comparingIndex = -1;

            // Notify the user
            MessageBox.Show("Bucket Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task TimSort()
        {
            int RUN = 32; // Size of each run (can be adjusted for testing)

            // Perform insertion sort on chunks of size RUN
            for (int i = 0; i < data.Count; i += RUN)
            {
                await InsertionSortTim(data, i, Math.Min(i + RUN - 1, data.Count - 1));
            }

            // Merge sorted chunks
            for (int size = RUN; size < data.Count; size = 2 * size)
            {
                for (int left = 0; left < data.Count; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = Math.Min(left + 2 * size - 1, data.Count - 1);

                    if (mid < right)
                    {
                        await MergeTim(data, left, mid, right);
                    }
                }
            }

            // Reset indices after sorting
            currentIndex = -1;
            comparingIndex = -1;

            // Notify the user
            MessageBox.Show("Tim Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Helper method: Insertion sort for TimSort
        private async Task InsertionSortTim(List<int> arr, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                int temp = arr[i];
                int j = i - 1;

                while (j >= left && arr[j] > temp)
                {
                    arr[j + 1] = arr[j];
                    j--;

                    // Visualization
                    currentIndex = i;
                    comparingIndex = j;
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);
                }

                arr[j + 1] = temp;

                // Visualization
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
            }
        }

        // Helper method: Merge for TimSort
        private async Task MergeTim(List<int> arr, int left, int mid, int right)
        {
            int len1 = mid - left + 1;
            int len2 = right - mid;

            int[] leftArr = new int[len1];
            int[] rightArr = new int[len2];

            for (int x = 0; x < len1; x++)
                leftArr[x] = arr[left + x];
            for (int x = 0; x < len2; x++)
                rightArr[x] = arr[mid + 1 + x];

            int i = 0, j = 0, k = left;

            while (i < len1 && j < len2)
            {
                if (leftArr[i] <= rightArr[j])
                {
                    arr[k] = leftArr[i];
                    i++;
                }
                else
                {
                    arr[k] = rightArr[j];
                    j++;
                }

                // Visualization
                currentIndex = k;
                comparingIndex = -1;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);

                k++;
            }

            while (i < len1)
            {
                arr[k] = leftArr[i];
                i++;
                k++;

                // Visualization
                currentIndex = k - 1;
                comparingIndex = -1;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
            }

            while (j < len2)
            {
                arr[k] = rightArr[j];
                j++;
                k++;

                // Visualization
                currentIndex = k - 1;
                comparingIndex = -1;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
            }
        }

        private async Task PigeonholeSort()
        {
            if (data.Count == 0) return;

            int min = data.Min();
            int max = data.Max();
            int range = max - min + 1;

            List<List<int>> holes = new List<List<int>>(range);
            for (int i = 0; i < range; i++)
                holes.Add(new List<int>());

            foreach (var item in data)
                holes[item - min].Add(item);

            int index = 0;
            for (int i = 0; i < range; i++)
            {
                foreach (var value in holes[i])
                {
                    data[index++] = value;

                    // Highlight the current value being placed
                    currentIndex = index - 1;
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested) return;
                }
            }

            currentIndex = -1;
            comparingIndex = -1;
            panelVisualizer.Invalidate();

            MessageBox.Show("Sorting Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        //fsdf
    }
}
