using Model.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Models.LoginModel
{
    public class Login : INotifyPropertyChanged, IModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        private string dataBase = string.Empty;

        public string DataBase
        {
            get { return dataBase; }
            set
            {
                if (dataBase != value)
                {
                    dataBase = value;
                    OnPropertyChanged("dataBase");
                }
            }
        }

        private string usuario = string.Empty;

        public string Usuario
        {
            get { return usuario; }
            set 
            {
                if (usuario != value)
                {
                    usuario = value;
                    OnPropertyChanged("Usuario");
                }
            }
        }

        private string senha = string.Empty;

        public string Senha
        {
            get { return senha; }
            set 
            {
                if (senha != value)
                {
                    senha = value;
                    OnPropertyChanged("Senha");
                }
            }
        }
    }
}
