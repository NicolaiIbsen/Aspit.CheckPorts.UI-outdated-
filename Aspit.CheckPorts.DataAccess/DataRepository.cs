using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspit.CheckPorts.Entities;
using System.Data;
using System.Collections.ObjectModel;

namespace Aspit.CheckPorts.DataAccess
{
    public class DataRepository
    {
        #region Fields
        /// <summary>The executor.</summary>
        private Executor executor;
        #endregion


        #region Constructor 
        /// <summary>Creates a new <see cref="DataRepository"/> object with the specified executor</summary>
        /// <param name="Executor">The excutor.</param>
        public DataRepository()
        {
            Executor = new Executor();
        }
        #endregion


        #region Properties
        /// <summary>Gets or sets the email.</summary>
        public Executor Executor { get => executor; set => executor = value; }
        #endregion


        #region Methods
        public List<Room> GetAllRooms()
        {
            List<Room> rooms = new List<Room>();
            rooms.Add(new Room("All"));
            string sql = "SELECT * FROM Room";
            DataSet set = Executor.Execute(sql);
            DataTable table = set.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                string roomName = (string)row["RoomName"];
                rooms.Add(new Room(roomName));
            }
            return rooms;
        }
        public List<string> GetAllPortSpecifiers()
        {
            List<string> portSpecifier = new List<string>();
            portSpecifier.Add("All");
            string sql = "SELECT PortSpecifier FROM Port";
            DataSet set = Executor.Execute(sql);
            DataTable table = set.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                string PortSpecifier = (string)row["PortSpecifier"];
                portSpecifier.Add(PortSpecifier);
            }
            return portSpecifier.Distinct().ToList();
        }
        public List<Port> GetAllPorts()
        {
            List<Port> ports = new List<Port>();
            string sql = "SELECT * FROM Port";
            DataSet set = Executor.Execute(sql);
            DataTable table = set.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                string portSpecifier = (string)row["PortSpecifier"];
                int portNumber = (int)row["PortNumber"];
                bool activity = (bool)row["Activity"];
                ports.Add(new Port(portSpecifier, portNumber, activity));
            }
            return ports;
        }
        public List<Port> GetPortsByRoomName(string roomName)
        {
            List<Port> ports = new List<Port>();
            string sql = $"SELECT * FROM Port WHERE (SELECT RoomId FROM Room WHERE RoomName = '{roomName}') = RoomId";
            DataSet set = Executor.Execute(sql);
            DataTable table = set.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                string portSpecifier = (string)row["PortSpecifier"];
                int portNumber = (int)row["PortNumber"];
                bool activity = (bool)row["Activity"];
                ports.Add(new Port(portSpecifier, portNumber, activity));
            }
            return ports;
        }
        public List<Port> GetPortsByPortSpecifier(string specifier)
        {
            List<Port> ports = new List<Port>();
            string sql = $"SELECT * FROM Port WHERE PortSpecifier = '{specifier}'";
            DataSet set = Executor.Execute(sql);
            DataTable table = set.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                string portSpecifier = (string)row["PortSpecifier"];
                int portNumber = (int)row["PortNumber"];
                bool activity = (bool)row["Activity"];
                ports.Add(new Port(portSpecifier, portNumber, activity));
            }
            return ports;
        }
        public List<Port> GetPortsByRoomNameAndSpecifier(string roomName, string specifier)
        {
            List<Port> ports = new List<Port>();
            string sql = $"SELECT * FROM Port WHERE (SELECT RoomId FROM Room WHERE RoomName = '{roomName}') = RoomId AND PortSpecifier = '{specifier}'";
            DataSet set = Executor.Execute(sql);
            DataTable table = set.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                string portSpecifier = (string)row["PortSpecifier"];
                int portNumber = (int)row["PortNumber"];
                bool activity = (bool)row["Activity"];
                ports.Add(new Port(portSpecifier, portNumber, activity));

            }
            return ports;
        }
        public string SqlCreater(ObservableCollection<Port> ports)
        {
            string sql = "";
            foreach (Port port in ports)
            {
                sql += $"UPDATE Port SET Activity = '{port.Activity}' WHERE PortSpecifier = '{port.PortSpecifier}' AND PortNumber = {port.PortNumber};";
            }
            return sql;
        }

        public string SqlInsert(ObservableCollection<Port> ports, string roomName)
        {
            string sql = "INSERT INTO Port (PortSpecifier, PortNumber, RoomId, Activity) VALUES";
            foreach (Port port in ports)
            {
                sql += $"('{port.PortSpecifier}', {port.PortNumber}, (SELECT RoomId FROM Room WHERE RoomName = '{roomName}'), '{port.Activity}'),";
            }
            return sql.TrimEnd(',');
        }

        public string SqlInsertNewRoom(string roomName)
        {
            return $"INSERT INTO Room(RoomName) VALUES('{roomName}')";
        }
        #endregion
    }
}
