namespace WallsCalculator.Services
{
    /// <summary>
    /// Калькулятор для расчета конструкции.
    /// </summary>
    public interface ICalculator<in TInput, out TOutput>
    {
        /// <summary>
        /// Получить расчет.
        /// </summary>
        /// <param name="input">Входные данные.</param>
        public TOutput? Calculate(TInput input);
    }
}