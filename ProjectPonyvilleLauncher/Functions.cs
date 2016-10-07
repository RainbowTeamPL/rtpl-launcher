using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPonyvilleLauncher.Functions
{
    public class Functions
    {
        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public static void SetAccessRule(string directory)
        {
            try
            {
                System.Security.AccessControl.DirectorySecurity sec = System.IO.Directory.GetAccessControl(directory);
                FileSystemAccessRule accRule = new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.FullControl, AccessControlType.Allow);
                sec.AddAccessRule(accRule);
            }
            catch { }
        }
    }
}