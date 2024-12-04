// form1.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompressionApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CompressFiles(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath);

            Parallel.ForEach(files, (currentFile) =>
            {
                string fileName = Path.GetFileName(currentFile);
                string compressedFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.gz";

                using (FileStream originalFileStream = File.OpenRead(currentFile))
                {
                    using (FileStream compressedFileStream = File.Create(Path.Combine(directoryPath, compressedFileName)))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                        }
                    }
                }
            });

            MessageBox.Show("Compression complete.");
        }

        private void DecompressFiles(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath, "*.gz");

            Parallel.ForEach(files, (currentFile) =>
            {
                string fileName = Path.GetFileName(currentFile);
                string decompressedFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.txt";

                using (FileStream originalFileStream = File.OpenRead(currentFile))
                {
                    using (FileStream decompressedFileStream = File.Create(Path.Combine(directoryPath, decompressedFileName)))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                        }
                    }
                }
            });

            MessageBox.Show("Decompression complete.");
        }

        private void compressButton_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    CompressFiles(folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void decompressButton_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    DecompressFiles(folderBrowserDialog.SelectedPath);
                }
            }
        }
    }
}
