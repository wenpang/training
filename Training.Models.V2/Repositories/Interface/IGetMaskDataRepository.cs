using System.Collections.Generic;
using System.Threading.Tasks;
using Training.Models.V2.Models;

namespace Training.Models.V2.Repositories.Interface
{
    public interface IGetMaskDataRepository
    {
        Task<List<MaskDataModel>> GetMaskData();
    }
}
