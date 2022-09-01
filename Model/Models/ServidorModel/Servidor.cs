using Model.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Models.ServidorModel
{
    public class Servidor : INotifyPropertyChanged, IModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        private int id = 0;

        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private string descricao = string.Empty;

        public string Descricao
        {
            get { return descricao; }
            set
            {
                if (descricao != value)
                {
                    descricao = value;
                    OnPropertyChanged("Descricao");
                }
            }
        }

        private string hostName = string.Empty;

        public string HostName
        {
            get { return hostName; }
            set
            {
                if (hostName != value)
                {
                    hostName = value;
                    OnPropertyChanged("HostName");
                }
            }
        }

        private string porta = string.Empty;

        public string Porta
        {
            get { return porta; }
            set
            {
                if (porta != value)
                {
                    porta = value;
                    OnPropertyChanged("Porta");
                }
            }
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
                    OnPropertyChanged("DataBase");
                }
            }
        }
    }
}
