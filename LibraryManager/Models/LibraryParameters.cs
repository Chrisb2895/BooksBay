namespace LibraryManager
{
    public class LibraryParameters
    {
        const int maxPageSize = 50;

        public int PageNumber { get; set; }

        private int _pageSize = 10;

        public int PageSize

        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}