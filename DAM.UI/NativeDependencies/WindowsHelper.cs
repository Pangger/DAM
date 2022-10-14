using DAM.NativeDependencies;
using System.Windows;

namespace DAM.UI.NativeDependencies
{
    public class WindowsHelper : IWindowsHelper
    {
        public void SaveToClipboard(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
