using System;
using System.Collections.Generic;
using System.Text;

namespace Util.Mensagens
{
    public static class Mensagem
    {
        private static Dictionary<int, string> dictMensagens = new Dictionary<int, string>();
        public static string ObterMensagem(int codigo)
        {
            if(dictMensagens.Count == 0)
            {
                CarregarMensagens();
            }

            string mensagem = string.Empty;

            dictMensagens.TryGetValue(codigo, out mensagem);

            return mensagem;
        }

        private static void CarregarMensagens()
        {
            dictMensagens.Add(1, "Campos não preenchidos ou inválidos");
            dictMensagens.Add(2, "Atenção");
            dictMensagens.Add(3, "Sucesso");
        }
    }
}
