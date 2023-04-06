﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Hellpers.Filter;

namespace WebApi.Controllers
{
    [ApiController]
    [CustomAuthorize]
    [ValidateModel]
    [Route("[controller]")]

    public class BaseApiController : ControllerBase
    {
    }
}
