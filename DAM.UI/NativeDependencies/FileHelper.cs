using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAM.NativeDependencies;
using Microsoft.Win32;

namespace DAM.UI.NativeDependencies
{
    public class FileHelper : IFileHelper
    {
        public string GetSolutionPath()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Solution file (*.sln)|*.sln";
            if (ofd.ShowDialog() == true)
            {
                return ofd.FileName;
            }

            return string.Empty;
        }
    }
}
