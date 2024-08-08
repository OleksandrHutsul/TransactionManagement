namespace TransactionManagement.BLL.Interfaces
{
    public interface ITransaction
    {
        public Task UploadingCSVFile(Stream stream);
    }
}