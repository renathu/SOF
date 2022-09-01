using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace View.AreaTrabalhoView
{
    /// <summary>
    /// Lógica interna para WinAreaTrabalho.xaml
    /// </summary>
    public partial class WinAreaTrabalho : Window
    {
        public WinAreaTrabalho()
        {
            InitializeComponent();

            Thread.Sleep(8000);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            
        }
    }
}
