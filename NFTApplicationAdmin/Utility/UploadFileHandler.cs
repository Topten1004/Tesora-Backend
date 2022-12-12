using Org.BouncyCastle.Asn1.Ocsp;
using System.Reflection;

namespace NFTAdminApplication.Utility
{
    /// <summary>
    /// Browser uploaded file handler
    /// </summary>
    public static class UploadFileHandler
    {
        /// <summary>
        /// Get the byte contents from an uploaded file
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public static byte[]? GetFileContents(IFormFile? formFile)
        {
            byte[]? content = null;
            if (formFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    content = ms.ToArray();
                }
            }

            return content;
        }

        /// <summary>
        /// Get the file extention of upload file
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public static string? GetFileExtention(IFormFile? formFile)
        {
            string? ext = null;

            if (formFile != null)
                ext = Path.GetExtension(formFile.FileName);

            return ext;
        }
    }
}
