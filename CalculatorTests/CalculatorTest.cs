using Xunit;
using CalculatorLib;

namespace CalculatorTests
{
    public class CalculatorTest
    {
        private readonly Calculator _calculator = new Calculator();

        [Fact(DisplayName = "İki pozitif sayıyı topla")]
        public void Add_TwoPositiveNumbers_ReturnSum()
        {
            int result = _calculator.Add(5, 3);
            Assert.Equal(8, result);
        }

        [Fact(DisplayName = "Negatif sayıları topla")]
        public void Add_NegativeNumbers_ReturnCorrectSum()
        {
            int result = _calculator.Add(-5, 3);
            Assert.Equal(-2, result);
        }

        [Fact(DisplayName = "Çıkarma işlemi")]
        public void Subtract_TwoNumbers_ReturnDifference()
        {
            int result = _calculator.Subtract(10, 3);
            Assert.Equal(7, result);
        }

        [Fact(DisplayName = "Sıfır ile toplama")]
        public void Add_WithZero_ReturnSameNumber()
        {
            int result = _calculator.Add(5, 0);
            Assert.Equal(5, result);
        }
    }
}
