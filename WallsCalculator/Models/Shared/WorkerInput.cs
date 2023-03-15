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
        /// Оплата труда работаника в день.
        /// </summary>
        [DisplayName("Оплата дня работы")]
        public decimal Price { get; set; }
        
        /// <summary>
        ///  Число дней найма работника.
        /// </summary>
        [DisplayName("Число дней найма работника")]
        public int DurationInDays { get; set; }
    }
}