using System;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Data.Services;
using Stripe;
using LibraryApp.Data.ViewModels;
using System.Linq;

namespace LibraryApp.Controllers
{
    public class StripeDashboardController : Controller
    {
        public StripeDashboardController()
        {
        }

        public IActionResult Index()
        {
            var response = new StripeDashboardVM();

            var balanceService = new BalanceService();
            var balanceResult = balanceService.Get();
            response.Balance = balanceResult;

            var transactionService = new BalanceTransactionService();
            var transactionResult = transactionService.List().ToList();
            response.Transactions = transactionResult;

            var customerService = new CustomerService();
            var customerResult = customerService.List().ToList();
            response.Customers = customerResult;

            var chargeService = new ChargeService();
            var chargeResult = chargeService.List().ToList();
            response.Charges = chargeResult;

            var disputeService = new DisputeService();
            var disputeResult = disputeService.List().ToList();
            response.Disputes = disputeResult;

            var refundService  = new RefundService();
            var refundResult = refundService.List().ToList();
            response.Refunds = refundResult;

            var productService  = new ProductService();
            var productResult = productService.List().ToList();
            response.Products = productResult;

            return View(response);
        }
    }
}
