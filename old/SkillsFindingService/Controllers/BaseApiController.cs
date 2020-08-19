using SkillsFindingService.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SkillsFindingService.Controllers
{

    public class BaseApiController : ApiController
    {
        protected int CurrentUserId
        {
            get
            {
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                var userIdClaim = claimsIdentity?.FindFirst(x => x.Type.Equals(ClaimTypes.SerialNumber));

                if (!string.IsNullOrEmpty(userIdClaim?.Value))
                {
                    int userId;
                    if (int.TryParse(userIdClaim.Value, out userId))
                        return userId;
                }

                throw new Exception("User id is not available");
            }
        }

        protected async Task<IHttpActionResult> Wrap<MIn>(Func<MIn, Task> method, MIn inParam)
        {
            WWMResponse response = new WWMResponse();
            try
            {
                await method.Invoke(inParam);
            }
            catch (Exception ex)
            {
                response.SetError(ex);
            }

            return Ok(response);
        }
    }
}