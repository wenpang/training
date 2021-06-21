namespace Training.UnitTests.Models
{
    public class TicketModel
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 出發地
        /// </summary>
        public string Departure { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// 人數
        /// </summary>
        public int PeopleNum { get; set; }

        /// <summary>
        /// 總額
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
