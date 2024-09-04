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
    }
}
