using Hangfire;
using HangfireSulotion.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangfireSulotion.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HangFireController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IHangFireServices _hangFireServices;

        public HangFireController(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IHangFireServices hangFireServices)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
            _hangFireServices = hangFireServices;
        }




        //should call to fire
        [HttpGet]
        public async Task<IActionResult> FireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _hangFireServices.FireAndForgetJob());
            return Ok();
        }

        // should call to fire after 5 seconds
        [HttpGet]
        public async Task<IActionResult> CreatedDelayedJob()
        {

            _backgroundJobClient.Schedule(() => _hangFireServices.DelayedJob(), TimeSpan.FromSeconds(5));
            return Ok();
        }

        //No Need To be Called to Run
        [HttpGet]
        public async Task<IActionResult> ReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _hangFireServices.ReccuringJob(), Cron.Minutely);
            //you can create your custom cron see documents
            return Ok();
        }

        //with this type you can fire a job then after it run another job which is like child of previous job with that id in hangfire dashboard
        [HttpGet]
        public async Task<IActionResult> CreateContinuationJob()
        {
            var parentjobId = _backgroundJobClient.Enqueue(() => _hangFireServices.FireAndForgetJob());
            _backgroundJobClient.ContinueJobWith(parentjobId, () => _hangFireServices.ContinuationJob());
            return Ok();
        }

        //Notice: HangFire Need A DataBase To Create Some Nedded Tabeles to Save Its Job
        //So If HangFire goes Off As Soon As It Goes On Bsck It Will Excute Failed Jobs which Saved in DataBase;

    }
}
