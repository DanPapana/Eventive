using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAWEventive.ApplicationLogic.Services;
using PAWEventive.Models.Events;
using System;
using System.Collections.Generic;

namespace PAWEventive.Controllers
{
    public class EventController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly EventService eventService;
        
        public EventController(UserManager<IdentityUser> userManager, EventService eventService)
        {
            this.userManager = userManager;
            this.eventService = eventService;
        }

        public ActionResult Index()
        {
            try
            {
                /*
                List<EventViewModel> eventViewModels = new List<EventViewModel>();

                foreach (var oneEvent in eventService.GetCurrentEvents())
                {
                    eventViewModels.Add(new EventViewModel
                    {
                        Title = oneEvent.Title,
                        EventCreatorId = oneEvent.CreatorId,
                        EventDetails = oneEvent.EventDetails,
                        Category = oneEvent.Category
                    });
                }
                */

                var userId = userManager.GetUserId(User);
                
                EventListViewModel viewModel = new EventListViewModel()
                {
                    EventList = eventService.GetCurrentEvents()
                };
                return View(viewModel);
            }
            catch (Exception)
            {
                return BadRequest("Invalid request received ");
            }
        }
    }
}