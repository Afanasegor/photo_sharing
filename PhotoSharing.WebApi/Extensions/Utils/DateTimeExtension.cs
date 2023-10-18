namespace PhotoSharing.WebApi.Extensions.Utils
{
    public static class DateTimeExtension
    {
        public static string ToFullString(this DateTime dateTime)
        {
            var result = dateTime.Day.ToString() +
                            dateTime.Month.ToString() +
                            dateTime.Year.ToString() +
                            "_" +
                            dateTime.Hour.ToString() +
                            dateTime.Minute.ToString() +
                            dateTime.Second.ToString() +
                            dateTime.Millisecond.ToString();

            return result;
        }
    }
}
