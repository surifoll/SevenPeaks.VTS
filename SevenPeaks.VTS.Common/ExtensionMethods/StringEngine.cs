namespace SevenPeaks.VTS.Common.ExtensionMethods
{
    public static class StringEngine
    {
        public static string Cleanup(this string value){
            return value?.Replace(" ", "").Trim();
        } 
    }
}