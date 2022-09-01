using MaterialDesignThemes.Wpf;
using Model.Models.LoginModel;
using Model.Models.ServidorModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Util.Mensagens;
using View.AreaTrabalhoView;
using View.LoginView.ConfigServidor;
using ViewModel.Login;
using ViewModel.Teste;

namespace View.LoginView
{
    /// <summary>
    /// Lógica interna para WinLogin.xaml
    /// </summary>
    public partial class WinLogin : Window
    {
        public WinLogin()
        {
            InitializeComponent();
        }

        private void Entrar()
        {
            Servidor servidor = null;

            var diretorioServidor = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Json", "Servidor.json");
            if (File.Exists(diretorioServidor))
            {
                string jsonString = File.ReadAllText(diretorioServidor);

                servidor = JsonSerializer.Deserialize<Servidor>(jsonString);
            }
            else
            {
                MessageBox.Show(this, "Informe o servidor", Mensagem.ObterMensagem(2), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (tbxUsuario.Text == string.Empty)
            {
                tbxUsuario.Focus();
                MessageBox.Show(this, "Informe o usuário", Mensagem.ObterMensagem(2), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TesteViewModel.TestarConexao(servidor) == false)
            {
                MessageBox.Show(this, "Não foi possivel estabelecer conexão com o servidor", Mensagem.ObterMensagem(2), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TesteViewModel.TestarDataBase(servidor) == false)
            {
                MessageBox.Show(this, "Não foi possivel estabelecer conexão com o banco de dados", Mensagem.ObterMensagem(2), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ViewModel.Base.Servidor = servidor;

            Login login = new Login();
            login.DataBase = servidor.DataBase;
            login.Usuario = tbxUsuario.Text;
            login.Senha = tbxSenha.Password;

            if (LoginViewModel.VerificarUsuario(login) == false)
            {
                MessageBox.Show(this, "Usuário ou senha está incorreto", Mensagem.ObterMensagem(2), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //dialogLoading.IsOpen = true;

            LoginViewModel.RetornaUsuarios();

            WinAreaTrabalho winAreaTrabalho = new WinAreaTrabalho();
            winAreaTrabalho.ContentRendered += WinAreaTrabalho_ContentRendered;
            winAreaTrabalho.Closed += WinAreaTrabalho_Closed;
            winAreaTrabalho.Owner = this;
            winAreaTrabalho.Show();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            string diretorioArquivo = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Json", "Login.json");
            if (File.Exists(diretorioArquivo))
            {
                string jsonString = File.ReadAllText(diretorioArquivo);

                var login = JsonSerializer.Deserialize<Login>(jsonString);
                tbxUsuario.Text = login.Usuario;
                tbxSenha.Password = login.Senha;
                chBoxLembrar.IsChecked = true;

                btnEntrar.Focus();
            }
            else
            {
                tbxUsuario.Focus();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string diretorioArquivo = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Json", "Login.json");

            if (chBoxLembrar.IsChecked == true)
            {
                Login login = new Login();
                login.Usuario = tbxUsuario.Text;
                login.Senha = tbxSenha.Password;

                if (Directory.Exists(Directory.GetParent(diretorioArquivo).FullName) == false)
                {
                    Directory.CreateDirectory(Directory.GetParent(diretorioArquivo).FullName);
                }

                var jsonString = JsonSerializer.Serialize<Login>(login);
                File.WriteAllText(diretorioArquivo, jsonString);
            }
            else
            {
                if (File.Exists(diretorioArquivo))
                {
                    File.Delete(diretorioArquivo);
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            Entrar();
        }

        private void btnServidor_Click(object sender, RoutedEventArgs e)
        {
            WinServidor winServidor = new WinServidor();
            winServidor.Owner = this;
            winServidor.ShowDialog();

            tbxUsuario.Focus();
        }

        private void btnSair_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void controle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Entrar();
            }
        }

        private void WinAreaTrabalho_ContentRendered(object sender, EventArgs e)
        {
            dialogLoading.IsOpen = false;
            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void WinAreaTrabalho_Closed(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
        }
    }
}
