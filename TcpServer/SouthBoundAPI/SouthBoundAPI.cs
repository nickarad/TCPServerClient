using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SouthBoundAPI
{
    public class IoTNode
    {
        public string Name;
        public int State;
        public string SerialNumber;
        public string Manufacturer;
        public string Firmware;
        public List<IoTThings> Things;

    }

    public class IoTThings

    {
        public string Name;
        public Functionality Funcionality;
        public string State;
        public string Manufacturer;
        public string Firmware;

    }

    public class Registration
    {
        public IoTNode Node;

    }
    public class ReportValue
    {
        public string SerialNumber;
        public string Name;
        public Functionality func;
        public string State;


    }
    public class UpdateValue
    {

    }

    public enum Functionality
    {
        Temparture,
        Humidity,
        Co2
    }

}
