using MaterialDesignThemes.Wpf.Transitions;
using Model.Models.LoginModel;
using Model.Models.ServidorModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
using ViewModel.Teste;

namespace View.LoginView.ConfigServidor
{
    /// <summary>
    /// Interação lógica para Page2.xam
    /// </summary>
    public partial class Page2 : UserControl
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        WinServidor winServidor = null;

        public Page2()
        {
            InitializeComponent();
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private bool Validar()
        {
            StringBuilder sbMensagem = new StringBuilder();
            sbMensagem.AppendLine(Mensagem.ObterMensagem(1));

            Control controle = null;

            if (tbxDescricao.Text == string.Empty)
            {
                if (controle == null)
                {
                    controle = tbxDescricao;
                }
                tbxDescricao.BorderBrush = new SolidColorBrush(MaterialDesignThemes.Wpf.Theme.Light.ValidationErrorColor);
                sbMensagem.AppendLine("Informe a descrição");
            }
            else
            {
                tbxDescricao.BorderBrush = new SolidColorBrush(MaterialDesignThemes.Wpf.Theme.Light.MaterialDesignTextBoxBorder);
            }

            if (tbxHostName.Text == string.Empty)
            {
                if (controle == null)
                {
                    controle = tbxHostName;
                }
                tbxHostName.BorderBrush = new SolidColorBrush(MaterialDesignThemes.Wpf.Theme.Light.ValidationErrorColor);
                sbMensagem.AppendLine("Informe o hostname");
            }
            else
            {
                tbxHostName.BorderBrush = new SolidColorBrush(MaterialDesignThemes.Wpf.Theme.Light.MaterialDesignTextBoxBorder);
            }

            if (tbxPorta.Text == string.Empty)
            {
                if (controle == null)
                {
                    controle = tbxPorta;
                }
                tbxPorta.BorderBrush = new SolidColorBrush(MaterialDesignThemes.Wpf.Theme.Light.ValidationErrorColor);
                sbMensagem.AppendLine("Informe a porta");
            }
            else
            {
                tbxPorta.BorderBrush = new SolidColorBrush(MaterialDesignThemes.Wpf.Theme.Light.MaterialDesignTextBoxBorder);
            }

            if (tbxDataBase.Text == string.Empty)
            {
                if (controle == null)
                {
                    controle = tbxDataBase;
                }
                tbxDataBase.BorderBrush = new SolidColorBrush(MaterialDesignThemes.Wpf.Theme.Light.ValidationErrorColor);
                sbMensagem.AppendLine("Informe a DB");
            }
            else
            {
                tbxDataBase.BorderBrush = new SolidColorBrush(MaterialDesignThemes.Wpf.Theme.Light.MaterialDesignTextBoxBorder);
            }

            if (controle != null)
            {
                MessageBox.Show(this.winServidor, sbMensagem.ToString(), Mensagem.ObterMensagem(2), MessageBoxButton.OK, MessageBoxImage.Warning);
                controle.Focus();
                return false;
            }

            return true;
        }

        private void Salvar()
        {
            if (Validar())
            {
                if (winServidor.operacao == Util.Enumeradores.EnumOperacao.Inserindo)
                {
                    winServidor.servidores.Add(winServidor.servidor);
                }
                winServidor.servidor.Descricao = tbxDescricao.Text;
                winServidor.servidor.HostName = tbxHostName.Text;
                winServidor.servidor.Porta = tbxPorta.Text;               
                winServidor.servidor.DataBase = tbxDataBase.Text;

                if (Directory.Exists(Directory.GetParent(winServidor.diretorioArquivo).FullName) == false)
                {
                    Directory.CreateDirectory(Directory.GetParent(winServidor.diretorioArquivo).FullName);
                }

                var jsonString = JsonSerializer.Serialize<ObservableCollection<Servidor>>(winServidor.servidores);
                File.WriteAllText(winServidor.diretorioArquivo, jsonString);

                //Alteração no servidor selecionado
                string diretorioArquivoServidor = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Json", "Servidor.json");
                if (File.Exists(diretorioArquivoServidor))
                {
                    string jsonServidor = File.ReadAllText(diretorioArquivoServidor);

                    var servidor = JsonSerializer.Deserialize<Servidor>(jsonServidor);
                    if(servidor.Id == winServidor.servidor.Id)
                    {
                        jsonServidor = JsonSerializer.Serialize<Servidor>(winServidor.servidor);
                        File.WriteAllText(diretorioArquivoServidor, jsonServidor);
                    }
                }

                winServidor.operacao = Util.Enumeradores.EnumOperacao.None;
                winServidor.pageSlider.SelectedIndex = 0;
                winServidor.page1.cbxEmpresa.Focus();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.winServidor = (WinServidor)Window.GetWindow(this);
        }

        private void btnTestarConexao_Click(object sender, RoutedEventArgs e)
        {
            if (Validar())
            {
                Servidor servidor = new Servidor();
                servidor.Descricao = tbxDescricao.Text;
                servidor.HostName = tbxHostName.Text;
                servidor.Porta = tbxPorta.Text;
                servidor.DataBase = tbxDataBase.Text;

                if (TesteViewModel.TestarConexao(servidor) == false)
                {
                    MessageBox.Show(this.winServidor, "Não foi possivel estabelecer conexão com o servidor", Mensagem.ObterMensagem(2), MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (TesteViewModel.TestarDataBase(servidor) == false)
                {
                    MessageBox.Show(this.winServidor, "Não foi possivel estabelecer conexão com o banco de dados", Mensagem.ObterMensagem(2), MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                MessageBox.Show(this.winServidor, "Conectado com sucesso", Mensagem.ObterMensagem(3), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void tbxPorta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void tbxPorta_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void tbxControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Salvar();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            winServidor.operacao = Util.Enumeradores.EnumOperacao.None;
            winServidor.pageSlider.SelectedIndex = 0;
            winServidor.page1.cbxEmpresa.Focus();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            Salvar();
        }
    }
}
