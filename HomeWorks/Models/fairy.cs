namespace HomeWorks.Models
{
    public static class fairy
    {
        public static string GenerateUniqueId()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var random = new Random();
            var randomSuffix = random.Next(100, 999); // 生成3位隨機數字
            return $"{timestamp}{randomSuffix}";
        }

        public static string CreartOrderId()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random();
            var randomSuffix = random.Next(100000, 999999); // 生成3位隨機數字
            return $"{timestamp}{randomSuffix}";
        }

        public static string GenerateRandomId(int minLength, int maxLength)
        {
            var random = new Random();
            int length = random.Next(minLength, maxLength + 1);
            var id = new char[length];
            for (int i = 0; i < length; i++)
            {
                id[i] = (char)random.Next('0', '9' + 1);
            }
            return new string(id);
        }
    }
}
