namespace Cooktel_E_commrece.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFile(IFormFile file, string[] allowExtensions);

        void DeleteFile(string file);
    }
}
