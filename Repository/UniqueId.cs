namespace Repository
{
    sealed class UniqueId
    {
        public string GenerateForLog()
        {
            return new Random().Next(100, 999) + " - " + DateTime.Now.ToString("dd-MMM-yyyy_HH-mm-ss-fff");
        }

        public string GenerateForDatabase()
        {
            return new Random().Next(100, 999) + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
        }
    }
}
