using System.ComponentModel;

namespace WallsCalculator.Models.Shared
{
    public class CalculationOutput
    {
        /// <summary>
        /// Кол-во изделия.
        /// </summary>
        public int TotalMaterialAmount { get; set; }
        
        /// <summary>
        /// Общая площадь.
        /// </summary>
        public double TotalArea { get; set; }
        
        /// <summary>
        /// Площадь возведения стен.
        /// </summary>
        public double AreaToCoverSquareM { get; set; }
            
        /// <summary>
        /// Площадь ненуждающаяся в возвредении стен.
        /// </summary>
        public double AreaToNotCoverSquareM { get; set; }
            
        /// <summary>
        /// Стоимость найма всех работников.
        /// </summary>
        public decimal AllWorkersPrice { get; set; }
            
        /// <summary>
        /// Толщина стены.
        /// </summary>
        public double WallDepthCentimeters { get; set; }

        /// <summary>
        /// Цена за весь материал стены.
        /// </summary>
        public decimal TotalMaterialPrice { get; set; }
        
        /// <summary>
        /// Итоговая цена с наймом строителей.
        /// </summary>
        public decimal TotalMaterialAndWorkersPrice { get; set; }
    }
}