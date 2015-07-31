using GroupBrush.BL.Canvases;
using GroupBrush.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GroupBrush.Web.Public.Controllers
{
    public class CanvasController : ApiController
    {
        ICanvasService _canvasService;
        public CanvasController(ICanvasService canvasService)
        {
            _canvasService = canvasService;
        }

        [Route("api/canvas")]
        [HttpPost]
        public Guid? CreateCanvas(CanvasDescription canvasDescription)
        {
            Guid? canvasId = null;
            if (canvasDescription != null)
            {
                canvasId = _canvasService.CreateCanvas(canvasDescription);
            }
            return canvasId;
        }

        [Route("api/canvas")]
        [HttpPut]
        public Guid? LookUpCanvas(CanvasName canvasName)
        {
            Guid? canvasId = null;
            if (canvasName != null)
            {
                canvasId = _canvasService.LookUpCanvas(canvasName.Name);
            }
            return canvasId;
        }
    }
}
