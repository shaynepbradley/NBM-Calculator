namespace Alcor
{
    public class AlcorException :Exception
    {
        public AlcorException()
        {
        }

        public AlcorException(string msg) : base(msg)
        {
        }

        public AlcorException(string msg, Exception innerException) : base(msg, innerException)
        {
        }
    }
}
