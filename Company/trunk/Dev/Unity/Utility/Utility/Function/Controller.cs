namespace Utility.Function
{
    public abstract class Controller
    {
        public abstract int Count {
            get;
        }

        public interface IRefresh
        {
            void refresh(int index);
        }
        public IRefresh refresh;

        public void setIRefresh(IRefresh i)
        {
            refresh = i;
        }




        private int index;
        public int Index
        {
            get
            {
                return index;
            }
        }

        public void indexClear()
        {
            indexChanged(0);
        }
        public void indexChanged(int i)
        {
            index = i;

            if (refresh != null)
            {
                refresh.refresh(index);
            }

        }

        public void indexUp()
        {
            //var v = VideoXMLDataList.lists;

            index++;
            //if (Index >= v.Count)
            if(index >= Count)
            {
                index = 0;
            }
            indexChanged(Index);
        }
        public void indexDown()
        {
            index--;
            if (index < 0)
            {
                //index = VideoXMLDataList.lists.Count - 1;
                index = Count - 1;
            }
            indexChanged(Index);

        }
    }


}
