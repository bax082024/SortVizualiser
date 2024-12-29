using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SortVizualizer
{
    public partial class SortVizualiserForm : Form
    {
        private List<int> data = new List<int>();
        private int currentIndex = -1;
        private int comparingIndex = -1;
        private bool cancelRequested = false;
        private bool isAlgorithmManuallySelected = false;


        public SortVizualiserForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            GenerateRandomData(50);

            isAlgorithmManuallySelected = true;
        }






        private async void btnStart_Click(object sender, EventArgs e)
        {
            cancelRequested = false;

            if (data == null || data.Count == 0)
            {
                GenerateRandomData(50);
            }

            btnStart.Enabled = false;
            btnReset.Enabled = false;

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
                    case "Gnome Sort":
                        await GnomeSort();
                        break;
                    case "Comb Sort":
                        await CombSort();
                        break;
                    case "Cycle Sort":
                        await CycleSort();
                        break;
                    case "Bitonic Sort":
                        await BitonicSortWrapper();
                        break;
                    case "Odd-Even Sort":
                        await OddEvenSort();
                        break;
                    case "Flash Sort":
                        await FlashSort();
                        break;
                    




                }
            }

            btnStart.Enabled = true;
            btnReset.Enabled = true;
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
                data.Add(random.Next(10, panelVisualizer.Height - 10));
            }

            panelVisualizer.Invalidate();
        }

        private void panelVisualizer_Paint(object sender, PaintEventArgs e)
        {
            if (data.Count == 0) return;

            Graphics g = e.Graphics;
            int barWidth = panelVisualizer.Width / data.Count;

            for (int i = 0; i < data.Count; i++)
            {
                Brush brush = Brushes.Blue;

                if (i == currentIndex) brush = Brushes.Red;
                else if (i == comparingIndex) brush = Brushes.Green;

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
                    currentIndex = j;
                    comparingIndex = j + 1;

                    if (data[j] > data[j + 1])
                    {
                        int temp = data[j];
                        data[j] = data[j + 1];
                        data[j + 1] = temp;
                    }

                    panelVisualizer.Invalidate();

                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested) return;
                }
            }
        }

        private async Task SelectionSort()
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                int minIndex = i;
                currentIndex = i;

                for (int j = i + 1; j < data.Count; j++)
                {
                    comparingIndex = j;
                    if (data[j] < data[minIndex])
                    {
                        minIndex = j;
                    }

                    panelVisualizer.Invalidate();

                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested) return;
                }

                if (minIndex != i)
                {
                    int temp = data[i];
                    data[i] = data[minIndex];
                    data[minIndex] = temp;
                }


                panelVisualizer.Invalidate();
            }

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

                currentIndex = i;
                comparingIndex = j;

                while (j >= 0 && data[j] > key)
                {
                    data[j + 1] = data[j];
                    j--;


                    comparingIndex = j;
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested) return;
                }

                data[j + 1] = key;

                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);

                if (cancelRequested) return; 
            }

            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Insertion Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task MergeSort(int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;

                await MergeSort(left, mid);
                if (cancelRequested) return;

                await MergeSort(mid + 1, right);
                if (cancelRequested) return;

                await Merge(left, mid, right);
            }
        }

        private async Task Merge(int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            int[] leftArray = new int[n1];
            int[] rightArray = new int[n2];

            for (int i = 0; i < n1; i++)
                leftArray[i] = data[left + i];
            for (int i = 0; i < n2; i++)
                rightArray[i] = data[mid + 1 + i];

            int iLeft = 0, iRight = 0, k = left;

            while (iLeft < n1 && iRight < n2)
            {
                currentIndex = k;
                comparingIndex = left + iLeft;
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

                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return;
            }


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

                await QuickSort(left, pivotIndex - 1);
                await QuickSort(pivotIndex + 1, right);
            }
        }

        private async Task<int> Partition(int left, int right)
        {
            int pivot = data[right];
            int i = left - 1;

            currentIndex = right;

            for (int j = left; j < right; j++)
            {
                comparingIndex = j;

                if (data[j] < pivot)
                {
                    i++;
                    int temp = data[i];
                    data[i] = data[j];
                    data[j] = temp;

                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);
                    if (cancelRequested) return -1;
                }
            }

            int tempPivot = data[i + 1];
            data[i + 1] = data[right];
            data[right] = tempPivot;

            panelVisualizer.Invalidate();
            await Task.Delay(trackBarSpeed.Value * 10);
            if (cancelRequested) return -1;

            return i + 1;
        }

        private async Task QuickSortWrapper()
        {
            cancelRequested = false;
            await QuickSort(0, data.Count - 1);

            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Quick Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task HeapSort()
        {
            int n = data.Count;

            for (int i = n / 2 - 1; i >= 0; i--)
            {
                await Heapify(n, i);
                if (cancelRequested) return;
            }


            for (int i = n - 1; i > 0; i--)
            {

                int temp = data[0];
                data[0] = data[i];
                data[i] = temp;

                currentIndex = 0;
                comparingIndex = i;

                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return;

                await Heapify(i, 0);
                if (cancelRequested) return;
            }

            currentIndex = -1;
            comparingIndex = -1;

            panelVisualizer.Invalidate();
            MessageBox.Show("Sorting Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private async Task Heapify(int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2; 

            if (left < n && data[left] > data[largest])
            {
                largest = left;
            }

            if (right < n && data[right] > data[largest])
            {
                largest = right;
            }


            if (largest != i)
            {
                int swap = data[i];
                data[i] = data[largest];
                data[largest] = swap;

                currentIndex = i; 
                comparingIndex = largest;

                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return;

                await Heapify(n, largest);
            }
        }

        private async Task CountingSort()
        {
            if (data.Count == 0) return;

            int maxValue = data.Max();

            int[] count = new int[maxValue + 1];

            foreach (var value in data)
            {
                count[value]++;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 5);
            }

            int index = 0;
            for (int i = 0; i < count.Length; i++)
            {
                while (count[i] > 0)
                {
                    data[index] = i;
                    index++;
                    count[i]--;

                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 5);

                    if (cancelRequested) return;
                }
            }

            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Counting Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task RadixSort()
        {
            if (data.Count == 0) return;

            int maxValue = data.Max();
            int digitPosition = 1;

            while (maxValue / digitPosition > 0)
            {
                await CountingSortByDigit(digitPosition);
                digitPosition *= 10;

                if (cancelRequested) return;
            }

            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Radix Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task CountingSortByDigit(int digitPosition)
        {
            int[] count = new int[10];
            int[] output = new int[data.Count];

            for (int i = 0; i < data.Count; i++)
            {
                currentIndex = i;
                int digit = (data[i] / digitPosition) % 10;
                count[digit]++;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 5);

                if (cancelRequested) return;
            }

            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = data.Count - 1; i >= 0; i--)
            {
                int digit = (data[i] / digitPosition) % 10;
                output[count[digit] - 1] = data[i];
                comparingIndex = count[digit] - 1;
                count[digit]--;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 5);

                if (cancelRequested) return;
            }

            for (int i = 0; i < data.Count; i++)
            {
                data[i] = output[i];
                currentIndex = i;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 5);

                if (cancelRequested) return;
            }
        }

        private async Task ShellSort()
        {
            if (data.Count == 0) return;

            for (int gap = data.Count / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < data.Count; i++)
                {
                    int temp = data[i];
                    int j = i;

                    while (j >= gap && data[j - gap] > temp)
                    {
                        currentIndex = j;
                        comparingIndex = j - gap;
                        data[j] = data[j - gap];

                        panelVisualizer.Invalidate();
                        await Task.Delay(trackBarSpeed.Value * 5);

                        j -= gap;

                        if (cancelRequested) return;
                    }

                    data[j] = temp;

                    currentIndex = j;
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 5);

                    if (cancelRequested) return;
                }
            }

            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Shell Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task BucketSort()
        {
            int bucketCount = 10;
            List<int>[] buckets = new List<int>[bucketCount];

            for (int i = 0; i < bucketCount; i++)
            {
                buckets[i] = new List<int>();
            }

            int maxValue = data.Max();
            int minValue = data.Min();
            int range = (maxValue - minValue) / bucketCount + 1;

            for (int i = 0; i < data.Count; i++)
            {
                int bucketIndex = (data[i] - minValue) / range;
                buckets[bucketIndex].Add(data[i]);

                currentIndex = i;
                comparingIndex = -1;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
            }

            data.Clear();
            for (int i = 0; i < bucketCount; i++)
            {
                buckets[i].Sort();

                foreach (var value in buckets[i])
                {
                    data.Add(value);

                    currentIndex = data.Count - 1;
                    comparingIndex = -1;
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);
                }
            }

            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Bucket Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task TimSort()
        {
            int RUN = 32;

            for (int i = 0; i < data.Count; i += RUN)
            {
                await InsertionSortTim(data, i, Math.Min(i + RUN - 1, data.Count - 1));
            }

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

            currentIndex = -1;
            comparingIndex = -1;

            MessageBox.Show("Tim Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

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

                    currentIndex = i;
                    comparingIndex = j;
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);
                }

                arr[j + 1] = temp;

                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
            }
        }

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

        private async Task GnomeSort()
        {
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("No data to sort!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int index = 0;

            while (index < data.Count)
            {
                currentIndex = index;

                if (index == 0 || data[index] >= data[index - 1])
                {
                    index++;
                }
                else
                {

                    int temp = data[index];
                    data[index] = data[index - 1];
                    data[index - 1] = temp;
                    index--;

                    comparingIndex = index;
                }

                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);

                if (cancelRequested)
                {
                    currentIndex = -1;
                    comparingIndex = -1;
                    panelVisualizer.Invalidate();
                    return;
                }
            }

            currentIndex = -1;
            comparingIndex = -1;
            panelVisualizer.Invalidate();

            MessageBox.Show("Gnome Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task CombSort()
        {
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("No data to sort!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int gap = data.Count;
            bool swapped = true;
            const double shrinkFactor = 1.3;

            while (gap > 1 || swapped)
            {
                gap = (int)(gap / shrinkFactor);
                if (gap < 1) gap = 1;

                swapped = false;

                for (int i = 0; i + gap < data.Count; i++)
                {
                    currentIndex = i; 
                    comparingIndex = i + gap;

                    if (data[i] > data[i + gap])
                    {
                        int temp = data[i];
                        data[i] = data[i + gap];
                        data[i + gap] = temp;
                        swapped = true;
                    }

                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested)
                    {
                        currentIndex = -1;
                        comparingIndex = -1;
                        panelVisualizer.Invalidate();
                        return;
                    }
                }
            }

            currentIndex = -1;
            comparingIndex = -1;
            panelVisualizer.Invalidate();

            MessageBox.Show("Comb Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task CycleSort()
        {
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("No data to sort!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int cycleStart = 0; cycleStart < data.Count - 1; cycleStart++)
            {
                int item = data[cycleStart];
                int pos = cycleStart;

                for (int i = cycleStart + 1; i < data.Count; i++)
                {
                    comparingIndex = i;
                    if (data[i] < item)
                    {
                        pos++;
                    }

                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);
                    if (cancelRequested) return;
                }

                if (pos == cycleStart) continue;

                while (item == data[pos]) pos++;
                int temp = data[pos];
                data[pos] = item;
                item = temp;

                currentIndex = pos;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return;

                while (pos != cycleStart)
                {
                    pos = cycleStart;

                    for (int i = cycleStart + 1; i < data.Count; i++)
                    {
                        comparingIndex = i;
                        if (data[i] < item)
                        {
                            pos++;
                        }

                        panelVisualizer.Invalidate();
                        await Task.Delay(trackBarSpeed.Value * 10);
                        if (cancelRequested) return;
                    }

                    while (item == data[pos]) pos++;
                    temp = data[pos];
                    data[pos] = item;
                    item = temp;

                    currentIndex = pos;
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);
                    if (cancelRequested) return;
                }
            }

            currentIndex = -1;
            comparingIndex = -1;
            panelVisualizer.Invalidate();

            MessageBox.Show("Cycle Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task BitonicSort(int low, int count, bool ascending)
        {
            if (count <= 1) return;

            int k = count / 2;

            await BitonicSort(low, k, true);

            await BitonicSort(low + k, k, false);

            await BitonicMerge(low, count, ascending);
        }

        private async Task BitonicMerge(int low, int count, bool ascending)
        {
            if (count <= 1) return;

            int k = count / 2;
            for (int i = low; i < low + k; i++)
            {
                comparingIndex = i + k;
                if ((ascending && data[i] > data[i + k]) || (!ascending && data[i] < data[i + k]))
                {
                    int temp = data[i];
                    data[i] = data[i + k];
                    data[i + k] = temp;
                }

                currentIndex = i;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 10);
                if (cancelRequested) return;
            }

            await BitonicMerge(low, k, ascending);
            await BitonicMerge(low + k, k, ascending);
        }

        private async Task BitonicSortWrapper()
        {
            cancelRequested = false;

            int nextPowerOfTwo = 1;
            while (nextPowerOfTwo < data.Count)
            {
                nextPowerOfTwo *= 2;
            }

            while (data.Count < nextPowerOfTwo)
            {
                data.Add(0);
            }

            await BitonicSort(0, data.Count, true);

            data.RemoveAll(x => x == 0);

            currentIndex = -1;
            comparingIndex = -1;
            panelVisualizer.Invalidate();

            MessageBox.Show("Bitonic Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task OddEvenSort()
        {
            if (data.Count == 0) return;

            bool sorted = false;

            while (!sorted)
            {
                sorted = true;

                for (int i = 1; i < data.Count - 1; i += 2)
                {
                    currentIndex = i; 
                    comparingIndex = i + 1;

                    if (data[i] > data[i + 1])
                    {
                        int temp = data[i];
                        data[i] = data[i + 1];
                        data[i + 1] = temp;

                        sorted = false;
                    }

                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested) return;
                }


                for (int i = 0; i < data.Count - 1; i += 2)
                {
                    currentIndex = i;
                    comparingIndex = i + 1; 

                    if (data[i] > data[i + 1])
                    {
                        int temp = data[i];
                        data[i] = data[i + 1];
                        data[i + 1] = temp;

                        sorted = false;
                    }

                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 10);

                    if (cancelRequested) return;
                }
            }

            currentIndex = -1;
            comparingIndex = -1;
            panelVisualizer.Invalidate();

            MessageBox.Show("Odd-Even Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task FlashSort()
        {
            if (data.Count <= 1) return;

            int n = data.Count;
            int m = Math.Max(1, n / 10);
            int[] l = new int[m];
            int min = data.Min();
            int maxIndex = 0;

            for (int i = 1; i < n; i++)
            {
                if (data[i] > data[maxIndex])
                    maxIndex = i;

                currentIndex = i;
                panelVisualizer.Invalidate();
                await Task.Delay(trackBarSpeed.Value * 5);

                if (cancelRequested) return;
            }

            int max = data[maxIndex];

            if (max == min) return;

            for (int i = 0; i < n; i++)
            {
                int bucketIndex = (int)((m - 1) * (double)(data[i] - min) / (max - min));
                l[bucketIndex]++;
            }

            for (int i = 1; i < m; i++)
            {
                l[i] += l[i - 1];
            }

            int move = 0;
            int j = 0;
            int k = m - 1;

            while (move < n - 1)
            {
                while (j > l[k] - 1)
                {
                    j++;
                    k = (int)((m - 1) * (double)(data[j] - min) / (max - min));
                }

                int flash = data[j];
                while (j != l[k])
                {
                    k = (int)((m - 1) * (double)(flash - min) / (max - min));
                    int pos = --l[k];
                    int temp = data[pos];
                    data[pos] = flash;
                    flash = temp;

                    currentIndex = pos;
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 5);

                    if (cancelRequested) return;

                    move++;
                }
            }

            for (int i = 1; i < n; i++)
            {
                int key = data[i];
                int j2 = i - 1;

                while (j2 >= 0 && data[j2] > key)
                {
                    data[j2 + 1] = data[j2];
                    j2--;

                    currentIndex = j2 + 1;
                    comparingIndex = j2;
                    panelVisualizer.Invalidate();
                    await Task.Delay(trackBarSpeed.Value * 5);

                    if (cancelRequested) return;
                }

                data[j2 + 1] = key;
            }

            currentIndex = -1;
            comparingIndex = -1;
            panelVisualizer.Invalidate();

            MessageBox.Show("Flash Sort Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.DarkBlue,  // Top color
                Color.DarkOrange,        // Bottom color
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(gradientBrush, this.ClientRectangle);
            }
        }

        private void comboAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isAlgorithmManuallySelected)
                return;

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

        private void SortVizualiserForm_Load(object sender, EventArgs e)
        {

        }
   
    }
}
