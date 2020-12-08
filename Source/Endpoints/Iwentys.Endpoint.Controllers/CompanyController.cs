﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Companies.Models;
using Iwentys.Features.Companies.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CompanyInfoDto>> Get()
        {
            return Ok(_companyService.Get());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyInfoDto>> Get(int id)
        {
            CompanyInfoDto company = await _companyService.Get(id);
            return Ok(company);
        }
    }
}