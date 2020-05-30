using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Data.Services;
using LibraryApp.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace LibraryApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IBookService _bookService;

        public CheckoutController(IBookService courseService)
        {
            _bookService = courseService;
        }

        public IActionResult Purchase(Guid id)
        {

            var book = _bookService.GetById(id);
            if (book == null) return NotFound();

            ViewBag.PurchaseAmount = book.Price;

            var data = new BookPurchaseVM
            {
                Id = book.Id,
                Description = book.Description,
                Author = book.Author,
                Thumbnail = book.Thumbnail,
                Title = book.Title,
                Price = book.Price,
                Nonce = ""
            };

            return View(data);
        }

        [HttpPost]
        public IActionResult Create(string stripeToken, Guid id)
        {
            var book = _bookService.GetById(id);

            var chargeOptions = new ChargeCreateOptions()
            {
                Amount = (long) (Convert.ToDouble(book.Price) * 100),
                Currency = "usd",
                Source = stripeToken,
                Metadata = new Dictionary<string, string>()
                {
                    { "BookId", book.Id.ToString() },
                    { "BookName", book.Title },
                    { "BookAuthor", book.Author },
                }
            };

            var service = new ChargeService();
            var charge = service.Create(chargeOptions);

            if (charge.Status == "succeeded")
            {
                return View("Success");
            }
            return View("Failure");
        }

        public IActionResult LoadAllPlans()
        {
            var service = new PlanService();
            var allPlans = service.List().ToList();

            return View(allPlans);
        }

        public IActionResult SubscribeToPlan(string id)
        {
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = "cus_HN5xdKA9s9FDUl",
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Plan = id
                    }
                }
            };

            var service = new SubscriptionService();
            var subscription = service.Create(subscriptionOptions);

            if (subscription.Created != null)
            {
                return View("Subscribed");
            }
            return View("NotSubscribed");
        }
    }
}