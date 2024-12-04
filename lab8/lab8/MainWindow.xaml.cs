using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Drawing;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;
using Size = System.Drawing.Size;
using Point = System.Drawing.Point;
using RadioButton = System.Windows.Forms.RadioButton;
using CheckBox = System.Windows.Forms.CheckBox;
using Button = System.Windows.Forms.Button;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab8
{
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateStatusBar_Click(object sender, RoutedEventArgs e)
        {
       
        }

        private void openClick(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog() { Description = "Select directory to open" };
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = dlg.SelectedPath;
                PopulateTreeView(selectedPath, fileTreeView);
            }
        }

        private void PopulateTreeView(string directoryPath, System.Windows.Controls.TreeView treeView)
        {
            var rootName = System.IO.Path.GetFileName(directoryPath);
            var rootPath = directoryPath;

            var root = new TreeViewItem
            {
                Header = rootName,
                Tag = rootPath
            };

            treeView.Items.Add(root);

            try
            {
                var directories = System.IO.Directory.GetDirectories(directoryPath);
                foreach (var directory in directories)
                {
                    AddTreeViewItem(root, directory, false);
                }

                var files = System.IO.Directory.GetFiles(directoryPath);
                foreach (var file in files)
                {
                    AddTreeViewItem(root, file, true);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AddTreeViewItem(TreeViewItem parentItem, string path, bool isFile = false)
        {
            var name = isFile ? System.IO.Path.GetFileName(path) : System.IO.Path.GetFileNameWithoutExtension(path);
            var item = new TreeViewItem
            {
                Header = name,
                Tag = path
            };

            var contextMenu = new ContextMenu();
            var deleteMenuItem = new MenuItem { Header = "Delete" };
            deleteMenuItem.Click += (sender, e) =>
            {
                try
                {
                    if (parentItem != null)
                    {
                        foreach (TreeViewItem item in parentItem.Items)
                        {
                            if (item.Tag.ToString() == path)
                            {
                                parentItem.Items.Remove(item);
                                break;
                            }
                        }
                    }

                    if (isFile)
                    {
                        System.IO.File.Delete(path);
                    }
                    else
                    {
                        System.IO.Directory.Delete(path, true);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error: " + ex.Message);
                }
            };

            if (!isFile)
            {
                var createMenuItem = new MenuItem { Header = "Create" };
                createMenuItem.Click += (sender, e) =>
                {
                    var dialog = new Form();
                    dialog.Text = "Create Dialog";
                    dialog.Size = new Size(400, 400);

                    var nameLabel = new Label();
                    nameLabel.Text = "Name: ";
                    nameLabel.Location = new Point(10, 10);

                    var nameTextBox = new TextBox();
                    nameTextBox.Text = "Name";
                    nameTextBox.Location = new Point(120, 10);

                    var radioButton1 = new RadioButton();
                    var radioButton2 = new RadioButton();
                    radioButton1.Location = new Point(10, 50);
                    radioButton2.Location = new Point(120, 50);
                    radioButton1.Text = "File";
                    radioButton2.Text = "Directory";
                    radioButton2.Size = new Size(TabIndex, 25);

                    var checkbox1 = new CheckBox();
                    var checkbox2 = new CheckBox();
                    var checkbox3 = new CheckBox();
                    var checkbox4 = new CheckBox();
                    checkbox1.Location = new Point(10, 100);
                    checkbox2.Location = new Point(10, 120);
                    checkbox3.Location = new Point(10, 140);
                    checkbox4.Location = new Point(10, 160);
                    checkbox1.Size = new Size(TabIndex, 23);
                    checkbox1.Text = "ReadOnly";
                    checkbox2.Text = "Archive";
                    checkbox3.Text = "Hidden";
                    checkbox4.Text = "System";

                    var okButton = new Button();
                    okButton.Text = "OK";
                    okButton.Location = new Point(10, 250);
                    okButton.Click += (sender, e) =>
                    {
                        string name = nameTextBox.Text;
                        bool isFile = radioButton1.Checked;
                        bool isReadOnly = checkbox1.Checked;
                        bool isArchive = checkbox2.Checked;
                        bool isHidden = checkbox3.Checked;
                        bool isSystem = checkbox4.Checked;

                        string fullPath = System.IO.Path.Combine(path, name);

                        try
                        {
                            if (isFile)
                            {
                                using (FileStream fs = System.IO.File.Create(fullPath))
                                {
                                    if (isReadOnly)
                                        System.IO.File.SetAttributes(fullPath, System.IO.File.GetAttributes(fullPath) | FileAttributes.ReadOnly);
                                    if (isArchive)
                                        System.IO.File.SetAttributes(fullPath, System.IO.File.GetAttributes(fullPath) | FileAttributes.Archive);
                                    if (isHidden)
                                        System.IO.File.SetAttributes(fullPath, System.IO.File.GetAttributes(fullPath) | FileAttributes.Hidden);
                                    if (isSystem)
                                        System.IO.File.SetAttributes(fullPath, System.IO.File.GetAttributes(fullPath) | FileAttributes.System);
                                }
                            }
                            else
                            {
                                var directoryInfo = System.IO.Directory.CreateDirectory(fullPath);
                                if (isReadOnly)
                                    System.IO.File.SetAttributes(fullPath, System.IO.File.GetAttributes(fullPath) | FileAttributes.ReadOnly);
                                if (isArchive)
                                    System.IO.File.SetAttributes(fullPath, System.IO.File.GetAttributes(fullPath) | FileAttributes.Archive);
                                if (isHidden)
                                    System.IO.File.SetAttributes(fullPath, System.IO.File.GetAttributes(fullPath) | FileAttributes.Hidden);
                                if (isSystem)
                                    System.IO.File.SetAttributes(fullPath, System.IO.File.GetAttributes(fullPath) | FileAttributes.System);
                            }

                            AddTreeViewItem(item, fullPath, isFile);

                            dialog.Close();
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show("Error: " + ex.Message);
                        }
                    };

                    var cancelButton = new Button();
                    cancelButton.Text = "Cancel";
                    cancelButton.Location = new Point(120, 250);
                    cancelButton.Click += (sender, e) =>
                    {
                        dialog.Close();
                    };

                    dialog.Controls.Add(nameLabel);
                    dialog.Controls.Add(nameTextBox);
                    dialog.Controls.Add(radioButton1);
                    dialog.Controls.Add(radioButton2);
                    dialog.Controls.Add(checkbox1);
                    dialog.Controls.Add(checkbox2);
                    dialog.Controls.Add(checkbox3);
                    dialog.Controls.Add(checkbox4);
                    dialog.Controls.Add(okButton);
                    dialog.Controls.Add(cancelButton);

                    dialog.ShowDialog();
                };

                contextMenu.Items.Add(createMenuItem);
            }

            contextMenu.Items.Add(deleteMenuItem);
            item.ContextMenu = contextMenu;

            parentItem.Items.Add(item);

            if (!isFile)
            {
                PopulateSubdirectories(path, item);
            }
        }

        private void PopulateSubdirectories(string directoryPath, TreeViewItem parentItem)
        {
            try
            {
                var directories = System.IO.Directory.GetDirectories(directoryPath);
                foreach (var directory in directories)
                {
                    var directoryName = System.IO.Path.GetFileName(directory);
                    var item = new TreeViewItem
                    {
                        Header = directoryName,
                        Tag = directory
                    };
                    parentItem.Items.Add(item);
                    PopulateSubdirectories(directory, item);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Error: " + e.Message);
            }
        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void fileTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender != null)
            {
                if (fileTreeView.SelectedItem is TreeViewItem selectedItem)
                {
                    string itemPath = selectedItem.Tag as string;
                    FileInfo fsi = new FileInfo(itemPath);
                    char r = (fsi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly ? 'r' : '-';
                    char a = (fsi.Attributes & FileAttributes.Archive) == FileAttributes.Archive ? 'a' : '-';
                    char h = (fsi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden ? 'h' : '-';
                    char s = (fsi.Attributes & FileAttributes.System) == FileAttributes.System ? 's' : '-';

                    string rahs = $"{r}{a}{h}{s}";
                    statusBar.Text = rahs;
                }
            }
        }
    }
}
