using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Actions;
using Application.Common.Model;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Models;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PBXController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PBXController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetCurrentSMSs")]
        public async Task<List<SMS>> GetCurrentSMSs() {
            return await _mediator
                .Send(new GetRecentSMSs())
                .ConfigureAwait(true);
        }

        [HttpGet("GetListOfMobileSMSs")]
        public async Task<List<MobileSMSDetails>> GetListOfMobileSMSs()
        {
            return await _mediator
                .Send(new GetListOfMobileSMSs())
                .ConfigureAwait(true);
        }

        
        [HttpGet("GetSMSsForNumber")]
        public async Task<List<SMS>> GetSMSsForNumber(string Mobile)
        {
            return await _mediator
                .Send(new GetSMSsForNumber(Mobile))
                .ConfigureAwait(true);
        }

        [HttpPost("MarkSMSAsRead")]
        public async Task<bool> MarkSMSAsRead([FromBody] int Id)
        {
            return await _mediator
                .Send(new MarkSMSAsRead(Id))
                .ConfigureAwait(true);
        }

        [HttpPost("ReplyToSMS")]
        public async Task<bool> ReplyToSMS(ReplytoSMSRequest reply)
        {
            //return true;
            return await _mediator
                .Send(new ReplyToSMS(reply.ReplyToId, reply.Reply))
                .ConfigureAwait(true);
        }




    }
}
