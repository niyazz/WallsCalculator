using System.ComponentModel;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Входные данные работника.
    /// </summary>
    public class WorkerInput
    {
        /// <summary>
        ///  Количество работников по такой цене.
        /// </summary>
        [DisplayName("Число рабочих за указанную оплату труда")]
        public int QuantityOfWorkers { get; set; }
        
        /// <summary>
        /// Оплата труда работаника.
        /// </summary>
        [DisplayName("Оплата труда")]
        public decimal Price { get; set; }
    }
}