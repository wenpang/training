using Training.Models.V2.Models;
using Training.Models.V2.Repositories.Interface;
using Training.Models.V2.Tool;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Training.Models.V2.Repositories
{
    public class GetMaskDataRepository : IGetMaskDataRepository
    {
        private readonly HttpClient httpClient;

        public GetMaskDataRepository()
        {
            httpClient = new HttpClient(new RetryHandler(new HttpClientHandler()));
            httpClient.BaseAddress = new Uri(Setting.GovOpenData);// 設定連線的基底位址
            httpClient.Timeout = TimeSpan.FromMilliseconds(30000); // HttpClient連接遇時設定

            #region 設定Http請求(Request)標頭(Header)欄位的預設值
            httpClient.DefaultRequestHeaders.Add("Authorization", "99730cdd75884083aa9fae0b9e05c030");// HTTP中的基本認證設定，用於辨識使用者身分及控管權限等作用
            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");// 設定回傳內容類型，application/json意旨內容類型為JSON格式
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");// 用來表示使用者將返回的Response Body可能知道的內容編碼形式，通常為壓縮演算法
            #endregion

            ServicePointManager.DefaultConnectionLimit = 150;// 設定最高允許同時連線數量的預設值
        }

        public async Task<List<MaskDataModel>> GetMaskData()
        {
            HttpResponseMessage data = await httpClient.GetAsync("?nid=116285&md5_url=53a72b2dcfdd9ecae43afda4b86089be");
            List<MaskDataModel> maskData = JsonSerializer.Deserialize<List<MaskDataModel>>(await data.Content.ReadAsStringAsync());
            return maskData;
        }
    }
}
