using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.MellatApp
{
   public class MellatRefundService
    {
        ServiceReference1.TransactionService bService;
        public async Task<string> GetCardNumber(long refNo)
        {
            ServiceReference1.TransactionServiceClient SOAP = new ServiceReference1.TransactionServiceClient();
       

            var gg = await bService.getTransactionByIdFromArchiveAsync(refNo);

            return gg;

        }

    }
}
