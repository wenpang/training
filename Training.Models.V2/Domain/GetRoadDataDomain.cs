using System.Collections.Generic;
using Training.Models.V2.Models;
using Training.Models.V2.Repositories;

namespace Training.Models.V2.Domain
{
    public class GetRoadDataDomain
    {
        public List<RoadDataModel> GetRoadData()
        {
            GetRoadDataRepository repo = new GetRoadDataRepository();
            return repo.GetRoadData();
        }
    }
}
