using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Ports.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Infra.Documents.Operations
{
    public class FileAtapter : IFileAtapter
    {
        
        public async Task GenerateFileCSV<T>(List<T> dados, string caminhoArquivo, string campoIgnorado)
        {
            // Verifique se há dados
            if (dados == null || dados.Count == 0)
            {
                throw new ArgumentException("Nenhum dado fornecido para gerar o arquivo.");
            }

            // Obter o tipo do objeto
            Type tipoObjeto = typeof(T);

            // Obter todas as propriedades públicas do objeto, excluindo a propriedade a ser ignorada
            PropertyInfo[] propriedades = tipoObjeto.GetProperties()
                                                     .Where(p => p.Name != campoIgnorado)
                                                     .ToArray();

            // Obter os cabeçalhos dos CSV
            var cabecalhos = propriedades.Select(p => p.Name);

            // Crie um arquivo CSV
            using (var streamWriter = new StreamWriter(caminhoArquivo))
            {
                // Escreva cabeçalhos
                var linhaCabecalho = string.Join(",", cabecalhos);
                await streamWriter.WriteLineAsync(linhaCabecalho);

                // Escreva dados
                foreach (var objeto in dados)
                {
                    // Obter os valores das propriedades para este objeto
                    var valoresPropriedades = propriedades.Select(p => p.GetValue(objeto)).ToArray();

                    // Converta os valores das propriedades para strings e os junte com vírgula
                    var linha = string.Join(",", valoresPropriedades);

                    // Escreva a linha no arquivo
                    await streamWriter.WriteLineAsync(linha);
                }
            }
        }
    }
}
