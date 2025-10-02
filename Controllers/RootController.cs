
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;
[ApiController]
[Route("api")]
public class RootController : ControllerBase
{
    private readonly EndpointDataSource _endpointDataSource;
    private readonly LinkGenerator _linkGenerator;

    public RootController(EndpointDataSource endpointDataSource, LinkGenerator linkGenerator)
    {
        _endpointDataSource = endpointDataSource;
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    public IActionResult GetAllUrls()
    {
        var urls = _endpointDataSource.Endpoints
            .OfType<RouteEndpoint>()
            .Select(ep => new
            {
                Route = ep.RoutePattern.RawText,
                HttpMethods = ep.Metadata.OfType<HttpMethodMetadata>()
                                .FirstOrDefault()?.HttpMethods
            })
            .Where(ep => ep.Route != null) // filter out non-route endpoints
            .ToList();

        return Ok(urls);
    }
}
