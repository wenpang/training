using Training.Models.V2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Training.Models.V2.Domain.Interface
{
    public interface IGetMaskDataDomain
    {
        Task<List<MaskDataModel>> GetMaskData();
    }
}
