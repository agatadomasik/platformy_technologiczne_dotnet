using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace lab10
{
    public partial class Form1 : Form
    {
        //private BindingList<Car> myCarsBindingList;
        private SortableSearchableBindingList<Car> myCarsBindingList;

        private List<Car> myCars;
        private Form dialog;
        private TextBox modelTextBox;
        private TextBox yearTextBox;
        private TextBox engineModelTextBox;
        private TextBox engineHPTextBox;
        private TextBox displacementTextBox;
        

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeCars();
            Ex1();
            Ex2();
            toolStripComboBox1.Enter += new EventHandler(toolStripComboBox1_Enter);
            toolStripButton1.Click += new EventHandler(toolStripButton1_Click);
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AllowUserToOrderColumns = true;

            dataGridView1.Columns.Add("EngineModelColumn", "Engine Model");
            dataGridView1.Columns.Add("DisplacementColumn", "Displacement");
            dataGridView1.Columns.Add("HorsePowerColumn", "Horse Power");

            // Ustawiamy właściwości sortowania dla nowych kolumn
            dataGridView1.Columns["EngineModelColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridView1.Columns["DisplacementColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridView1.Columns["HorsePowerColumn"].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dataGridView = sender as DataGridView;
                if (dataGridView.Columns[e.ColumnIndex].Name == "EngineModelColumn")
                {
                    e.Value = myCarsBindingList[e.RowIndex].Engine.Model;
                }
                else if (dataGridView.Columns[e.ColumnIndex].Name == "DisplacementColumn")
                {
                    e.Value = myCarsBindingList[e.RowIndex].Engine.Displacement;
                }
                else if (dataGridView.Columns[e.ColumnIndex].Name == "HorsePowerColumn")
                {
                    e.Value = myCarsBindingList[e.RowIndex].Engine.HorsePower;
                }
            }
        }

        private void InitializeCars()
        {
            myCarsBindingList = new SortableSearchableBindingList<Car>(new List<Car>(){
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            });

            myCars = new List<Car>(new List<Car>(){
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            });

            //dataGridView1.DataSource = myCars;
            dataGridView1.DataSource = myCarsBindingList;
            dataGridView1.Columns["Engine"].Visible = false;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
            if (column.Name == "EngineModelColumn")
            {
                myCarsBindingList = new SortableSearchableBindingList<Car>(myCarsBindingList.OrderBy(car => car.Engine.Model).ToList());
            }
            else if (column.Name == "DisplacementColumn")
            {
                myCarsBindingList = new SortableSearchableBindingList<Car>(myCarsBindingList.OrderBy(car => car.Engine.Displacement).ToList());
            }
            else if (column.Name == "HorsePowerColumn")
            {
                myCarsBindingList = new SortableSearchableBindingList<Car>(myCarsBindingList.OrderBy(car => car.Engine.HorsePower).ToList());
            }
            else if (column.Index == 3) // Model
            {
                myCarsBindingList = new SortableSearchableBindingList<Car>(myCarsBindingList.OrderBy(car => car.Model).ToList());
            }
            else if (column.Index == 5) // Year
            {
                myCarsBindingList = new SortableSearchableBindingList<Car>(myCarsBindingList.OrderBy(car => car.Year).ToList());
            }

            dataGridView1.DataSource = myCarsBindingList;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            dialog = new Form();
            dialog.Text = "Create Dialog";
            dialog.Size = new Size(280, 250);

            var modelLabel = new Label();
            modelLabel.Text = "Model: ";
            modelLabel.Location = new Point(10, 10);
            modelTextBox = new TextBox();
            modelTextBox.Location = new Point(120, 10);

            var yearLabel = new Label();
            yearLabel.Text = "Year: ";
            yearLabel.Location = new Point(10, 40);
            yearTextBox = new TextBox();
            yearTextBox.Location = new Point(120, 40);

            var engineModelLabel = new Label();
            engineModelLabel.Text = "Engine model: ";
            engineModelLabel.Location = new Point(10, 70);
            engineModelTextBox = new TextBox();
            engineModelTextBox.Location = new Point(120, 70);

            var engineHPLabel = new Label();
            engineHPLabel.Text = "Horse power: ";
            engineHPLabel.Location = new Point(10, 100);
            engineHPTextBox = new TextBox();
            engineHPTextBox.Location = new Point(120, 100);

            var displacementLabel = new Label();
            displacementLabel.Text = "Displacement: ";
            displacementLabel.Location = new Point(10, 130);
            displacementTextBox = new TextBox();
            displacementTextBox.Location = new Point(120, 130);

            var okButton = new Button();
            okButton.Text = "OK";
            okButton.Click += new System.EventHandler(this.OKButton_Click);


            var cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(144, 170);
            cancelButton.Click += new System.EventHandler(this.cancelButton_Click);

            okButton.Location = new Point(10, 170);
            dialog.Controls.Add(modelLabel);
            dialog.Controls.Add(modelTextBox);
            dialog.Controls.Add(yearLabel);
            dialog.Controls.Add(yearTextBox);
            dialog.Controls.Add(engineModelLabel);
            dialog.Controls.Add(engineModelTextBox);
            dialog.Controls.Add(engineHPLabel);
            dialog.Controls.Add(engineHPTextBox);
            dialog.Controls.Add(displacementLabel);
            dialog.Controls.Add(displacementTextBox);
            dialog.Controls.Add(okButton);
            dialog.Controls.Add(cancelButton);

            dialog.ShowDialog();
        }

        private void toolStripComboBox1_Enter(object sender, EventArgs e)
        {
            var comboBox = sender as ToolStripComboBox;
            if (comboBox != null)
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(new string[] { "Model", "Year", "EngineModel", "HorsePower", "Displacement" });
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string selectedProperty = toolStripComboBox1.SelectedItem as string;
            string searchValue = toolStripTextBox1.Text;

            if (string.IsNullOrEmpty(selectedProperty) || string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Please Enter a search value.");
                return;
            }

            var filteredCars = myCars.Where(car =>
            {
                switch (selectedProperty)
                {
                    case "Model":
                        return car.Model.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0;
                    case "Year":
                        return car.Year.ToString().Contains(searchValue);
                    case "EngineModel":
                        return car.Engine.Model.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0;
                    case "HorsePower":
                        return car.Engine.HorsePower.ToString().Contains(searchValue);
                    case "Displacement":
                        return car.Engine.Displacement.ToString().Contains(searchValue);
                    default:
                        return false;
                }
            }).ToList();

            myCarsBindingList = new SortableSearchableBindingList<Car>(filteredCars);
            dataGridView1.DataSource = myCarsBindingList;
        }


        public void cancelButton_Click(object sender, EventArgs e)
        {
            dialog.Close();
        }

        public void OKButton_Click(object sender, EventArgs e)
        {
            string model = modelTextBox.Text;
            string year = yearTextBox.Text;
            string engineModel = engineModelTextBox.Text;
            string engineHP = engineHPTextBox.Text;
            string displacement = displacementTextBox.Text;
            myCarsBindingList.Add(new Car(model, new Engine(double.Parse(displacement), int.Parse(engineHP), 
                engineModel), int.Parse(year)));
            dialog.Close();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedIndices = new List<int>();
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    selectedIndices.Add(row.Index);
                }

                foreach (int index in selectedIndices)
                {
                    if (index != -1)
                    {
                        myCarsBindingList.RemoveAt(index);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select cars to remove.");
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string searchText = searchBar.Text.ToLower();

            var sortedCars = myCarsBindingList.OrderByDescending(car =>
                car.Model.ToLower().Contains(searchText) ||
                car.Engine.Model.ToLower().Contains(searchText) ||
                car.Engine.HorsePower.ToString().Contains(searchText) ||
                car.Engine.Displacement.ToString().Contains(searchText) ||
                car.Year.ToString().Contains(searchText)).ToList();

            myCarsBindingList = new SortableSearchableBindingList<Car>(sortedCars);
            dataGridView1.DataSource = myCarsBindingList;
        }

        private void Ex1()
        {
            var elementsQuery = from car in myCarsBindingList
                                where car.Model == "A6"
                                let engineType = car.Engine.Model == "TDI" ? "diesel" : "petrol"
                                group car by engineType into carGroup
                                select new
                                {
                                    engineType = carGroup.Key,
                                    avgHPPL = carGroup.Average(car => car.Engine.HorsePower / car.Engine.Displacement)
                                } into result
                                orderby result.avgHPPL descending
                                select result;

            foreach (var e in elementsQuery)
            {
                Console.WriteLine(e.engineType + ": " + e.avgHPPL);
            }

            var elementsMethod = myCarsBindingList
                .Where(car => car.Model == "A6")
                .Select(car => new
                {
                    car,
                    engineType = car.Engine.Model == "TDI" ? "diesel" : "petrol"
                })
                .GroupBy(x => x.engineType, x => x.car)
                .Select(g => new
                {
                    engineType = g.Key,
                    avgHPPL = g.Average(car => car.Engine.HorsePower / car.Engine.Displacement)
                })
                .OrderByDescending(result => result.avgHPPL);

            foreach (var e in elementsMethod)
            {
                Console.WriteLine(e.engineType + ": " + e.avgHPPL);
            }
        }


        private void Ex2()
        {
            Comparison<Car> arg1 = new Comparison<Car>(CompareCarsByHorsePowerDescending);
            Predicate<Car> arg2 = new Predicate<Car>(IsEngineTDI);
            Action<Car> arg3 = new Action<Car>(ShowCarInMessageBox);

            myCars.Sort(arg1);
            myCars.FindAll(arg2).ForEach(arg3);
        }

        private int CompareCarsByHorsePowerDescending(Car car1, Car car2)
        {
            return car2.Engine.HorsePower.CompareTo(car1.Engine.HorsePower);
        }

        private bool IsEngineTDI(Car car)
        {
            return car.Engine.Model == "TDI";
        }

        private void ShowCarInMessageBox(Car car)
        {
            MessageBox.Show("Model: " + car.Model + "\nYear: " + car.Year + "\nEngineModel: " + car.Engine.Model + "\nHorsePower: " + car.Engine.HorsePower 
                + " \nDisplacement: " + car.Engine.Displacement );
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }
    }




    public class Car
    {
        public string Model { get; set; }
        public Engine Engine { get; set; }
        public int Year { get; set; }

        public Car() { }
        public Car(string model, Engine motor, int year)
        {
            Model = model;
            Engine = motor;
            Year = year;
        }
    }

    public class Engine
    {
        public double Displacement { get; set; }
        public int HorsePower { get; set; }
        public string Model { get; set; }

        public Engine() { }
        public Engine(double displacement, int horsePower, string model)
        {
            Displacement = displacement;
            HorsePower = horsePower;
            Model = model;
        }
        public string toString()
        {
            return "" + Model + "\n" + HorsePower +"\n" + Displacement;
        }
    }

    public class SortableSearchableBindingList<T> : BindingList<T>
    {
        private PropertyDescriptor _sortProperty;
        private ListSortDirection _sortDirection;
        private List<T> _originalList;

        public SortableSearchableBindingList() : base()
        {
            _originalList = new List<T>();
        }

        public SortableSearchableBindingList(IList<T> list) : base(list)
        {
            _originalList = new List<T>(list);
        }

        protected override bool SupportsSortingCore => true;
        protected override bool SupportsSearchingCore => true;

        protected override ListSortDirection SortDirectionCore => _sortDirection;

        protected override PropertyDescriptor SortPropertyCore => _sortProperty;

        protected override bool IsSortedCore => _sortProperty != null;

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            if (prop.PropertyType.GetInterface(nameof(IComparable)) == null)
            {
                throw new NotSupportedException($"Cannot sort by {prop.Name}. {prop.PropertyType.Name} does not implement IComparable.");
            }

            _sortProperty = prop;
            _sortDirection = direction;

            var sortedList = this.Items.ToList();
            if (direction == ListSortDirection.Ascending)
            {
                sortedList = sortedList.OrderBy(x => prop.GetValue(x)).ToList();
            }
            else
            {
                sortedList = sortedList.OrderByDescending(x => prop.GetValue(x)).ToList();
            }

            this.ClearItems();
            foreach (var item in sortedList)
            {
                this.Add(item);
            }

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            if (_originalList.Count == 0) return;

            this.ClearItems();
            foreach (var item in _originalList)
            {
                this.Add(item);
            }

            _sortProperty = null;
            _sortDirection = ListSortDirection.Ascending;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            for (int i = 0; i < this.Count; i++)
            {
                var item = this[i];
                if (prop.PropertyType == typeof(string))
                {
                    if ((string)prop.GetValue(item) == (string)key)
                    {
                        return i;
                    }
                }
                else if (prop.PropertyType == typeof(int))
                {
                    if ((int)prop.GetValue(item) == (int)key)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }

}

