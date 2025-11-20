namespace Cooktel_E_commrece.Helper
{
    public class UserParams
    {
        public int PageNumber { get; set; }

        private int _PageSize = 10;
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value>50)? 50:value;
        }
    }
}
