using System;
using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            if (windowWidth <= 0)
                throw new ArgumentException("Window width must be greater than 0.", nameof(windowWidth));

            Queue<DataPoint> queue = new Queue<DataPoint>();
            double currentSum = 0;

            foreach (var point in data)
            {
                queue.Enqueue(point);
                currentSum += point.Value;

                if (queue.Count > windowWidth)
                {
                    // Убираем элемент, который выходит из окна
                    var removedPoint = queue.Dequeue();
                    currentSum -= removedPoint.Value;
                }

                // Среднее значение для текущего окна
                double average = currentSum / queue.Count;

                yield return new DataPoint(point.Timestamp, average);
            }
        }
    }

    // Пример класса DataPoint
    public class DataPoint
    {
        public DateTime Timestamp { get; }
        public double Value { get; }

        public DataPoint(DateTime timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }
    }

    // Точка входа программы
    class Program
    {
        static void Main(string[] args)
        {
            var data = new List<DataPoint>
            {
                new DataPoint(DateTime.Now.AddSeconds(1), 1),
                new DataPoint(DateTime.Now.AddSeconds(2), 2),
                new DataPoint(DateTime.Now.AddSeconds(3), 3),
                new DataPoint(DateTime.Now.AddSeconds(4), 4),
                new DataPoint(DateTime.Now.AddSeconds(5), 5)
            };

            // Вызываем метод скользящего среднего с окном размера 3
            var movingAverage = data.MovingAverage(3); 

            // Выводим результаты
            foreach (var point in movingAverage)
            {
                Console.WriteLine($"Timestamp: {point.Timestamp}, Moving Average: {point.Value}");
            }
        }
    }
}
