using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using model = Model.Models;

namespace ViewModel.Login
{
    public class LoginViewModel
    {
        public static bool VerificarUsuario(model.LoginModel.Login login)
        {
            try
            {
                string url = $"{Base.Servidor.HostName}:{Base.Servidor.Porta}/api/login/verificarUsuario";

                var response = Connect.Get(url, login);

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public static void RetornaUsuarios()
        {
            try
            {
                string url = $"{Base.Servidor.HostName}:{Base.Servidor.Porta}/api/login/retornaUsuarios";

                var response = Connect.Get(url, null);

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var dados = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch
            {
                
            }
        }
    }
}
