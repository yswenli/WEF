namespace WEF.MvcPager
{
    public class PagerItem
    {
        public PagerItem(string text, int pageIndex, bool disabled, PagerItemType type)
        {
            Text = text;
            PageIndex = pageIndex;
            Disabled = disabled;
            Type = type;
        }

        public string Text { get; set; }
        public int PageIndex { get; set; }
        public bool Disabled { get; set; }
        public PagerItemType Type { get; set; }
    }

    public enum PagerItemType : byte
    {
        FirstPage,
        NextPage,
        PrevPage,
        LastPage,
        MorePage,
        NumericPage
    }
}
