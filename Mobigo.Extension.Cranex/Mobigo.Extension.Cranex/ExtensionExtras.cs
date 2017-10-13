using System.Windows.Forms;
using Mobigo.Core.Automation;

namespace Mobigo.Extension.Cranex
{
    public class ExtensionExtras
    {
        private IPlatform _platform;
        
        public ExtensionExtras(IPlatform platform)
        {
            _platform = platform;
        }
        public void ShowMsg(string msg)
        {
            if (_platform.AllowUI) MessageBox.Show(msg);
        }
    }
}
