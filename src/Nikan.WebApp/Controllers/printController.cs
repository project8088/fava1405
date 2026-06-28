using cle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nikan.Services;
using Nikan.Services.CitizenCards;
using Nikan.Services.Citizens;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Nikan.WebApp.Controllers
{
    public class printController : Controller
    {
        #region Ctor
        public static IHostingEnvironment _environment;
        private readonly IUsersService _usersService; 
        private readonly ISiteSettingService _siteSettingService; 
        private readonly ICardService _card;
        private readonly ICitizenCardService _citizencard;
        private readonly ICardInfoExportService _cardInfoExport;
        private readonly ICitizenService _citzene;
        private readonly IDistributeCardService _distributeCardService;

        public printController( 
            ISiteSettingService siteSettingService,
              ICitizenService citzene,
               IDistributeCardService distributeCardService,
              ICardInfoExportService cardInfoExport,
              ICitizenCardService citizencard,
            IHostingEnvironment environment,
              IUsersService userManager,
               ICardService card 

            )
        {
            _environment = environment;
            _citizencard = citizencard;
            _card = card;
            _citzene = citzene;
            _cardInfoExport = cardInfoExport;
            _siteSettingService = siteSettingService;
            _distributeCardService = distributeCardService;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));

        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        
        [AllowAnonymous]
        public IActionResult QueueForPost(string queueId,int? printType)
        { 
            if(string.IsNullOrWhiteSpace(queueId) || printType==null)
            {
                return Redirect("/");
            }
            HttpContext.Session.SetString("queueId", queueId); 
            HttpContext.Session.SetInt32("printType", printType.Value);
            return View();
        }


        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetQueueForPostReport()
        {
            var report = new StiReport();
            try
            {
              
                var id = HttpContext.Session.GetString("queueId");
                if (id == null) return StiNetCoreViewer.GetReportResult(this, report);

                var printType = HttpContext.Session.GetInt32("printType");
                if (printType == null) return StiNetCoreViewer.GetReportResult(this, report);



                var filesPath = _environment.WebRootPath;
                if (printType == 1)
                    filesPath += @"\Resources\Reports\ReportFormtManzalat.mrt";//فرمت چاپ طرح منزلت
                else if (printType == 2)
                    filesPath += @"\Resources\Reports\PrintQueueCard.mrt";//فرمت چاپ کارت شهروندی
                else if (printType == 3)
                    filesPath += @"\Resources\Reports\ListToPost.mrt";//فرمت تحویل لیست به پست
                else if (printType == 4)
                    filesPath += @"\Resources\Reports\ListToCenter.mrt";//فرمت تحویل لیست به مرکز
                else if (printType == 5)
                    filesPath += @"\Resources\Reports\ListTahvilPostBeCitizen.mrt";//فرمت تحویل پست به شهروند
                else 
                    filesPath += @"\Resources\Reports\ListBarCode.mrt";//فرمت لیست بارکد



                report.Load(filesPath);
                var res = await _distributeCardService.GetPrintQueueForPostList(id);
                if (res.IsSuccess)
                {
                    var data = res.Data;
                    var groupName = "";
                    var queueName = "";
                    if (data.Any())
                    {
                        queueName = data.FirstOrDefault().QueueName;
                    }
                    report.RegBusinessObject("PrintCardForPost", res.Data); //  
                    report.Dictionary.Variables.Add("Date", DateTime.Now.ToShortDateString());
                    report.Dictionary.Variables.Add("GroupName", groupName);
                    report.Dictionary.Variables.Add("QueueName", queueName);
                    report.Dictionary.Variables.Add("RowCount", data.Count);
                    report.Dictionary.SynchronizeBusinessObjects(2);
                }

            }
            catch (Exception er)
            {

               
            }
           
            return StiNetCoreViewer.GetReportResult(this, report);

            
        }



    


        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public virtual ActionResult PrintReport()
        {
            return StiNetCoreViewer.PrintReportResult(this);
        }

        public virtual ActionResult ExportReport()
        {
            return StiNetCoreViewer.ExportReportResult(this);
        }



         
    }
}