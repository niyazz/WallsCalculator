namespace WallsCalculator.Models
{
    /// <summary>
    ///  Файл.
    /// </summary>
    public class HttpFileContent
    {
        /// <summary>
        /// Название с расширением.
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// Содержимое.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Тип содержимого.
        /// </summary>
        public string ContentType { get; set; }
    }
}