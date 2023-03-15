using System.ComponentModel;
using WallsCalculator.Models.Shared;

namespace WallsCalculator.Models
{
    public class BalkCalculationOutput : CalculationOutput
    {
        /// <summary>
        /// Данные для расчета.
        /// </summary>
        public BalkCalculationInput Input { get; set; }
        
        /// <summary>
        /// Объем бруса в куб.метрах.
        /// </summary>
        public double BalkVolumeCubeM { get; set; }

        /// <summary>
        /// Количество шт. бруса в одном куб.метре.
        /// </summary>
        public int OneCubeBalkAmount { get; set; }

        /// <summary>
        /// Объем бруса на дом в куб.метре.
        /// </summary>
        public double AreaToCoverCubeM { get; set; }

        /// <summary>
        /// Вес конструкции из бруса.
        /// </summary>
        public double ConstructionWeight { get; set; }
        
        /// <summary>
        /// Число рядов бруса
        /// </summary>
        public double BalkRowsAmount { get; set; }
    }
}