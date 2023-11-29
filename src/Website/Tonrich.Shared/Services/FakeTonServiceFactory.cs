namespace Tonrich.Shared.Services.Implementations
{
    public static class FakeTonServiceFactory
    {
        public static ITonService Random1Year()
        {
            var list = new List<WalletActivityDto>();


            var startDate = DateTimeOffset.Now.AddMonths(-6);
            var endDate = DateTimeOffset.Now;
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                Random random = new();
                double randomDecimal = random.NextDouble() * 2000;
                randomDecimal = Math.Round(randomDecimal, 1);
                list.Add(new WalletActivityDto { ActivityDate = date, ActivityAmount = (decimal)randomDecimal });
            }
            var nfts = new List<string>()
            {
                "@koton​",
                "@latina​",
                "afshin.ton​",
                "+888 1010 1122​",
                "@koton​",
                "@latina​",
                "afshin.ton​",
                "+888 1010 1122​"
            };
            //var tonService = new FakeTonService(new List<WalletDto>()
            //{
            //    new()
            //    {
            //        Id = "7a6ffed9-4252-427e-af7d-3dcaaf2db2df",
            //        Balance = 2500.2m,
            //        Activities = list,
            //        Nfts =nfts,
            //        Transaction = new TransactionInfoDto
            //        {
            //            DepositRate= 122.2m,
            //            DepositLastMonth =10m,
            //            SpentRate = 201.12m,
            //            SpentLastMonth = 12.12m
            //        },
            //        Nft = 1232.2m,
            //        Worth = 901.2m
            //    }
            //});
            var tonService = new FakeTonService();
            return tonService;
        }
    }
}
