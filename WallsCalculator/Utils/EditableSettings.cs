namespace WallsCalculator.Utils
{
    /// <summary>
    /// Изменяемые настройки.
    /// </summary>
    public class EditableSettings
    {
        /// <summary>
        /// Автор проекта.
        /// </summary>
        public string Author { get; set; }
        
        /// <summary>
        /// Название документа для бруса.
        /// </summary>
        public string BalkDocumentName { get; set; }
        
        /// <summary>
        /// Название документа для кирпича.
        /// </summary>
        public string BrickDocumentName { get; set; }
        
        /// <summary>
        /// Название документа для блока.
        /// </summary>
        public string BlockDocumentName { get; set; }
    }
}