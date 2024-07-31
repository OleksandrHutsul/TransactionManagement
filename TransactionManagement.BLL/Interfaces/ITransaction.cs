namespace TransactionManagement.BLL.Interfaces
{
    internal interface ITransaction
    {
        public Task DownloadingCSVFile(Stream stream);
    }
}