namespace UnityGeneral
{
    /// <summary>
    /// Функции для конвертирование чисел в текст.
    /// </summary>
    public static class ValuesFormatter
    {
        /// <summary>
        /// Если число больше 1000 - округляет его до вида x.xxК
        /// </summary>
        public static string AsMoneyText(this float value)
        {
            if (value <= 1000.0f) return ((int)value).ToString();
            float converted = value / 1000.0f;

            return converted.ToString("0.00") + "K";
        }
        /// <summary>
        /// Отображает число в виде процентов(1.0f = 100%)
        /// </summary>
        public static string AsPercentText(this float value)
        {
            int converted = (int)(value * 100.0f);

            return converted.ToString() + "%";
        }
    }
}