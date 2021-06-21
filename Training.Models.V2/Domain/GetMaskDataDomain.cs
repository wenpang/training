using Training.Models.V2.Domain.Interface;
using Training.Models.V2.Models;
using Training.Models.V2.Repositories.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Training.Models.V2.Domain
{
    public class GetMaskDataDomain : IGetMaskDataDomain
    {
        private readonly IGetMaskDataRepository _repo;

        public GetMaskDataDomain(IGetMaskDataRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<MaskDataModel>> GetMaskData()
        {
            return await _repo.GetMaskData();
        }
    }
}
