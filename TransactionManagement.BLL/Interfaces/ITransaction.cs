namespace TransactionManagement.BLL.Interfaces
{
    internal interface ITransaction
    {
        public Task UploadingCSVFile(Stream stream);
    }
}