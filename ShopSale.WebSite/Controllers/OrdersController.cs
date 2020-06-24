namespace ShopSale.WebSite.Controllers
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
    using Data.Interfaces;
    using Models;

    [Authorize]
	public class OrdersController : Controller
	{
		private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository orderRepository,
                                IProductRepository productRepository)
		{
			this._orderRepository = orderRepository;
            this._productRepository = productRepository;
        }

		public async Task<IActionResult> Index()
		{
			var model = await this._orderRepository.GetOrdersAsync(this.User.Identity.Name);
			return View(model);
		}

		public async Task<IActionResult> Create()
		{
			var model = await this._orderRepository.GetDetailTempsAsync(this.User.Identity.Name);
			return this.View(model);
		}

		public IActionResult AddProduct()
		{
			var model = new AddItemViewModel
			{
				Quantity = 1,
				Products = this._productRepository.GetComboProducts()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AddProduct(AddItemViewModel model)
		{
			if (this.ModelState.IsValid)
			{
				await this._orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
				return this.RedirectToAction("Create");
			}

			return this.View(model);
		}

		public async Task<IActionResult> DeleteItem(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			await this._orderRepository.DeleteDetailTempAsync(id.Value);
			return this.RedirectToAction("Create");
		}

		public async Task<IActionResult> Increase(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			await this._orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1);
			return this.RedirectToAction("Create");
		}

		public async Task<IActionResult> Decrease(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			await this._orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);
			return this.RedirectToAction("Create");
		}

		public async Task<IActionResult> ConfirmOrder()
		{
			var response = await this._orderRepository.ConfirmOrderAsync(this.User.Identity.Name);
			if (response)
			{
				return this.RedirectToAction("Index");
			}

			return this.RedirectToAction("Create");
		}

		public async Task<IActionResult> Deliver(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var order = await this._orderRepository.GetOrdersAsync(id.Value);
			if (order == null)
			{
				return NotFound();
			}

			var model = new DeliverViewModel
			{
				Id = order.Id,
				DeliveryDate = DateTime.Today
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Deliver(DeliverViewModel model)
		{
			if (this.ModelState.IsValid)
			{
				await this._orderRepository.DeliverOrder(model);
				return this.RedirectToAction("Index");
			}

			return this.View();
		}

	}
}
