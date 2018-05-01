using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aspit.CheckPorts.DataAccess;
using Aspit.CheckPorts.Entities;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace Aspit.CheckPorts.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataRepository repository = new DataRepository();
        ObservableCollection<Port> fixedPorts = new ObservableCollection<Port>();
        ObservableCollection<Port> newPorts = new ObservableCollection<Port>();
        List<Room> rooms = new List<Room>();
        string fileLocation = @"C:\Nymappe\omegalul.txt";

        public MainWindow()
        {
            InitializeComponent();
            UpdateComboBoxesAdd();
        }


        #region "Oversigt" Tab
        private void ComboBoxRoomName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabcontrol.SelectedIndex == 0)
            {
                myDatagrid.ItemsSource = repository.GetPortsByRoomName(ComboBoxRoomName.SelectedItem.ToString());
            }

            if (ComboBoxRoomName.SelectedIndex == 0)
            {
                myDatagrid.ItemsSource = repository.GetAllPorts();
            }
        }

        private void ButtonAllPorts_Click(object sender, RoutedEventArgs e)
        {
            PrintPortStatus();
        }
        private void MyDatagrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            fixedPorts.Add((Port)e.Row.Item);
            myDatagrid_Copy.ItemsSource = fixedPorts.Distinct().ToList();
            TextBlockEditedPorts.Text = $"Redigerede porte: {fixedPorts.Count} (Max: 355)";
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (fixedPorts.Count > 0)
            {                
                repository.Executor.Execute(repository.SqlCreater(fixedPorts));
                fixedPorts = new ObservableCollection<Port>();

                System.Windows.MessageBox.Show("Gemt");
            }
            else
            {
                System.Windows.MessageBox.Show("No Ports entered");
            }
        }

        private void ComboBoxPortSpecifier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDatagridWithComboBoxPortSpecifier();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            fixedPorts = new ObservableCollection<Port>();
            TextBlockEditedPorts.Text = $"Redigerede porte: {fixedPorts.Count} (Max: 355)";
            ComboBoxRoomName.SelectedIndex = 0;
            ComboBoxPortSpecifier.SelectedIndex = 0;
            UpdateDatagridWithComboBoxPortSpecifier();
            myDatagrid_Copy.ItemsSource = null;
        }

        #endregion


        #region "Tilføj" Tab
        private void ButtonTilføjTilføj_Click(object sender, RoutedEventArgs e)
        {
            newPorts.Add(new Port(TextBoxPortSpecifierTilføj.Text.ToUpper(), Convert.ToInt32(TextBoxPortNumberTilføj.Text), checkbox.IsChecked.Value));
            DatagridTilføj.ItemsSource = newPorts;
        }

        private void ButtonSaveTilføj_Click(object sender, RoutedEventArgs e)
        {
            if (newPorts.Count > 0)
            {
                repository.Executor.Execute(repository.SqlInsert(newPorts, ComboBoxRoomNameTilføj.Text));
            }
            else
            {
                System.Windows.MessageBox.Show("No Ports entered");
            }
        }

        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
            newPorts.Add(new Port(TextBoxPortSpecifierTilføj.Text.ToUpper(), Convert.ToInt32(TextBoxPortNumberTilføj.Text), checkbox.IsChecked.Value));
            DatagridTilføj.ItemsSource = newPorts;
            }
        }        

        private void ComboBoxRoomNameTilføj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxRoomNameTilføj.SelectedIndex == ComboBoxRoomNameTilføj.Items.Count - 1)
            {
                TextBoxNewRoom.Visibility = Visibility.Visible;
                ButtonNewRoomSave.Visibility = Visibility.Visible;
            }
            else
            {
                TextBoxNewRoom.Visibility = Visibility.Hidden;
                ButtonNewRoomSave.Visibility = Visibility.Hidden;
            }
        }

        private void ButtonNewRoomSave_Click(object sender, RoutedEventArgs e)
        {
            TextBoxNewRoom.Visibility = Visibility.Hidden;
            ButtonNewRoomSave.Visibility = Visibility.Hidden;
            repository.Executor.Execute(repository.SqlInsertNewRoom(TextBoxNewRoom.Text));
            UpdateComboBoxesAdd();
            /*ComboBoxRoomName.SelectedIndex = -1;
            ComboBoxRoomName.ItemsSource = repository.GetAllRooms();*/
        }
        #endregion


        #region Methods
        private void UpdateComboBoxesAdd()
        {
            rooms = repository.GetAllRooms();
            rooms.Add(new Room("Nyt Rum"));
            ComboBoxRoomNameTilføj.ItemsSource = rooms.Skip(1);
            ComboBoxRoomName.ItemsSource = repository.GetAllRooms();
            ComboBoxPortSpecifier.ItemsSource = repository.GetAllPortSpecifiers();
        }
        private void UpdateDatagridWithComboBoxPortSpecifier()
        {
            if (ComboBoxRoomName.SelectedIndex == 0 && ComboBoxPortSpecifier.SelectedIndex == 0)
            {
                myDatagrid.ItemsSource = repository.GetAllPorts();
            }
            else if (ComboBoxRoomName.SelectedIndex == 0 || ComboBoxRoomName.SelectedValue == null)
            {
                myDatagrid.ItemsSource = repository.GetPortsByPortSpecifier(ComboBoxPortSpecifier.SelectedItem.ToString());
            }
            else if (ComboBoxPortSpecifier.SelectedIndex == 0 || ComboBoxRoomName.SelectedIndex == 0)
            {
                myDatagrid.ItemsSource = repository.GetPortsByRoomName(ComboBoxRoomName.SelectedItem.ToString());
            }
            else if (ComboBoxPortSpecifier.SelectedIndex == 0)
            {
                myDatagrid.ItemsSource = repository.GetAllPorts();
            }
            else
            {
                myDatagrid.ItemsSource = repository.GetPortsByRoomNameAndSpecifier(ComboBoxRoomName.SelectedItem.ToString(), ComboBoxPortSpecifier.SelectedItem.ToString());
            }
        }
        public void PrintPortStatus()
        {

            string toDisplay = string.Join(Environment.NewLine, AllPortsFormatted());
            if (System.Windows.Forms.MessageBox.Show(toDisplay, "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
            {
                StreamWriter streamWriter = new StreamWriter(fileLocation);
                foreach (string formatedPort in AllPortsFormatted())
                {
                    streamWriter.WriteLine(formatedPort);
                }
                streamWriter.Close();
            }
            else
            {

            }
        }
        public string[] AllPortsFormatted()
        {
            List<string> GetAllPortSpecifier = repository.GetAllPortSpecifiers().Skip(1).ToList();
            List<Port> ports = repository.GetAllPorts();
            string[] portsString = new string[GetAllPortSpecifier.Count];
            int n = 0;
            int i = 0;
            while (n < portsString.Rank)
            {
                while (i < ports.Count)
                {
                    if (i > 0)
                    {
                        if (ports[i].PortSpecifier != ports[i - 1].PortSpecifier)
                        {
                            portsString[n] = portsString[n].TrimEnd(' ', '|');
                            portsString[n] += $"\n \n";
                            portsString[n + 1] += $"{ports[i].ToString()} | ";
                            n++;
                            i++;
                        }
                        else
                        {

                            portsString[n] += $"{ports[i].ToString()} | ";
                            i++;
                        }
                    }
                    else
                    {
                        portsString[n] += $"{ports[i].ToString()} | ";
                        i++;
                    }
                }
            }
            return portsString;
        }
        #endregion
    }
}
