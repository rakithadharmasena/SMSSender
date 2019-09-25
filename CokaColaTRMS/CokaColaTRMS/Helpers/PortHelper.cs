using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace CokaColaTRMS.Helpers
{
    public class PortHelper
    {
        public static List<string> GetModemPorts()
        {
            try
            {
                List<string> portNames = new List<string>();
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_POTSModem");
                
                foreach (ManagementObject mo in mos.Get())
                {
                  
                    // for modem port
                    portNames.Add(mo["AttachedTo"].ToString());
                }

                return portNames;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}
