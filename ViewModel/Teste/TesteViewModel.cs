using Model.Models.BaseDadosModel;
using Model.Models.LoginModel;
using Model.Models.ServidorModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ViewModel.Teste
{
    public static class TesteViewModel
    {
        public static bool TestarConexao(Servidor servidor)
        {
            try
            {
                string url = $"{servidor.HostName}:{servidor.Porta}/api/teste/testarConexao";                
                
                var response = Connect.Get(url, null);

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public static bool TestarDataBase(Servidor servidor)
        {
            try
            {
                string url = $"{servidor.HostName}:{servidor.Porta}/api/teste/testarDataBase";                

                BaseDados baseDados = new BaseDados();
                baseDados.Nome = servidor.DataBase;

                var response = Connect.Get(url, baseDados);

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
    }
}
