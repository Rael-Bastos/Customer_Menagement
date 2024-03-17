namespace Domain.Ports.Services
{
    public interface IFileAtapter
    {
        Task GenerateFileCSV<T>(List<T> dados, string caminhoArquivo,string campoIgnorado);
    }
}
