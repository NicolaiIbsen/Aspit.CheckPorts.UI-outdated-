using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspit.CheckPorts.Entities
{
    public class Room
    {
        #region Fields
        private int id;
        private string roomName;
        #endregion


        #region Constructors
        public Room(int id, string room)
        {
            Id = id;
            RoomName = room;
        }
        public Room(string room)
        {
            RoomName = room;
        }
        #endregion


        #region Properties
        public int Id { get => id; set => id = value; }
        public string RoomName { get => roomName; set => roomName = value; }
        #endregion


        #region Methods
        public override string ToString()
        {
            return $"{roomName}";
        }
        #endregion
    }
}
