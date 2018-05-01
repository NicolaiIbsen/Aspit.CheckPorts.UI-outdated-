using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspit.CheckPorts.Entities
{
    public class Port
    {
        #region Fields
        private int id;
        private string portSpecifier;
        private int portNumber;
        private int roomId;
        private bool activity;
        #endregion

        #region Constructors
        /*public Port(string portSpecifier, int portNumber, int roomId, bool activity)
        {
            PortX = portSpecifier;
            PortTal = portNumber;
            int RoomId = roomId;
            Aktiv = activity;
        }*/
        public Port(string portSpecifier, int portNumber, bool activity)
        {
            PortSpecifier = portSpecifier;
            PortNumber = portNumber;
            Activity = activity;
        }
        public Port(string portSpecifier)
        {
            PortSpecifier = portSpecifier;
        }
        /*public Port(string portSpecifier, string portNumber, int roomId, bool activity)
        {
            PortSpecifier = portSpecifier;
            PortNumber = portNumber;
            RoomId = roomId;
            Activity = activity;
        }*/
        #endregion

        #region Properties
        //public int Id { get => id; set => id = value; }
        public string PortSpecifier { get => portSpecifier; set => portSpecifier = value; }
        public int PortNumber { get => portNumber; set => portNumber = value; }
        //public int RoomId { get => roomId; set => roomId = value; }
        public bool Activity { get => activity; set => activity = value; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{portSpecifier} {portNumber} {activity} ";
        }
        public string PortSpecfierToString()
        {
            return $"{portSpecifier}";
        }
        #endregion
    }
}
