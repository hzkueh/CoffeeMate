using Microsoft.AspNetCore.Mvc;

namespace CoffeeMate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase;
