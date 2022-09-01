using Model.Models.ServidorModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Util.Mensagens;

namespace View.LoginView.ConfigServidor
{
    /// <summary>
    /// Interação lógica para Page1.xam
    /// </summary>
    public partial class Page1 : UserControl
    {
        WinServidor winServidor = null;

        public Page1()
        {
            InitializeComponent();
        }

        private void Salvar()
        {
            if(cbxEmpresa.SelectedValue == null)
            {
                MessageBox.Show("Informe a empresa", Mensagem.ObterMensagem(2), MessageBoxButton.OK, MessageBoxImage.Warning);
                cbxEmpresa.Focus();
                return;
            }

            if (Directory.Exists(Directory.GetParent(winServidor.diretorioArquivo).FullName) == false)
            {
                Directory.CreateDirectory(Directory.GetParent(winServidor.diretorioArquivo).FullName);
            }

            var servidor = winServidor.servidores.First(f => f.Id == Convert.ToInt32(cbxEmpresa.SelectedValue));

            var jsonString = JsonSerializer.Serialize<Servidor>(servidor);
            File.WriteAllText(System.IO.Path.Combine(Directory.GetParent(winServidor.diretorioArquivo).FullName, "Servidor.json"), jsonString);
            winServidor.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.winServidor = (WinServidor)Window.GetWindow(this);
        }

        private void cbxEmpresa_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Salvar();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            winServidor.page2.tbxDescricao.Text = string.Empty;
            winServidor.page2.tbxHostName.Text = string.Empty;
            winServidor.page2.tbxPorta.Text = string.Empty;          
            winServidor.page2.tbxDataBase.Text = string.Empty;

            winServidor.servidor = new Servidor();
            winServidor.servidor.Id = winServidor.servidores.Count > 0 ? winServidor.servidores.Max(f => f.Id) + 1 : 1;
            winServidor.operacao = Util.Enumeradores.EnumOperacao.Inserindo;
            winServidor.pageSlider.SelectedIndex = 1;
            winServidor.page2.tbxDescricao.Focus();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            winServidor.servidor = winServidor.servidores.First(f => f.Id == Convert.ToInt32(cbxEmpresa.SelectedValue));
            winServidor.page2.tbxDescricao.Text = winServidor.servidor.Descricao;
            winServidor.page2.tbxHostName.Text = winServidor.servidor.HostName;
            winServidor.page2.tbxPorta.Text = winServidor.servidor.Porta;           
            winServidor.page2.tbxDataBase.Text = winServidor.servidor.DataBase;

            winServidor.operacao = Util.Enumeradores.EnumOperacao.Alterando;
            winServidor.pageSlider.SelectedIndex = 1;
            winServidor.page2.tbxDescricao.Focus();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Deseja excluir a a empresa?", Mensagem.ObterMensagem(2), MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var item = winServidor.servidores.First(f => f.Id == Convert.ToInt32(cbxEmpresa.SelectedValue));
                winServidor.servidores.Remove(item);
            }
        }

        private void btnCancelarEmpresa_Click(object sender, RoutedEventArgs e)
        {
            winServidor.Close();
        }

        private void btnSalvarEmpresa_Click(object sender, RoutedEventArgs e)
        {
            Salvar();
        }
    }
}
