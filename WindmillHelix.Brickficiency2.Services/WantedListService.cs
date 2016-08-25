using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.Services
{
    internal class WantedListService : IWantedListService
    {
        private readonly IBricklinkWantedListApi _bricklinkWantedListApi;

        public WantedListService(IBricklinkWantedListApi bricklinkWantedListApi)
        {
            _bricklinkWantedListApi = bricklinkWantedListApi;
        }

        public IReadOnlyCollection<WantedListItem> GetWantedListItems(int wantedListId)
        {
            return _bricklinkWantedListApi.GetWantedListItems(wantedListId);
        }

        public IReadOnlyCollection<WantedList> GetWantedLists()
        {
            return _bricklinkWantedListApi.GetWantedLists();
        }
    }
}
