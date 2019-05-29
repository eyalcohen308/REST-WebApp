using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;

namespace newEx3.Models
{
    public class InfoModel
    {
        private static InfoModel s_instace = null;

        public static InfoModel Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new InfoModel();
                }
                return s_instace;
            }
        }

        //public Employee Employee { get; private set; }
        // defult values to null.
        public string ip { get; set; } = null;
        public string port { get; set; } = null;
        public int time { get; set; }
        public InfoModel()
        {
        //    Employee = new Employee();
        }

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario
        public void OpenServer()
        {
            if(ip==null || port == null) { return; }
            Instructions.getInstance.open(ip, Int32.Parse(port));
        }
        public double GetLon()
        {
            if (Instructions.getInstance.isOpen())
            {
                return Instructions.getInstance.GetParamValue("lon");
            }
            throw new System.InvalidOperationException("try getting lon without open the server");
        }
        public double GetLat()
        {
            if (Instructions.getInstance.isOpen())
            {
                return Instructions.getInstance.GetParamValue("lat");
            }
            throw new System.InvalidOperationException("try getting lat without open the server");
        }
        public void ReadData(string name)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, name));
            if (!File.Exists(path))
            {
                //Employee.FirstName = name;
                //Employee.LastName = name;
                //Employee.Salary = 500;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    //file.WriteLine(Employee.FirstName);
                    //file.WriteLine(Employee.LastName);
                    //file.WriteLine(Employee.Salary);
                }
            }
            else
            {
                string[] lines = System.IO.File.ReadAllLines(path);        // reading all the lines of the file
                //Employee.FirstName = lines[0];
                //Employee.LastName = lines[1];
                //Employee.Salary = int.Parse(lines[2]);
            }
        }

    }
}