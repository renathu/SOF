using Model.Models.ServidorModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Util.Enumeradores;

namespace View.LoginView.ConfigServidor
{
    /// <summary>
    /// Lógica interna para WinServidor.xaml
    /// </summary>
    public partial class WinServidor : Window
    {
        internal ObservableCollection<Servidor> servidores;

        internal Servidor servidor;

        internal string diretorioArquivo = string.Empty;

        internal EnumOperacao operacao = EnumOperacao.None;

        public WinServidor()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (pageSlider.SelectedIndex == 0)
                {
                    this.Close();
                }
                else
                {
                    operacao = Util.Enumeradores.EnumOperacao.None;
                    pageSlider.SelectedIndex = 0;
                    page1.cbxEmpresa.Focus();
                }
            }
        }        

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            page1.cbxEmpresa.Focus();

            diretorioArquivo = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Json", "Servidores.json");

            if (File.Exists(diretorioArquivo))
            {
                string jsonString = File.ReadAllText(diretorioArquivo);

                servidores = JsonSerializer.Deserialize<ObservableCollection<Servidor>>(jsonString);
            }
            else
            {
                servidores = new ObservableCollection<Servidor>();
            }
            servidores.CollectionChanged += Servidores_CollectionChanged;
            
            page1.cbxEmpresa.SelectedValuePath = "Id";
            page1.cbxEmpresa.DisplayMemberPath = "Descricao";
            page1.cbxEmpresa.ItemsSource = servidores;


            var diretorioServidor = System.IO.Path.Combine(Directory.GetParent(diretorioArquivo).FullName, "Servidor.json");
            if (File.Exists(diretorioServidor))
            {
                string jsonString = File.ReadAllText(diretorioServidor);

                servidor = JsonSerializer.Deserialize<Servidor>(jsonString);
                page1.cbxEmpresa.SelectedValue = servidor.Id;
            }

            if (servidores.Count == 0)
            {
                page1.btnEdit.IsEnabled = false;
                page1.btnDelete.IsEnabled = false;
            }
        }

        private void Servidores_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                page1.btnEdit.IsEnabled = true;
                page1.btnDelete.IsEnabled = true;
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove && servidores.Count == 0)
            {
                page1.btnEdit.IsEnabled = false;
                page1.btnDelete.IsEnabled = false;
            }
        }       
    }
}
